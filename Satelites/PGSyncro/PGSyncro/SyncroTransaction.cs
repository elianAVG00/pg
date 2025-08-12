using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using PGSyncro.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace PGSyncro
{
    public static class SyncroTransaction
    {
        public static SyncroProcess ProcesarTransaccion(GetTransactionsToSync_Result transactionToWork, string hash = "", bool validadorNuevoDeDecidir = false) {
            SyncroProcess retorno = new SyncroProcess();
            Program.stats.TotalTransactionsProcessed++;
            TransactionOriginal respuestaDelValidador = GetRespuestaDelValidador(transactionToWork, hash ,  validadorNuevoDeDecidir );
            retorno.Communication = respuestaDelValidador.QueryResponse;
            retorno.Status = SaveStatus(transactionToWork, respuestaDelValidador);
            return retorno;


        }

        public static TransactionOriginal GetRespuestaDelValidador(GetTransactionsToSync_Result transactionToWork, string hash = "", bool validadorNuevoDeDecidir = false) {

            TransactionOriginal respuestaDelValidador = new TransactionOriginal();
            //Proceso normal
            if (transactionToWork.ValidatorId == 1) //NPS
            {

                //Verifico si la transaction tiene transactionId (caso NPS)
                if (string.IsNullOrWhiteSpace(transactionToWork.TransactionId))
                {
                    //   CloseByTimeOut(transactionToWork.TransactionIdPK);

                    respuestaDelValidador.QueryResponse = new SyncroModel
                    {
                        HasError = false,
                        HasResponse = true,
                        HasTransaction = false
                    };
                }
                else
                {
                    //Transaction a sincronizar comun que llego a NPS
                    respuestaDelValidador = NPSCall.GetNPSTransaction(transactionToWork, hash);
                }
            }
            else
            {
                //Es de DECIDIR
                if (validadorNuevoDeDecidir)
                {
                    //Es del nuevo validador
                    respuestaDelValidador = SPSCalls.GetAPIService(transactionToWork, hash);
                }
                else
                {
                    //Es del viejo DECIDIR
                    respuestaDelValidador = SPSCalls.GetXMLService(transactionToWork);
                }
            }
            return respuestaDelValidador;
        }

        public static bool MonitorCloseProcess() {
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    MonitorSyncroProcess monitor = datacontext.MonitorSyncroProcesses.Where(m => m.MonitorSyncroProcessId == Program.SincroId).FirstOrDefault();
                    monitor.EndOn = DateTime.Now;
                    monitor.TotalTransactionsClosed = Program.stats.TotalTransactionsClosed;
                    monitor.TotalTransactionsClosedERROR = Program.stats.TotalTransactionsClosedERROR;
                    monitor.TotalTransactionsClosedNA = Program.stats.TotalTransactionsClosedNA;
                    monitor.TotalTransactionsClosedNAByTIMEOUT = Program.stats.TotalTransactionsClosedNAByTIMEOUT;
                    monitor.TotalTransactionsClosedNO = Program.stats.TotalTransactionsClosedNO;
                    monitor.TotalTransactionsClosedOK = Program.stats.TotalTransactionsClosedOK;
                    monitor.TotalTransactionsNotClosed = Program.stats.TotalTransactionsNotClosed;
                    monitor.TotalTransactionsProcessed = Program.stats.TotalTransactionsProcessed;
                    monitor.TotalTransactionsWithError = Program.stats.TotalTransactionsWithError;
                    monitor.UpdatedBy = Program.appVersion;
                    monitor.UpdatedOn = DateTime.Now;
                    monitor.WithProcessError = Program.witherror;
                    datacontext.SaveChanges();
                    return true;
                }
                }
            catch (Exception ex) {
                PGConfiguration.InsertLog("MonitorCloseProcess", "No se pudo cerrar el proceso MonitorSyncroProcessId " + Program.SincroId, ex);

                return false;
            }
        }
        public static bool MonitorCloseTransaction(long MonitorSyncroProcessRecordsId, SyncroProcess syncro)
        {
            try
            {
                if (syncro.Status.ItWasClose)
                {
                    Program.stats.TotalTransactionsClosed++;
                }
                else {
                    Program.stats.TotalTransactionsNotClosed++;
                }
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    string error = GetErrors(syncro);
                    MonitorSyncroProcessRecord monitorTrans = datacontext.MonitorSyncroProcessRecords.Where(mtr => mtr.MonitorSyncroProcessRecordsId == MonitorSyncroProcessRecordsId).FirstOrDefault();
                    if (syncro.Status?.ItWasClose == true) {
                        monitorTrans.ClosedByProcess = DateTime.Now;
                    }

                    monitorTrans.Error = error;
                    monitorTrans.EndProcess = DateTime.Now;
                    monitorTrans.HasError = (error != null);
                    monitorTrans.UpdatedBy = Program.appVersion;
                    monitorTrans.UpdatedOn = DateTime.Now;
                    datacontext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                PGConfiguration.InsertLog("MonitorCloseTransaction", "No se pudo cerrar la transacción MonitorSyncroProcessRecordsId " + MonitorSyncroProcessRecordsId, ex);

            }
            return false;
        }

        public static bool MonitorBeginTransaction(long MonitorSyncroProcessRecordsId) {
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    MonitorSyncroProcessRecord monitorTrans = datacontext.MonitorSyncroProcessRecords.Where(mtr => mtr.MonitorSyncroProcessRecordsId == MonitorSyncroProcessRecordsId).FirstOrDefault();
                    monitorTrans.BeginProcess = DateTime.Now;
                    monitorTrans.UpdatedBy = Program.appVersion;
                    monitorTrans.UpdatedOn = DateTime.Now;
                    datacontext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                PGConfiguration.InsertLog("MonitorBeginTransaction", "No se pudo abrir la transacción MonitorSyncroProcessRecordsId " + MonitorSyncroProcessRecordsId, ex);

            }
            return false;
        }

        private static SyncroSaveStatus SaveStatus(GetTransactionsToSync_Result transactionToWork, TransactionOriginal transactionOriginal) {


            if (transactionOriginal.QueryResponse.HasError)
            {
                //Hay Error en la comunicacion
                return new SyncroSaveStatus
                {
                    ErrorMessage = "Hubo error en la comunicación",
                    HasError = true,
                    ItWasClose = false,
                    StatusSaved = false
                };
            }
            else
            {
                if (!transactionOriginal.QueryResponse.HasResponse)
                {
                    //No tiene respuesta

                    return new SyncroSaveStatus
                    {
                        ErrorMessage = "La respuesta se encuentra vacía en la comunicación",
                        HasError = true,
                        ItWasClose = false,
                        StatusSaved = false
                    };
                }
                else
                {
                    //Tiene respuesta
                    if (transactionOriginal.QueryResponse.HasTransaction)
                    {
                        //Tiene transaccion
                        return TransactionRepository.ProcessResult(transactionToWork, transactionOriginal);
                    }
                    else
                    {
                        //No tiene transaccion
                       return TransactionRepository.CerrarPorInexistenteEnValidador(transactionToWork.TransactionIdPK);
                    }
                }
            }
        }
        private static string GetErrors(SyncroProcess syncro) {
                if (syncro.Communication?.HasError == true) {
                Program.stats.TotalTransactionsWithError++;
                return JsonConvert.SerializeObject(syncro);
                }
                if (syncro.Status?.HasError == true) {
                Program.stats.TotalTransactionsWithError++;
                return JsonConvert.SerializeObject(syncro);
                }
            return null;
        }
    }
}
