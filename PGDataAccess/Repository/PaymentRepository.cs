using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Models;
using PGDataAccess.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PGDataAccess.Repository
{
    public static class PaymentRepository
    {
        public static void UpdateTransactionSyncByJob(long transactionNumber)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    TransactionAdditionalInfo taiToUpdate = (from tai in context.TransactionAdditionalInfo
                                                             where tai.TransactionNumber == transactionNumber
                                                             select tai).FirstOrDefault();
                    if (taiToUpdate != null)
                    {
                        taiToUpdate.WasSyncByJob = true;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());

            }
        }

        public static void UpdateTransactionCloseByTimeOut(long transactionIdPK)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    TransactionAdditionalInfo taiToUpdate = (from tai in context.TransactionAdditionalInfo
                                                             where tai.TransactionIdPK == transactionIdPK
                                                             select tai).FirstOrDefault();
                    if (taiToUpdate != null)
                    {
                        taiToUpdate.WasTimeOutForced = true;
                    }

                    StatusRepository.UpdateTransactionStatus(transactionIdPK, "PGPAYMENTFORCEDTIMEOUT", "payment", null);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        //Get GenericCode = 4 OR Status Code = "214"
        public static List<TransactionValidatorNumber> GetNATransactions(DateTime desde, DateTime hasta)
        {

            List<TransactionValidatorNumber> transactions = new List<TransactionValidatorNumber>();

            try
            {
                using (var context = new PGDataEntities())
                {
                    transactions = (
                             from trans in context.Transactions
                             join ts in context.TransactionStatus
                                on trans.Id equals ts.TransactionsId
                             join sc in context.StatusCode
                                on ts.StatusCodeId equals sc.StatusCodeId
                             join tai in context.TransactionAdditionalInfo
                                on trans.Id equals tai.TransactionIdPK
                             join val in context.Validators
                                on tai.ValidatorId equals val.ValidatorId
                             where (sc.GenericCodeId == 4)// GenericCode = 4 es NA, Added 214 code
                             && (DbFunctions.TruncateTime(trans.CreatedOn) >= desde
                             && DbFunctions.TruncateTime(trans.CreatedOn) <= hasta)
                             && ts.IsActual
                             select new TransactionValidatorNumber
                             {
                                 TransactionIdPK = trans.Id,
                                 ValidatorTransactionId = trans.TransactionId,
                                 TransactionNumber = tai.TransactionNumber,
                                 Validator = val.Name
                             }).ToList();

                    return transactions;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                return null;
            }
        }

        public static CheckTransaction CheckIfTransactionIsComplete(string transactionId)
        {
            //0 - No existe
            //1 - Incompleta
            //2 - Completa
            long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
            var checkdataToReturn = new CheckTransaction() { CheckResult = 0, ValidatorShortName = null };
            try
            {
                using (var context = new PGDataEntities())
                {
                    string genericCode = (from trans in context.Transactions
                                          join tai in context.TransactionAdditionalInfo
                                              on trans.Id equals tai.TransactionIdPK
                                          join ts in context.TransactionStatus
                                              on trans.Id equals ts.TransactionsId
                                          join sc in context.StatusCode
                                              on ts.StatusCodeId equals sc.StatusCodeId
                                          join gc in context.GenericCode
                                              on sc.GenericCodeId equals gc.Id
                                          where tai.TransactionNumber == transactionNumber && ts.IsActual
                                          select gc.Type).FirstOrDefault();

                    if (genericCode != null)
                    {
                        string validator = (from trans in context.Transactions
                                            join tai in context.TransactionAdditionalInfo
                                                on trans.Id equals tai.TransactionIdPK
                                            join val in context.Validators
                                                on tai.ValidatorId equals val.ValidatorId
                                            where tai.TransactionNumber == transactionNumber
                                            select val.Name).FirstOrDefault();
                        checkdataToReturn.ValidatorShortName = validator;
                        checkdataToReturn.CheckResult = (genericCode == "NA") ? 1 : 2;
                    }

                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                checkdataToReturn.CheckResult = 0;
            }
            return checkdataToReturn;

        }

        /// <summary>
        /// Actualiza el estado del commerce item
        /// </summary>
        /// <param name="id">0 - Normal, 1 - Con reclamo abierto, 2 - Anulado</param>
        /// <param name="state"></param>
        public static void UpdateCommerceItemState(long id, int state)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    CommerceItems commerceItem = (from ci in context.CommerceItems
                                                  where ci.CommerceItemsId == id
                                                  select ci).FirstOrDefault();

                    if (commerceItem != null)
                    {
                        commerceItem.State = state;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }

        }

        public static CommerceItemModel GetCommerceItemById(long id)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    CommerceItems commerceItem = (from ci in context.CommerceItems
                                                  where ci.CommerceItemsId == id
                                                  select ci).FirstOrDefault();

                    if (commerceItem != null)
                    {
                        //TODO Migrate To Map
                        return new CommerceItemModel
                        {
                            Code = commerceItem.Code,
                            CreatedBy = commerceItem.CreatedBy,
                            CreatedOn = commerceItem.CreatedOn,
                            Description = commerceItem.Description,
                            CommerceItemId = commerceItem.CommerceItemsId,
                            IsActive = commerceItem.IsActive,
                            OriginalCode = commerceItem.OriginalCode,
                            Amount = commerceItem.Amount,
                            State = commerceItem.State,
                            TransactionId = commerceItem.TransactionIdPK,
                        };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }

        }

        public static bool UpdateCommerceItemCode(string oldCode, string newCode, string transactionId)
        {
            try
            {
                long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new PGDataEntities())
                {
                    CommerceItems ciToUpdate = (from ci in context.CommerceItems
                                                  join t in context.Transactions
                                                    on ci.TransactionIdPK equals t.Id
                                                  join tai in context.TransactionAdditionalInfo
                                                    on t.Id equals tai.TransactionIdPK
                                                  where ci.IsActive &&
                                                    ci.Code == oldCode &&
                                                    tai.TransactionNumber == transactionNumber
                                                  select ci).FirstOrDefault();

                    if (ciToUpdate != null)
                    {
                        ciToUpdate.OriginalCode = oldCode;
                        ciToUpdate.Code = newCode;
                        ciToUpdate.UpdatedBy = "clientMethod";
                        ciToUpdate.UpdatedOn = DateTime.Now;
                        context.SaveChanges();

                        Logs log = new Logs
                        {
                            Date = DateTime.Now,
                            Type = "Log",
                            Thread = "Update Commerce Item",
                            Message = "OldCommerceItemCode:" + oldCode + " NewCommerceItemCode:" + newCode,
                            Exception = "",
                            createdBy = "system",
                            createdOn = DateTime.Now
                        };
                        context.Logs.Add(log);
                        context.SaveChanges();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
            return false;
        }

        public static int SaveTransaction(TransactionModel transaction, IEnumerable<CommerceItemModel> commerceItems)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = $"transaction={transaction},commerceItems.Count={commerceItems.Count()}";
                    LogRepository.InsertLogCommon(LogTypeModel.Info, $"Parametros: {inputParameters}");
                    Transactions transactionEntity = Mapper.Transaction_ModelToEF(transaction);
                    if (commerceItems != null)
                    {
                        foreach (CommerceItemModel mci in commerceItems)
                        {
                            var commerceItem = new CommerceItems
                            {
                                Transactions = transactionEntity,
                                Code = mci.Code,
                                Description = mci.Description,
                                Amount = mci.Amount,
                                CreatedBy = "system",
                                CreatedOn = DateTime.Now
                            };
                            context.CommerceItems.Add(commerceItem);
                        }
                    }

                    context.Transactions.Add(transactionEntity);
                    context.SaveChanges();

                    TransactionAdditionalInfo newInfo = new TransactionAdditionalInfo();
                    newInfo.ProdVersionUsed = AppConfigRepository.GetAppVersion("PGMS");
                    newInfo.CurrentAmount = transaction.CurrentAmount;
                    newInfo.TransactionIdPK = transactionEntity.Id;
                    newInfo.MerchantId = transaction.MerchantId;
                    newInfo.CallbackUrl = transaction.CallbackUrl;
                    newInfo.Payments = transaction.Payments.HasValue ? transaction.Payments.Value : 1;
                    newInfo.LanguageId = transaction.LanguageId;
                    newInfo.BarCode = transaction.BarCode;
                    newInfo.IsEPCValidated = transaction.IsEPCValidated;    
                    newInfo.EPCValidateURL = transaction.EPCValidateURL;
                    newInfo.VersionUsed = transaction.AppVersion;
                    newInfo.CreatedBy = transaction.CreatedBy;
                    newInfo.CreatedOn = DateTime.Now;
                    newInfo.CustomerMail = transaction.CustomerMail;
                    newInfo.IsActive = true;
                    newInfo.ChannelId = transaction.ChannelId;
                    newInfo.ProductId = transaction.ProductId;
                    newInfo.ServiceId = transaction.ServiceId;
                    newInfo.ClientId = transaction.ClientId;
                    newInfo.ValidatorId = transaction.ValidatorId;
                    newInfo.ExternalApp = transaction.ExternalAppId;
                    newInfo.CurrencyId = transaction.CurrencyId;
                    //ADDED 4.1 - Additional Info
                    newInfo.VersionUsed = newInfo.ProdVersionUsed;
                    newInfo.UniqueCode = transaction.UniqueCode;
                    newInfo.IsCommerceItemValidated = transaction.IsCommerceItemValidated;
                    newInfo.IsSimulation = transaction.IsSimulation;
                    newInfo.TransactionNumber = transactionEntity.Id;

                    context.TransactionAdditionalInfo.Add(newInfo);
                    context.SaveChanges();
                    return (int)transactionEntity.Id;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogCommon(LogTypeModel.Error, "Entity of type " + eve.Entry.Entity.GetType().Name 
                            + " in state " + eve.Entry.State + " has the following validation errors:", "- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage).ToString());
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Función para obtener la cadena de inner exceptions
                string GetInnerExceptions(Exception exception)
                {
                    var messages = new StringBuilder();
                    while (exception != null)
                    {
                        messages.AppendLine(exception.Message);
                        exception = exception.InnerException;
                    }
                    return messages.ToString();
                }

                var fullError = ex.ToString(); // Incluye la traza completa
                var innerErrors = GetInnerExceptions(ex);
                LogRepository.InsertLogCommon(LogTypeModel.Error, "DbUpdateException completa: " + fullError);
                LogRepository.InsertLogCommon(LogTypeModel.Error, "Inner Exceptions: " + innerErrors);
                throw new Exception("Error al actualizar la base de datos: " + innerErrors, ex);
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static bool UpdateTransactionValidatorData(long transactionIdPK, string validatorTransactionId, string internalNumber)
        {
            LogRepository.InsertLogCommon(LogTypeModel.Debug, "transactionIdPK: " + transactionIdPK + " - validatorTransactionId: " + validatorTransactionId + " - internalNumber: " + internalNumber);

            using (var context = new PGDataEntities())
            {
                try
                {
                    var transaction = context.Transactions.FirstOrDefault(t => t.Id == transactionIdPK);
                    if (transaction != null)
                    {
                        transaction.TransactionId = validatorTransactionId;
                        transaction.InternalNbr = internalNumber;
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogCommon(LogTypeModel.Error, "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:", "- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage).ToString());
                        }
                    }
                    throw;
                }
            }
        }

        public static void UpdateTransactionId(long transactionIdPK, string transactionId)
        {
            long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
            using (var context = new PGDataEntities())
            {
                var transactionAditionalInfo = context.Transactions.FirstOrDefault(t => t.Id == transactionIdPK).TransactionAdditionalInfo.First();
                transactionAditionalInfo.TransactionNumber = transactionNumber;
                context.SaveChanges();
            }
        }

        public static string GetTransactionNumberAsStringByTransactionId(string transactionId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    
                    var transactionEntity = (from trans in context.Transactions
                                             join tai in context.TransactionAdditionalInfo
                                                on trans.Id equals tai.TransactionIdPK
                                             where trans.TransactionId == transactionId
                                             select tai.TransactionNumber.ToString()).FirstOrDefault();
                    return transactionEntity;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static Models.TransactionModel GetTransactionByTransactionId(string transactionId)
        {
            try
            {
                long transactionNumber = Tools.CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new EF.PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var transactionEntity = context.TransactionAdditionalInfo.FirstOrDefault(tai => tai.TransactionNumber == transactionNumber).Transactions;
                    if (transactionEntity != null)
                        return Mapper.Transaction_EFToModel(transactionEntity);
                    else
                        return null;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        public static TransactionModel GetTransactionById(long? idTransaction)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionId=", idTransaction);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    var transactionEntity = context.Transactions.FirstOrDefault(t => t.Id == idTransaction);
                    return Mapper.Transaction_EFToModel(transactionEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static long? GetTransactionPKByTransactionId(string transactionId)
        {
            long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);

            using (var context = new PGDataEntities())
            {
                Transactions transaction = (
                    from trans in context.Transactions
                    join tai in context.TransactionAdditionalInfo
                    on trans.Id equals tai.TransactionIdPK
                    where tai.TransactionNumber == transactionNumber
                    select trans).FirstOrDefault();
                    
                return transaction?.Id;
            }
        }

        public static bool isDuplicatedElectronicPaymentCode(string electronicPaymentCode, int serviceId)
        {
            bool returnedValue = false;
            try
            {
                using (var context = new PGDataEntities())
                {
                    var epc = new SqlParameter
                    {
                        ParameterName = "EPC",
                        Value = electronicPaymentCode
                    };

                    var sId = new SqlParameter
                    {
                        ParameterName = "ServiceId",
                        Value = serviceId
                    };

                    var ifexists = new SqlParameter();
                    ifexists.ParameterName = "Exists";
                    ifexists.SqlDbType = SqlDbType.Bit;
                    ifexists.Direction = ParameterDirection.Output;

                    context.Database.ExecuteSqlCommand("[dbo].[IsEPCExists] @EPC,@ServiceId,@Exists OUTPUT", epc, sId, ifexists);
                    returnedValue = Convert.ToBoolean(ifexists.Value);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno: ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
            return returnedValue;

        }

        public static List<Models.TransactionModel> GetTransactionsByElectronicPaymentCodeAndMerchantId(string electronicPaymentCode, string merchantId)
        {
            var transactions = new List<TransactionModel>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, $"Parametros: electronicPaymentCode={electronicPaymentCode},merchantId={merchantId}");

                    List<Transactions> listTransactionEntity = (from t in context.Transactions
                                                                join tai in context.TransactionAdditionalInfo
                                                                    on t.Id equals tai.TransactionIdPK
                                                                where t.ElectronicPaymentCode == electronicPaymentCode && tai.MerchantId == merchantId
                                                                select t).ToList();

                    if (listTransactionEntity.Count > 0)
                    {
                        foreach (var transactionEntity in listTransactionEntity)
                        {
                            transactions.Add(Mapper.Transaction_EFToModel(transactionEntity));
                        }
                    }
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
            return transactions;
        }

        public static void UpdateTransaction(TransactionModel transaction)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Info, $"Parametros: transaction={transaction}");

                    var originalTransaction = context.Transactions.FirstOrDefault(t => t.Id == transaction.Id);
                    if (originalTransaction != null)
                        originalTransaction = Mapper.Transaction_ModelToEF(originalTransaction, transaction);
                        context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());

            }
        }

        public static bool IsTransactionInTRIByTransactionIdPK(long transactionIdPK)
        {
            using (var context = new EF.PGDataEntities())
            {
                var transactionInTRI = context.TransactionResultInfo.FirstOrDefault(ttri => ttri.TransactionIdPK == transactionIdPK);

                if (transactionInTRI == null)
                {
                    //No existe
                    return false;
                }
                else
                {
                    //Existe
                    return true;
                }
            }
        }

        public static bool IsTransactionInTRIByTransactionId(string transactionId)
        {
            using (var context = new PGDataEntities())
            {
                long? transactionIdPK = GetTransactionPKByTransactionId(transactionId);
                if (transactionIdPK != null)
                {
                    var transactionInTRI = context.TransactionResultInfo.FirstOrDefault(ttri => ttri.Transactions.Id == transactionIdPK);

                    if (transactionInTRI == null)
                    {
                        //No existe
                        return false;
                    }
                    else
                    {
                        //Existe
                        return true;
                    }
                }
                return false;
            }
        }

        public static long InsertPaymentValidatorComm(PaymentValidatorCommModel paymentValidatorComm)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    PaymentValidatorComm paymentValidatorCommModel = Mapper.PaymentValidatorComm_ModelToEF(paymentValidatorComm);
                    context.PaymentValidatorComm.Add(paymentValidatorCommModel);
                    context.SaveChanges();
                    return paymentValidatorCommModel.PaymentValidatorCommId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdatePaymentValidatorComm(PaymentValidatorCommModel paymentValidatorComm)
        {
            using (var context = new PGDataEntities())
            {
                var originalRecord = context.PaymentValidatorComm.FirstOrDefault(pvc => pvc.PaymentValidatorCommId == paymentValidatorComm.PaymentValidatorCommId);

                originalRecord.ResponseDate = DateTime.Now;
                originalRecord.ResponseMessage = paymentValidatorComm.ResponseMessage;

                context.SaveChanges();
            }
        }

        public static void UpdatePaymentValidatorCommByIdTransaction(Models.PaymentValidatorCommModel paymentValidatorComm)
        {
            using (var context = new PGDataEntities())
            {
                var originalRecord = context.PaymentValidatorComm.FirstOrDefault(pvc => pvc.TransactionIdPK == paymentValidatorComm.TransactionId);

                originalRecord.ResponseDate = DateTime.Now;
                originalRecord.ResponseMessage = paymentValidatorComm.ResponseMessage;
                context.SaveChanges();
            }
        }

        public static int InsertTransactionResultInfo(TransactionResultInfoModel transactionResultInfo)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var transactionResultInfoEntity = Mapper.TransactionResultInfo_ModelToEF(transactionResultInfo);
                    var transaction = context.Transactions.FirstOrDefault(t => t.Id == transactionResultInfo.TransactionIdPK);

                    //Actualizar TAI
                    if (transaction != null) // Check if transaction exists
                    {
                        var transTAI = context.TransactionAdditionalInfo.FirstOrDefault(t => t.TransactionIdPK == transaction.Id);
                        transTAI.TicketNumber = transactionResultInfo.TicketNumber;
                        transTAI.CardHolder = transactionResultInfo.CardHolder;
                        transTAI.CardMask = transactionResultInfo.CardMask;
                        transTAI.AuthorizationCode = transactionResultInfo.AuthorizationCode;
                        transTAI.BatchNbr = transactionResultInfo.BatchNbr;
                    }

                    transactionResultInfoEntity.CreatedOn = transaction.CreatedOn;
                    context.TransactionResultInfo.Add(transactionResultInfoEntity);
                    context.SaveChanges();
                    return transactionResultInfoEntity.TransactionResultInfoId;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogCommon(LogTypeModel.Error, "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:", "- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage).ToString());
                    }
                }
                throw;
            }
        }

        public static TransactionCompletedInfo GetTransactionResult(long transactionIdPK, string language)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ProxyCreationEnabled = false;

                    context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                    var transaction = context.Transactions
                        .Where(t => t.Id == transactionIdPK)
                        .Include("CommerceItems")
                        .Include("TransactionResultInfo")
                        .Include("TransactionAdditionalInfo.Clients")
                        .Include("TransactionAdditionalInfo.Services")
                        .Include("TransactionAdditionalInfo.Products")
                        .Include("TransactionStatus.StatusCode.GenericCode")
                        .Include("TransactionStatus.StatusCode.StatusMessage.Language")
                        .Include("PaymentClaim.Currency")
                        .Include("PaymentClaim.PaymentClaimStatus.StatusCode.GenericCode")
                        .Include("PaymentClaim.PaymentClaimStatus.StatusCode.StatusMessage.Language")
                        .Include("PaymentClaim.PaymentClaimStatus.PaymentClaim.Claimer.DocType")
                        .FirstOrDefault();

                    if (transaction == null)
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Info, "No entra la transaction");
                    }

                    return BuildTransactionCompletedInfo(language, transaction);
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Error interno al generar resultado: " + ex.Message, ex);
            }
        }

        private static TransactionCompletedInfo BuildTransactionCompletedInfo(string language, Transactions transaction)
        {
            if (transaction == null) return null;

            var trasactionCompletedInfo = new TransactionCompletedInfo
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Channel = transaction.Channel,
                ConvertionRate = transaction.ConvertionRate,
                CurrencyCode = transaction.CurrencyCode,
                ElectronicPaymentCode = transaction.ElectronicPaymentCode,
                InternalNbr = transaction.InternalNbr,
                MerchantId = transaction.MerchantId,
                ValidatorTransactionId = transaction.TransactionId,
                TrxAmount = transaction.TrxAmount,
                TrxCurrencyCode = transaction.TrxCurrencyCode,
                Validator = transaction.Validator,
                WebSvcMethod = transaction.WebSvcMethod,
                CommerceItems = transaction.CommerceItems?.Select(ci => BuildCommerceItemModel(ci)).ToList() ?? new List<CommerceItemModel>(),
                PaymentClaims = transaction.PaymentClaim?.Select(pc => BuildPaymentClaimModel(language, pc)).ToList() ?? new List<PaymentClaimModel>(),
                TransactionCompletedStatus = transaction.TransactionStatus.Where(ts => ts.IsActual).Select(ts => BuildStatusCompletedInfo(language, ts)).FirstOrDefault(),
                TransactionNumber = transaction.TransactionAdditionalInfo.Where(tai => tai.TransactionIdPK == transaction.Id).Select(tai => tai.TransactionNumber).FirstOrDefault() 
            };

            trasactionCompletedInfo.TransactionId = CustomTool.ConvertTNtoTransaction(trasactionCompletedInfo.TransactionNumber);
            FillTransactionlInfo(trasactionCompletedInfo, transaction);

            return trasactionCompletedInfo;
        }

        private static void FillTransactionlInfo(TransactionCompletedInfo transactionCompletedInfo, Transactions transaction)
        {
            var transactionAdditionalInfo = transaction.TransactionAdditionalInfo.FirstOrDefault();
            var transactionResultInfo = transaction.TransactionResultInfo.FirstOrDefault();

            if (transactionAdditionalInfo != null)
            {
                transactionCompletedInfo.PayDate = transactionAdditionalInfo.CreatedOn;
                transactionCompletedInfo.AppVersion = transactionAdditionalInfo.VersionUsed;
                transactionCompletedInfo.BarCode = transactionAdditionalInfo.BarCode;
                transactionCompletedInfo.CallbackUrl = transactionAdditionalInfo.CallbackUrl;
                transactionCompletedInfo.CreatedBy = transactionAdditionalInfo.CreatedBy;
                transactionCompletedInfo.CreatedOn = transactionAdditionalInfo.CreatedOn;
                transactionCompletedInfo.EPCValidateURL = transactionAdditionalInfo.EPCValidateURL;
                transactionCompletedInfo.ExternalApp = transactionAdditionalInfo.ExternalApp;
                transactionCompletedInfo.IsEPCValidated = transactionAdditionalInfo.IsEPCValidated;
                transactionCompletedInfo.CreatedOn = transactionAdditionalInfo.CreatedOn;
                transactionCompletedInfo.TransactionCustomerMail = transactionAdditionalInfo.CustomerMail;
                transactionCompletedInfo.TransactionNumber = transactionAdditionalInfo.TransactionNumber;
                transactionCompletedInfo.UniqueCode = transactionAdditionalInfo.UniqueCode;

                if (transactionAdditionalInfo.Clients != null)
                {
                    transactionCompletedInfo.Client = new ClientModel
                    {
                        ClientId = transactionAdditionalInfo.Clients.ClientId,
                        LegalName = transactionAdditionalInfo.Clients.LegalName,
                        ShortName = transactionAdditionalInfo.Clients.ShortName,
                        SupportMail = transactionAdditionalInfo.Clients.SupportMail,
                        TributaryCode = transactionAdditionalInfo.Clients.TributaryCode
                    };
                }
                if (transactionAdditionalInfo.Products != null)
                {
                    transactionCompletedInfo.Product = new ProductModel
                    {
                        Description = transactionAdditionalInfo.Products.Description,
                        ProductCode = transactionAdditionalInfo.Products.ProductCode,
                        ProductId = transactionAdditionalInfo.Products.ProductId,
                        Type = transactionAdditionalInfo.Products.Type
                    };
                }
                if (transactionAdditionalInfo.Services != null)
                {
                    transactionCompletedInfo.Service = new ServiceModel
                    {
                        ServiceId = transactionAdditionalInfo.Services.ServiceId,
                        Description = transactionAdditionalInfo.Services.Description,
                        MerchantId = transactionAdditionalInfo.Services.MerchantId,
                        Name = transactionAdditionalInfo.Services.Name
                    };
                }
                if (transactionResultInfo != null)
                {
                    transactionCompletedInfo.AuthorizationCode = transactionResultInfo.AuthorizationCode;
                    transactionCompletedInfo.CardHolder = transactionResultInfo.CardHolder;
                    transactionCompletedInfo.CardMask = transactionResultInfo.CardMask;
                    transactionCompletedInfo.CardNbrLfd = transactionResultInfo.CardNbrLfd;
                    transactionCompletedInfo.Country = transactionResultInfo.Country;
                    transactionCompletedInfo.Currency = transactionResultInfo.Currency;
                    transactionCompletedInfo.TicketNumber = transactionResultInfo.TicketNumber;
                    transactionCompletedInfo.Payments = transactionResultInfo.Payments;
                    transactionCompletedInfo.CustomerDocNumber = transactionResultInfo.CustomerDocNumber;
                    transactionCompletedInfo.CustomerDocType = transactionResultInfo.CustomerDocType;
                }
            }
        }

        private static CommerceItemModel BuildCommerceItemModel(CommerceItems ci)
        {
            return new CommerceItemModel
            {
                Code = ci.Code,
                Description = ci.Description,
                Amount = ci.Amount,
                State = ci.State
            };
        }

        private static PaymentClaimModel BuildPaymentClaimModel(string language, PaymentClaim paymentClaim)
        {
            return new PaymentClaimModel
            {
                PaymentClaimNumber = paymentClaim.PaymentClaimNumber,
                Amount = paymentClaim.Amount,
                CurrencyDescription = paymentClaim.Currency.Description,
                Observation = paymentClaim.Observation,
                PaymentClaimCompletedStatus = paymentClaim.PaymentClaimStatus.Where(pcs => pcs.IsActual).Select(pcs => BuildStatusCompletedInfo(language, pcs)).FirstOrDefault(),
                Claimer = BuildClaimerModel(paymentClaim),
                CommerceItems = paymentClaim.CommerceItemClaim.Select(cic => BuildCommerceItemClaimModel(cic)).ToList()
            };
        }

        private static CommerceItemModel BuildCommerceItemClaimModel(CommerceItemClaim commerceItemClaim)
        {
            if (commerceItemClaim.CommerceItems == null) return new CommerceItemModel();

            return new CommerceItemModel
            {
                Code = commerceItemClaim.CommerceItems.Code,
                Description = commerceItemClaim.CommerceItems.Description,
                Amount = commerceItemClaim.CommerceItems.Amount,
                State = commerceItemClaim.CommerceItems.State
            };
        }

        private static ClaimerModel BuildClaimerModel(PaymentClaim pc)
        {
            if (pc.Claimer == null) return new ClaimerModel();

            return new ClaimerModel
            {
                Cellphone = pc.Claimer.Cellphone,
                DocNumber = pc.Claimer.DocNumber,
                DocShortName = pc.Claimer.DocType.ShortName,
                Email = pc.Claimer.Email,
                LastName = pc.Claimer.LastName,
                Name = pc.Claimer.Name,
                Phone = pc.Claimer.Phone
            };
        }

        private static StatusCompletedInfo BuildStatusCompletedInfo(string language, PaymentClaimStatus pcs)
        {
            if (pcs.StatusCode == null) return new StatusCompletedInfo();

            var statusMessage = pcs.StatusCode.StatusMessage.SingleOrDefault(sm => sm.Language.ISO6391 == language);
            return new StatusCompletedInfo
            {
                GenericCode = pcs.StatusCode.GenericCode.Type,
                GenericMessage = pcs.StatusCode.GenericCode.Description,
                ResponseCode = pcs.StatusCode.Code,
                ResponseMessage = statusMessage?.Message
            };
        }

        private static StatusCompletedInfo BuildStatusCompletedInfo(string language, TransactionStatus pcs)
        {
            if (pcs.StatusCode == null) return new StatusCompletedInfo();

            var statusMessage = pcs.StatusCode.StatusMessage.SingleOrDefault(sm => sm.Language.ISO6391 == language);
            return new StatusCompletedInfo
            {
                GenericCode = pcs.StatusCode.GenericCode.Type,
                GenericMessage = pcs.StatusCode.GenericCode.Description,
                ResponseCode = pcs.StatusCode.Code,
                ResponseMessage = statusMessage?.Message
            };
        }

        public static CallbackModel GetCallback(long transactionIdPK)
        {

            CallbackModel callback = new CallbackModel();
            using (var context = new PGDataEntities())
            {
                var transaction = context.Transactions.FirstOrDefault(tr => tr.Id == transactionIdPK);
                if (transaction != null)
                {
                    var transactionModel = Mapper.Transaction_EFToModel(transaction);
                    callback.ReturnUrl = transactionModel.CallbackUrl;

                    var serviceConfig = context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == transactionModel.ServiceId);
                    if (serviceConfig != null)
                    {
                        callback.IsCallbackPosted = serviceConfig.IsCallbackPosted;
                    }
                }
            }
            return callback;

        }

        public static ServiceModel GetServiceByTransactionId(long transactionIdPK)
        {
            using (var context = new PGDataEntities()) 
            {
                var taiEntry = context.TransactionAdditionalInfo.FirstOrDefault(tai => tai.TransactionIdPK == transactionIdPK);
                if (taiEntry != null)
                {
                    var service = context.Services.FirstOrDefault(s => s.ServiceId == taiEntry.ServiceId);
                    return Mapper.Service_EFToModel(service);
                }
                return null;
            }
        }

        //CHANGE 4.1 TransactionNumber PBI 2528
        public static TransactionModel GetTransactionByValidatorTransactionId(string validatorTransactionId, int validatorId)
        {
            using (var context = new PGDataEntities())
            {
                var transactionEntity = (from trans in context.Transactions
                                         join tai in context.TransactionAdditionalInfo
                                         on trans.Id equals tai.TransactionIdPK
                                         where trans.TransactionId == validatorTransactionId
                                         && tai.ValidatorId == validatorId
                                         select trans).FirstOrDefault();

                return Mapper.Transaction_EFToModel(transactionEntity);
            }
        }

    }
}