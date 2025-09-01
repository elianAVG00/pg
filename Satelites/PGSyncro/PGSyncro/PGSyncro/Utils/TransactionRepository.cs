using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PGSyncro.Repositories
{
    public class TransactionRepository
    {
        private readonly PaymentGatewayEntities _context;
        private readonly ILogger<TransactionRepository> _logger;
        private readonly PGConfiguration _pgConfiguration;

        public TransactionRepository(
            PaymentGatewayEntities context,
            ILogger<TransactionRepository> logger,
            PGConfiguration pgConfiguration)
        {
            _context = context;
            _logger = logger;
            _pgConfiguration = pgConfiguration;
        }

        public async Task<IncompleteTransactionsModel> GetTransactionsToSyncAsync(bool fixhistoric)
        {
            var transactionsToSync = new IncompleteTransactionsModel();
            try
            {
                _context.Database.SetCommandTimeout(600);

                var parameter = new SqlParameter("@paramHistoric", fixhistoric);
                var transactions = await _context.Database
                    .SqlQueryRaw<GetTransactionsToSync_Result>("EXEC GetTransactionsToSync @paramHistoric", parameter)
                    .ToListAsync();

                transactionsToSync.Transactions = transactions;
                transactionsToSync.IsCompleted = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F14: Error al obtener transacciones para sincronizar.");
                transactionsToSync.IsCompleted = false;
            }
            return transactionsToSync;
        }

        private async Task SetStatusAsync(long idpk, int code)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SetStatus @p0, @p1, @p2",
                parameters: new object[] { idpk, code, AppConfig.appVersion }
            );
        }

        public async Task<SyncroSaveStatus> CerrarPorInexistenteEnValidadorAsync(long transactionIDPK)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                await SetStatusAsync(transactionIDPK, 52);
                AppConfig.stats.TotalTransactionsClosedNAByTIMEOUT++;
                retorno.HasError = false;
                retorno.StatusSaved = true;
                retorno.ItWasClose = true;
                retorno.StatusCodeId = 52;
                retorno.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F15: Error al cerrar por inexistente en validador.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
            }
            return retorno;
        }

        public async Task<SyncroSaveStatus> ProcessResultAsync(
          GetTransactionsToSync_Result transactionToWork,
          TransactionOriginal transactionData)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                var statusCode = GetStatusCodeIdByValidatorCode(transactionToWork.ValidatorId, transactionData.OriginalCode, transactionData.ModuleType);

                if (statusCode == null)
                {
                    retorno.HasError = true;
                    retorno.ErrorMessage = $"El codigo especificado para Validador {transactionToWork.ValidatorId}, Codigo {transactionData.OriginalCode}, Modulo {transactionData.ModuleType} no existe.";
                    retorno.StatusSaved = false;
                    retorno.ItWasClose = false;
                    retorno.StatusCodeId = -2;
                }
                else
                {
                    switch (statusCode.GenericCodeId)
                    {
                        case 1:
                            retorno = await AprobarTransaccionAsync(transactionData);
                            break;
                        case 2:
                            retorno = await CerrarTransaccionPorRechazoAsync(transactionToWork.TransactionIdPK, statusCode.StatusCodeId);
                            break;
                        case 3:
                            retorno = await CerrarTransaccionPorErrorGeneradoAsync(transactionToWork.TransactionIdPK, statusCode.StatusCodeId);
                            break;
                        case 4:
                            retorno = await ProcesarEnCursoAsync(transactionToWork);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F16: Error en ProcessResult.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }

        private async Task<SyncroSaveStatus> CerrarPorTimeOutAsync(long TransactionIdPK)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                await SetStatusAsync(TransactionIdPK, 51);
                AppConfig.stats.TotalTransactionsClosedNAByTIMEOUT++;
                retorno.HasError = false;
                retorno.ErrorMessage = null;
                retorno.StatusSaved = true;
                retorno.ItWasClose = true;
                retorno.StatusCodeId = 51;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F17: Error al cerrar por timeout.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }

        private async Task<SyncroSaveStatus> ProcesarEnCursoAsync(GetTransactionsToSync_Result transaccion)
        {
            int minutes = 3600; 

            switch (transaccion.ValidatorId)
            {
                case 3: // SPSDecidir
                    try
                    {
                        string timeoutString = await _pgConfiguration.GetConfigBySettingAsync("SPS_Timeout");
                        if (!string.IsNullOrEmpty(timeoutString) && int.TryParse(timeoutString, out int timeoutValue))
                        {
                            minutes = timeoutValue;
                        }
                        else
                        {
                            _logger.LogWarning("No se pudo obtener o parsear 'SPS_Timeout' desde la configuración. Usando valor por defecto de {DefaultMinutes} minutos.", minutes);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al obtener 'SPS_Timeout' desde la configuración. Usando valor por defecto.");
                    }
                    break;
            }

            if (DateTime.Now > transaccion.CreatedOn.AddMinutes(minutes))
            {
                return await CerrarPorTimeOutAsync(transaccion.TransactionIdPK);
            }
            else
            {
                return await ActualizarEstadoAEnCursoAsync(transaccion.TransactionIdPK);
            }
        }

        private async Task<SyncroSaveStatus> ActualizarEstadoAEnCursoAsync(long TransactionIdPK)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                await SetStatusAsync(TransactionIdPK, 1);
                AppConfig.stats.TotalTransactionsClosedNA++;
                retorno.HasError = false;
                retorno.ErrorMessage = null;
                retorno.StatusSaved = true;
                retorno.ItWasClose = false;
                retorno.StatusCodeId = 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F18: Error al actualizar a en curso.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }
        public Status GetStatusCodeIdByValidatorCode(int validatorId, string originalcode, string type)
        {
            try
            {
                return AppConfig.codes
                    .Where(sc => sc.type == type && sc.validatorId == validatorId && sc.originalcode == originalcode)
                    .Select(sc => new Status()
                    {
                        GenericCodeId = sc.GenericCodeId,
                        StatusCodeId = sc.StatusCodeId
                    })
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F19: Error al obtener código de estado desde la caché.");
                Task.Run(() => _pgConfiguration.InsertLogAsync(nameof(GetStatusCodeIdByValidatorCode), "Error al intentar obtener el codigo ", ex));
                return null;
            }
        }

        public async Task<List<StatusModuleCode>> GetStatusCodeIdByValidatorCodeAsync()
        {
            try
            {
                return await (from sc in _context.StatusCodes
                              join cm in _context.CodeMappings on sc.StatusCodeId equals cm.StatusCodeId
                              join modcode in _context.ModuleCodes on cm.ModuleCodeId equals modcode.ModuleCodeId
                              join mod in _context.Modules on modcode.ModuleId equals mod.Id
                              where mod.ValidatorId != null
                              select new StatusModuleCode
                              {
                                  type = mod.Type,
                                  validatorId = mod.ValidatorId.Value,
                                  originalcode = modcode.OriginalCode,
                                  GenericCodeId = sc.GenericCodeId,
                                  StatusCodeId = sc.StatusCodeId
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F19: Error al obtener códigos de estado desde la BD.");
                await _pgConfiguration.InsertLogAsync(nameof(GetStatusCodeIdByValidatorCodeAsync), "Error al intentar obtener el codigo ", ex);
                return null;
            }
        }

        private async Task<SyncroSaveStatus> AprobarTransaccionAsync(TransactionOriginal transaction)
        {
            DateTime now = DateTime.Now;
            var retorno = new SyncroSaveStatus();
            try
            {
                await using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
                {
                    await SetStatusAsync(transaction.TransactionIdPK, 5);

                    var transactionAdditionalInfo = await _context.TransactionAdditionalInfos
                                                                 .FirstOrDefaultAsync(c => c.TransactionIdPk == transaction.TransactionIdPK);
                    if (transactionAdditionalInfo != null)
                    {
                        transactionAdditionalInfo.AuthorizationCode = transaction.AuthorizationCode;
                        transactionAdditionalInfo.CardMask = transaction.CardMask;
                        transactionAdditionalInfo.CardHolder = transaction.CardHolder;
                        transactionAdditionalInfo.TicketNumber = transaction.TicketNumber;
                        transactionAdditionalInfo.BatchNbr = transaction.NroLote;
                        transactionAdditionalInfo.SynchronizationDate = now;
                    }

                    AppConfig.stats.TotalTransactionsClosedOK++;
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();

                    retorno.HasError = false;
                    retorno.ErrorMessage = null;
                    retorno.StatusSaved = true;
                    retorno.ItWasClose = true;
                    retorno.StatusCodeId = 5;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F20: Error al aprobar transacción.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }

        private async Task<SyncroSaveStatus> CerrarTransaccionPorRechazoAsync(long transactionIDPK, int StatusCodeId)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                await SetStatusAsync(transactionIDPK, StatusCodeId);
                retorno.StatusCodeId = StatusCodeId;
                AppConfig.stats.TotalTransactionsClosedNO++;
                retorno.HasError = false;
                retorno.ErrorMessage = null;
                retorno.StatusSaved = true;
                retorno.ItWasClose = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F21: Error al cerrar por rechazo.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }

        private async Task<SyncroSaveStatus> CerrarTransaccionPorErrorGeneradoAsync(long transactionIDPK, int StatusCodeId)
        {
            var retorno = new SyncroSaveStatus();
            try
            {
                await SetStatusAsync(transactionIDPK, StatusCodeId);
                AppConfig.stats.TotalTransactionsClosedERROR++;
                retorno.HasError = true;
                retorno.ErrorMessage = "La transacción fue cerrada, pero porque existe un error en la misma. Debe revisarse manually";
                retorno.StatusSaved = true;
                retorno.ItWasClose = true;
                retorno.StatusCodeId = StatusCodeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F22: Error al cerrar por error generado.");
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }
    }
}