using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Repositories;
using PGSyncro.Utils;
using PGSyncro.Validators;
using System;
using System.Threading.Tasks;

namespace PGSyncro
{
    public class SyncroTransaction
    {
        private readonly PaymentGatewayEntities _context;
        private readonly ILogger<SyncroTransaction> _logger;
        private readonly TransactionRepository _transactionRepository;
        private readonly PGConfiguration _pgConfiguration;
        private readonly SPSCalls _spsCalls; 

        public SyncroTransaction(
            PaymentGatewayEntities context,
            ILogger<SyncroTransaction> logger,
            TransactionRepository transactionRepository,
            PGConfiguration pgConfiguration,
            SPSCalls spsCalls)
        {
            _context = context;
            _logger = logger;
            _transactionRepository = transactionRepository;
            _pgConfiguration = pgConfiguration;
            _spsCalls = spsCalls;
        }

        public async Task<SyncroProcess> ProcesarTransaccionAsync(
          GetTransactionsToSync_Result transactionToWork,
          string hash = "",
          bool validadorNuevoDeDecidir = false)
        {
            var retorno = new SyncroProcess();
            AppConfig.stats.TotalTransactionsProcessed++;

            var respuestaDelValidador = await GetRespuestaDelValidadorAsync(transactionToWork, hash, validadorNuevoDeDecidir);
            retorno.Communication = respuestaDelValidador.QueryResponse;
            retorno.Status = await SaveStatusAsync(transactionToWork, respuestaDelValidador);

            return retorno;
        }

        public async Task<TransactionOriginal> GetRespuestaDelValidadorAsync(
        GetTransactionsToSync_Result transactionToWork,
        string hash = "",
        bool validadorNuevoDeDecidir = false)
        {
            TransactionOriginal respuestaDelValidador;
            if (!validadorNuevoDeDecidir) //DECIDIR
            {
                respuestaDelValidador = await _spsCalls.GetXMLServiceAsync(transactionToWork);
            }
            else
            {
                respuestaDelValidador = await _spsCalls.GetAPIServiceAsync(transactionToWork, hash);
            }
            return respuestaDelValidador;
        }

        public async Task<bool> MonitorCloseProcessAsync()
        {
            try
            {
                var monitor = await _context.MonitorSyncroProcesses.FindAsync(AppConfig.SincroId);

                if (monitor == null)
                {
                    await _pgConfiguration.InsertLogAsync(nameof(MonitorCloseProcessAsync), $"No se encontró el registro de monitoreo para SincroId: {AppConfig.SincroId}");
                    return false;
                }

                monitor.EndOn = DateTime.Now;
                monitor.TotalTransactionsClosed = AppConfig.stats.TotalTransactionsClosed;
                monitor.TotalTransactionsClosedError = AppConfig.stats.TotalTransactionsClosedERROR;
                monitor.TotalTransactionsClosedNa = AppConfig.stats.TotalTransactionsClosedNA;
                monitor.TotalTransactionsClosedNabyTimeout = AppConfig.stats.TotalTransactionsClosedNAByTIMEOUT;
                monitor.TotalTransactionsClosedNo = AppConfig.stats.TotalTransactionsClosedNO;
                monitor.TotalTransactionsClosedOk = AppConfig.stats.TotalTransactionsClosedOK;
                monitor.TotalTransactionsNotClosed = AppConfig.stats.TotalTransactionsNotClosed;
                monitor.TotalTransactionsProcessed = AppConfig.stats.TotalTransactionsProcessed;
                monitor.TotalTransactionsWithError = AppConfig.stats.TotalTransactionsWithError;
                monitor.UpdatedBy = AppConfig.appVersion;
                monitor.UpdatedOn = DateTime.Now;
                monitor.WithProcessError = AppConfig.witherror;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _pgConfiguration.InsertLogAsync(nameof(MonitorCloseProcessAsync), $"No se pudo cerrar el proceso MonitorSyncroProcessId {AppConfig.SincroId}", ex);
                _logger.LogError(ex, "Error F02: No se pudo cerrar el proceso de monitoreo.");
                return false;
            }
        }

        public async Task<bool> MonitorCloseTransactionAsync(
          long monitorSyncroProcessRecordsId,
          SyncroProcess syncro)
        {
            try
            {
                if (syncro.Status.ItWasClose)
                    AppConfig.stats.TotalTransactionsClosed++;
                else
                    AppConfig.stats.TotalTransactionsNotClosed++;

                string error = GetErrors(syncro);
                var monitorTrans = await _context.MonitorSyncroProcessRecords.FindAsync(monitorSyncroProcessRecordsId);

                if (monitorTrans == null)
                {
                    await _pgConfiguration.InsertLogAsync(nameof(MonitorCloseTransactionAsync), $"No se encontró el registro de monitoreo para MonitorSyncroProcessRecordsId: {monitorSyncroProcessRecordsId}");
                    return false;
                }

                if (syncro.Status?.ItWasClose == true)
                    monitorTrans.ClosedByProcess = DateTime.Now;

                monitorTrans.Error = error;
                monitorTrans.EndProcess = DateTime.Now;
                monitorTrans.HasError = error != null;
                monitorTrans.UpdatedBy = AppConfig.appVersion;
                monitorTrans.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _pgConfiguration.InsertLogAsync(nameof(MonitorCloseTransactionAsync), $"No se pudo cerrar la transacción MonitorSyncroProcessRecordsId {monitorSyncroProcessRecordsId}", ex);
                _logger.LogError(ex, "Error F03: No se pudo cerrar la transacción de monitoreo.");
            }
            return false;
        }

        public async Task<bool> MonitorBeginTransactionAsync(long monitorSyncroProcessRecordsId)
        {
            try
            {
                var monitorTrans = await _context.MonitorSyncroProcessRecords.FindAsync(monitorSyncroProcessRecordsId);
                if (monitorTrans == null)
                {
                    await _pgConfiguration.InsertLogAsync(nameof(MonitorBeginTransactionAsync), $"No se encontró el registro de monitoreo para MonitorSyncroProcessRecordsId: {monitorSyncroProcessRecordsId}");
                    return false;
                }
                monitorTrans.BeginProcess = DateTime.Now;
                monitorTrans.UpdatedBy = AppConfig.appVersion;
                monitorTrans.UpdatedOn = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _pgConfiguration.InsertLogAsync(nameof(MonitorBeginTransactionAsync), $"No se pudo abrir la transacción MonitorSyncroProcessRecordsId {monitorSyncroProcessRecordsId}", ex);
                _logger.LogError(ex, "Error F04: No se pudo iniciar la transacción de monitoreo.");
            }
            return false;
        }

        private async Task<SyncroSaveStatus> SaveStatusAsync(
      GetTransactionsToSync_Result transactionToWork, 
      TransactionOriginal transactionOriginal)
        {
            if (transactionOriginal.QueryResponse.HasError)
            {
                return new SyncroSaveStatus
                {
                    ErrorMessage = "Hubo error en la comunicación",
                    HasError = true,
                    ItWasClose = false,
                    StatusSaved = false,
                    StatusCodeId = -1 
                };
            }

            if (!transactionOriginal.QueryResponse.HasResponse)
            {
                return new SyncroSaveStatus
                {
                    ErrorMessage = "La respuesta se encuentra vacía en la comunicación",
                    HasError = true,
                    ItWasClose = false,
                    StatusSaved = false,
                    StatusCodeId = -1 
                };
            }

            if (transactionOriginal.QueryResponse.HasTransaction)
            {
                return await _transactionRepository.ProcessResultAsync(transactionToWork, transactionOriginal);
            }
            else
            {
                return await _transactionRepository.CerrarPorInexistenteEnValidadorAsync(transactionToWork.TransactionIdPK);
            }
        }

        private string GetErrors(SyncroProcess syncro)
        {
            if (syncro.Communication?.HasError == true || syncro.Status?.HasError == true)
            {
                AppConfig.stats.TotalTransactionsWithError++;
                return JsonConvert.SerializeObject(syncro);
            }
            return null;
        }
    }
}