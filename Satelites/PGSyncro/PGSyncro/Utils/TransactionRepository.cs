using PGSyncro.EFData;
using PGSyncro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Utils
{
   public static class TransactionRepository
    {

        public static List<GetTransactionsToSync_Result> GetTransactionsToSync() {
            using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
            {
                datacontext.Database.CommandTimeout = 600;
                return datacontext.Database.SqlQuery<GetTransactionsToSync_Result>("GetTransactionsToSync").ToList();
            }

        }
        public static SyncroSaveStatus CerrarPorInexistenteEnValidador(long transactionIDPK)
        {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            Program.stats.TotalTransactionsClosedNAByTIMEOUT++;
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == transactionIDPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = 52,
                            TransactionsId = transactionIDPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });
                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        retorno.HasError = false;
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
            }

            return retorno;

        }

        public static SyncroSaveStatus ProcessResult(GetTransactionsToSync_Result transactionToWork, TransactionOriginal transactionData)
        {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try
            {
                Status statusCode = GetStatusCodeIdByValidatorCode(transactionToWork.ValidatorId, transactionData.OriginalCode, transactionData.ModuleType);
                if (statusCode == null)
                {
                    //El codigo especificado no existe!
                    retorno.HasError = true;
                    retorno.ErrorMessage = string.Format("El codigo especificado para Validador {0}, Codigo {1}, Modulo {2} no existe.", transactionToWork.ValidatorId, transactionData.OriginalCode, transactionData.ModuleType);
                    retorno.StatusSaved = false;
                    retorno.ItWasClose = false;
                }
                else
                {
                    switch (statusCode.GenericCodeId)
                    {
                        case 1:
                            //Transaccion aprobada
                            retorno = AprobarTransaccion(transactionData); 
                            break;
                        case 2:
                            //Transaccion rechazada
                            retorno = CerrarTransaccionPorRechazo(transactionToWork.TransactionIdPK, statusCode.StatusCodeId);
                            break;
                        case 3:
                            //Transaccion con error
                            retorno = CerrarTransaccionPorErrorGenerado(transactionToWork.TransactionIdPK, statusCode.StatusCodeId);
                            break;
                        case 4:
                            //Transaccion sigue en curso
                            retorno = ProcesarEnCurso(transactionToWork);
                            break;
                        default:
                            //Error en codigo generico.
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }

            return retorno;


        }



        private static SyncroSaveStatus CerrarPorTimeOut(long TransactionIdPK) {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == TransactionIdPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = 51,
                            TransactionsId = TransactionIdPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });
                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        Program.stats.TotalTransactionsClosedNAByTIMEOUT++;
                        retorno.HasError = false;
                        retorno.ErrorMessage = null;
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = true;

                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }

            return retorno;

            
        }

        private static SyncroSaveStatus ProcesarEnCurso(GetTransactionsToSync_Result transaccion)
        {
            int minutes = 3600;
            switch (transaccion.ValidatorId) {
                case 1:
                    //NPS Timeout
                    minutes = Program.NPSTimeOutInMinutes;
                    break;
                case 3:
                    //SPS Timeout
                    minutes = Program.SPSTimeOutInMinutes;
                    break;
                default:
                    break;
            }

            if (DateTime.Now > transaccion.CreatedOn.AddMinutes(minutes))
            {
                //Pasado el tiempo de timeout
            return    CerrarPorTimeOut(transaccion.TransactionIdPK);
            }
            else {
            return    ActualizarEstadoAEnCurso(transaccion.TransactionIdPK);
            }



        }

        private static SyncroSaveStatus ActualizarEstadoAEnCurso(long TransactionIdPK)
        {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == TransactionIdPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = 1,
                            TransactionsId = TransactionIdPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });
                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        Program.stats.TotalTransactionsClosedNA++;
                        retorno.HasError = false;
                        retorno.ErrorMessage = null;
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = false;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }

            return retorno;
        

            
        }

        public static Status GetStatusCodeIdByValidatorCode(int validatorId, string originalcode, string type) {



            try {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                  return  (
                                from sc in datacontext.StatusCodes
                                join cm in datacontext.CodeMappings
                                on sc.StatusCodeId equals cm.StatusCodeId
                                join modcode in datacontext.ModuleCodes
                                on cm.ModuleCodeId equals modcode.ModuleCodeId
                                join mod in datacontext.Modules
                                on modcode.ModuleId equals mod.Id
                                where mod.Type == type && mod.validatorId == validatorId && modcode.OriginalCode == originalcode
                                select new Status { 
                                    GenericCodeId = sc.GenericCodeId,
                                    StatusCodeId = sc.StatusCodeId 
                                }
                        ).FirstOrDefault();
                }
            } catch (Exception ex) {
                PGConfiguration.InsertLog("GetStatusCodeIdByValidatorCode", "Error al intentar obtener el codigo ", ex);

                return null;
            }

     


        }

        private static SyncroSaveStatus AprobarTransaccion(TransactionOriginal transaction) {

            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == transaction.TransactionIdPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = 5,
                            TransactionsId = transaction.TransactionIdPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });

                        List<TransactionResultInfo> transactionResultInfo = datacontext.TransactionResultInfoes.Where(tri => tri.TransactionIdPK == transaction.TransactionIdPK).ToList();
                        if (!transactionResultInfo.Any())
                        {
                            datacontext.TransactionResultInfoes.Add(new TransactionResultInfo
                            {
                                TransactionIdPK = transaction.TransactionIdPK,
                                ResponseCode = "PG_SINCRO_APROV",
                                StateMessage = "APROBADA - Autorizada por el sincronizador de PG",
                                StateExtendedMessage = "Proceso de sincronizacion: " + Program.SincroId,
                                Amount = transaction.Amount,
                                Payments = transaction.Payments,
                                Currency = null,
                                Country = null,
                                AuthorizationCode = transaction.AuthorizationCode,
                                CardMask = transaction.CardMask,
                                CardNbrLfd = transaction.Card4LastDigits,
                                CardHolder = transaction.CardHolder,
                                CustomerDocNumber = null,
                                CustomerDocType = null,
                                CustomerEmail = transaction.Mail,
                                TicketNumber = transaction.TicketNumber,
                                CreatedBy = Program.appVersion,
                                CreatedOn = DateTime.Now,
                                BatchNbr = transaction.NroLote,
                                SynchronizationDate = DateTime.Now,
                                BatchSPSDate = null,
                                MailSynchronizationDate = null
                            });
                        }
                        else
                        {
                            foreach (TransactionResultInfo tri in transactionResultInfo)
                            {
                                tri.ResponseCode = "PG_SINCRO_APROV";
                                tri.StateMessage = "APROBADA - Autorizada por el sincronizador de PG";
                                tri.StateExtendedMessage = "Proceso de sincronizacion: " + Program.SincroId;
                                tri.Amount = transaction.Amount;
                                tri.Payments = transaction.Payments;
                                tri.AuthorizationCode = transaction.AuthorizationCode;
                                if (tri.CardMask.Length > transaction.CardMask.Length) {
                                    tri.CardMask = transaction.CardMask;
                                } 
                                tri.CardNbrLfd = transaction.Card4LastDigits;
                                tri.CardHolder = transaction.CardHolder;
                                tri.CustomerEmail = transaction.Mail;
                                tri.TicketNumber = transaction.TicketNumber;
                                tri.CreatedBy = Program.appVersion;
                                if (tri.BatchSPSDate == null && string.IsNullOrEmpty(tri.BatchNbr)) {
                                    //Lo inserto si no me vino en el spsbatch y es nulo
                                    tri.BatchNbr = transaction.NroLote;
                                }
                                tri.SynchronizationDate = DateTime.Now;
                            }
                        }
                        Program.stats.TotalTransactionsClosedOK++;
                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        retorno.HasError = false;
                        retorno.ErrorMessage = null;
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }

            return retorno;


            
        }

        private static SyncroSaveStatus CerrarTransaccionPorRechazo(long transactionIDPK, int StatusCodeId) {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == transactionIDPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = StatusCodeId,
                            TransactionsId = transactionIDPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });

                        

                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        Program.stats.TotalTransactionsClosedNO++;
                        retorno.HasError = false;
                        retorno.ErrorMessage = null;
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }

        private static SyncroSaveStatus CerrarTransaccionPorErrorGenerado(long transactionIDPK, int StatusCodeId)
        {
            SyncroSaveStatus retorno = new SyncroSaveStatus();
            try
            {
                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                    using (var dbContextTransaction = datacontext.Database.BeginTransaction())
                    {
                        List<TransactionStatu> resultados = datacontext.TransactionStatus.Where(ts => ts.TransactionsId == transactionIDPK).ToList();
                        for (int i = 0; i < resultados.Count; i++)
                        {
                            resultados[i].IsActual = false;
                        }
                        datacontext.TransactionStatus.Add(new TransactionStatu()
                        {
                            IsActive = true,
                            IsActual = true,
                            StatusCodeId = StatusCodeId,
                            TransactionsId = transactionIDPK,
                            CreatedBy = Program.appVersion,
                            CreatedOn = DateTime.Now
                        });



                        datacontext.SaveChanges();
                        dbContextTransaction.Commit();
                        Program.stats.TotalTransactionsClosedERROR++;
                        retorno.HasError = true;
                        retorno.ErrorMessage = "La transacción fue cerrada, pero porque existe un error en la misma. Debe revisarse manualmente";
                        retorno.StatusSaved = true;
                        retorno.ItWasClose = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.ErrorMessage = ex.Message;
                retorno.StatusSaved = false;
                retorno.ItWasClose = false;
            }
            return retorno;
        }





    }
}
