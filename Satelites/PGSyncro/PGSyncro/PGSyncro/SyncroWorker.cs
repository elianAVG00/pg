using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Repositories;
using PGSyncro.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace PGSyncro
{
    public class SyncroWorker : BackgroundService
    {
        private readonly ILogger<SyncroWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _appVersion;

        public SyncroWorker(ILogger<SyncroWorker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _appVersion = $"PGSync v.{Assembly.GetExecutingAssembly().GetName().Version} [{WindowsIdentity.GetCurrent().Name}] [{Environment.UserName}]";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                bool log_output = _configuration.GetValue<bool>("AppSettings:log_output");
                DateTime fixHistoricalSyncro = DateTime.ParseExact(_configuration["AppSettings:FixHistoricalSyncro"], "yyyyMMdd", CultureInfo.InvariantCulture);

                _print(_appVersion, "OK", log_output);
                _print("INICIO DE PROCESO DE SINCRONIZACIÓN", "OK", log_output);

                var args = Environment.GetCommandLineArgs();
                bool fixHistoric = args.Contains("historico");

                if (fixHistoric)
                {
                    _print("PROCESO DE SINCRONIZACIÓN DE REPARACIÓN HISTÓRICA", "OK", log_output);
                    _print($"CONSIDERANDO DESDE: {fixHistoricalSyncro:yyyy-MM-dd} HASTA EL: {DateTime.Now:yyyy-MM-dd}", "OK", log_output);
                }

                _print("=================================================================", "OK", log_output);
                _print("PARAMETROS CONFIGURADOS", "OK", log_output);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var spsRepository = scope.ServiceProvider.GetRequiredService<SPSRepository>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<TransactionRepository>();
                    var pgConfiguration = scope.ServiceProvider.GetRequiredService<PGConfiguration>();
                    var syncroTransaction = scope.ServiceProvider.GetRequiredService<SyncroTransaction>();

                    AppConfig.codes = await transactionRepository.GetStatusCodeIdByValidatorCodeAsync();

                    var spsValidators = await spsRepository.GetSPSValidatorsAsync();
                    var securityConfiguration = await pgConfiguration.GetValidatorsSecurityConfigurationAsync();
                    AppConfig.servicesidWithConciliation = await pgConfiguration.GetServicesWithConciliationAsync();

                    IncompleteTransactionsModel transactionsToSync = await transactionRepository.GetTransactionsToSyncAsync(fixHistoric);

                    if (transactionsToSync.IsCompleted)
                    {
                        _print("OBTENER TRANSACCIONES INCOMPLETAS", "OK", log_output);

                        var header = transactionsToSync.Transactions.FirstOrDefault(ti => ti.TypeRow == 1);
                        if (header != null)
                        {
                            AppConfig.SincroId = header.MonitorSyncroProcessId.GetValueOrDefault();
                            AppConfig.TotalSincroFromUSP = header.MonitorSyncroProcessRecordsId.GetValueOrDefault();
                        }

                        _print($"PROCESO DE SINCRONIZACIÓN # {AppConfig.SincroId}", "OK", log_output);
                        _print($"TRANSACCIONES INCOMPLETAS A VERIFICAR: {AppConfig.TotalSincroFromUSP}", "OK", log_output);

                        long processedCount = 0;
                        foreach (var transaction in transactionsToSync.Transactions.Where(t => t.TypeRow == 2))
                        {
                            processedCount++;
                            string hashKey = securityConfiguration.FirstOrDefault(c => c.ValidatorId == transaction.ValidatorId && c.ServiceId == transaction.ServiceId)?.HashKey;

                            bool isNewDecidirValidator = false;
                            if (transaction.ValidatorId == 3)
                                isNewDecidirValidator = spsValidators.Any(v => v.ServiceID == transaction.ServiceId);

                            long recordId = transaction.MonitorSyncroProcessRecordsId ?? AppConfig.SincroId;

                            if (await syncroTransaction.MonitorBeginTransactionAsync(recordId))
                            {
                                SyncroProcess syncroResult = await syncroTransaction.ProcesarTransaccionAsync(transaction, hashKey, isNewDecidirValidator);

                                _print($"PROCESANDO [{processedCount}/{AppConfig.TotalSincroFromUSP}] - TIDPK: {transaction.TransactionIdPK} - Estado Final: {(syncroResult.Status.ItWasClose ? "CERRADA" : "NO CERRADA")} - Error: {syncroResult.Status.HasError}", "OK", log_output);
                               
                                if (fixHistoric)
                                    _print($"SINC. HIST. {processedCount} de {AppConfig.TotalSincroFromUSP} -TIDPK: {transaction.TransactionIdPK} -TN: {transaction.TransactionNumber} -VID: {transaction.ValidatorId} -SID: {transaction.ServiceId} -C: {syncroResult.Status.ItWasClose} -E: {syncroResult.Status.HasError} -SCI: {syncroResult.Status.StatusCodeId} ", "OK", log_output);

                                if (!await syncroTransaction.MonitorCloseTransactionAsync(recordId, syncroResult))
                                {
                                    _print("No se pudo cerrar la transaccion", "ERROR", log_output);
                                    await pgConfiguration.InsertLogAsync("Main", "No se pudo cerrar la transaccion");
                                }
                            }
                            else
                            {
                                _print("No se pudo comenzar el inicio de la transaccion", "ERROR", log_output);
                                await pgConfiguration.InsertLogAsync("Main", "No se pudo comenzar el inicio de la transaccion");
                            }
                        }
                    }
                    else
                    {
                        _print("OBTENER TRANSACCIONES INCOMPLETAS", "ERROR", log_output);
                        await pgConfiguration.InsertLogAsync("Main", "Error general al obtener transacciones para actualizar");
                    }

                    await syncroTransaction.MonitorCloseProcessAsync();
                    _print("SINCRONIZACIÓN COMPLETA", "OK", log_output);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F01: Fallo general en el sistema de sincronizacion");
            }

            IHostApplicationLifetime appLifetime = _serviceProvider.GetService<IHostApplicationLifetime>();
            appLifetime.StopApplication();
        }
        public void _print(string message, string result, bool log_output)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            string colorCode = "";
            switch (result)
            {
                case "OK":
                    colorCode = "339966";
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "NO":
                case "ERROR":
                    colorCode = "ff0000";
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "WARNING":
                    colorCode = "ff9900";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.Write(result.ToUpper());
            Console.ForegroundColor = ConsoleColor.White;
            string formattedMessage = string.Format("] [{1}] {0}", message.ToUpper(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.WriteLine(formattedMessage);
            string message1 = $"[{result}{formattedMessage}";

            if (log_output)
                _logger.LogInformation($"[{result}]{formattedMessage}");
        }
    }
}