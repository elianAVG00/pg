using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace PGDataAccess.Repository
{
    static public class StatusRepository
    {
        static public string GetValidationResponseByStatusCode(string StatusCode, string language = "es")
        {
            string httpResponse = "Error in validation code. Please contact the administrator";
            try
            {
                using (var context = new PGDataEntities())
                {
                    httpResponse = (from sc in context.StatusCode
                                    join sm in context.StatusMessage
                                        on sc.StatusCodeId equals sm.StatusCodeId
                                    where (sc.Code == StatusCode)
                                    && sc.IsActive && sm.IsActive
                                    && (sm.Language.ISO6391 == language || sm.Language.ISO6391 == language || sm.Language.ISO6392 == language)
                                    select sm.Message).FirstOrDefault();
                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public HTTPResponse_Model GetHTTPResponseByStatusCodeOrPGCode(string StatusCodeOrPGCode, string language = "es")
        {
            HTTPResponse_Model httpResponse = new HTTPResponse_Model();
            HTTPResponse_Model httpResponseERR = new HTTPResponse_Model();
            httpResponseERR.HTTPStatusCode = 500;
            httpResponseERR.Message = "NACK - Service is unavailable or there is an error on the request - Error HTTP500";
            try
            {
                using (var context = new PGDataEntities())
                {
                    httpResponse = (from sc in context.StatusCode
                                    join sm in context.StatusMessage
                                        on sc.StatusCodeId equals sm.StatusCodeId
                                    join cm in context.CodeMapping
                                    on sm.StatusCodeId equals cm.StatusCodeId
                                    join mc in context.ModuleCode
                                    on cm.ModuleCodeId equals mc.ModuleCodeId
                                    where (sc.Code == StatusCodeOrPGCode || mc.OriginalCode == StatusCodeOrPGCode)
                                    && sc.IsActive && sm.IsActive
                                    && (sm.Language.ISO6391 == language || sm.Language.ISO6391 == language || sm.Language.ISO6392 == language)
                                    select new HTTPResponse_Model
                                    {
                                        Message = sm.Message,
                                        StatusCode = sc.Code,
                                        PG_Code = mc.OriginalCode,
                                        StatusCodeId = sc.StatusCodeId
                                    }).FirstOrDefault();
                    if (httpResponse != null)
                    {
                        httpResponse.HTTPStatusCode = Convert.ToInt16(httpResponse.StatusCode.Split('_')[1]);
                        return httpResponse;
                    }
                    else {
                        return httpResponseERR;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public string GetModuleCodeOfTransaction(string transactionId)
        {
            StatusCodeModel scm = GetStatusCodeOfTransaction(transactionId);

            if (scm != null)
            {
                return GetModuleCodeByStatusCode("payment", scm.Code);
            }
            else
            {
                return "PGPAYMENTUNDEFINED";
            }
        }

        static public StatusCodeModel GetStatusCodeOfTransaction(string transactionId)
        {
            try
            {
                long transactionNumber = Tools.CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("TransactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters), TransactionNumber: transactionNumber.ToString(), logtransactionType: LogTransactionType.TransactionNumber);
                    Transactions transactionEntity = context.TransactionAdditionalInfo.FirstOrDefault(t => t.TransactionNumber == transactionNumber).Transactions;
                    if (transactionEntity != null)
                    {
                        StatusCode statusCodeEntity = context.TransactionStatus
                            .FirstOrDefault(ts => ts.TransactionsId == transactionEntity.Id && ts.IsActual).StatusCode;
                        return Mapper.StatusCode_EFToModel(statusCodeEntity);
                    }
                    return null;
                }
            }
            catch (NullReferenceException nullRefEx)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullRefEx);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public StatusResponseMessageModel GetStatusMessageByOriginalCode(string modulteType, string originalCode, string languageCodeISO6391)
        {
            var response = new StatusResponseMessageModel();
            var statusCode = GetStatusCodeByOriginalCode(originalCode, modulteType, null);

            try
            {
                using (var context = new PGDataEntities())
                {
                    response = (from sm in context.StatusMessage
                                join lang in context.Language on sm.IdLanguage equals lang.Id
                                join sc in context.StatusCode on sm.StatusCodeId equals sc.StatusCodeId
                                join gc in context.GenericCode on sc.GenericCodeId equals gc.Id
                                where (lang.ISO6391 == languageCodeISO6391) && (sc.Code == statusCode)
                                select new StatusResponseMessageModel()
                                {
                                    ResponseStatusCode = sc.Code,
                                    ResponseGenericCode = gc.Type,
                                    ResponseGenericMessage = gc.Description,
                                    ResponseStatusMessage = sm.Message
                                }).FirstOrDefault();

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public string MapOriginalCodeFromValidatorToPGCode(string originalCode, string moduleTypeFrom, string moduleTypeTo, int? validatorFrom = null)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    int StatusCodeIdForModule = (from mc in context.ModuleCode
                                                 join cm in context.CodeMapping on mc.ModuleCodeId equals cm.ModuleCodeId
                                                 join m in context.Module on mc.ModuleId equals m.Id
                                                 where (mc.OriginalCode == originalCode) && (m.Type == moduleTypeFrom) && (m.validatorId == validatorFrom)
                                                 select cm.StatusCodeId).FirstOrDefault();

                    string PGCodeToReturn = (from mc in context.ModuleCode
                                             join cm in context.CodeMapping on mc.ModuleCodeId equals cm.ModuleCodeId
                                             join m in context.Module on mc.ModuleId equals m.Id
                                             where (cm.StatusCodeId == StatusCodeIdForModule) && (m.Type == moduleTypeTo) && (m.validatorId == null)
                                             select mc.OriginalCode).FirstOrDefault();

                    return PGCodeToReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public string GetStatusCodeByOriginalCode(string originalCode, string moduleType, int? validator = null)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string StatusCodeToReturn = (from mc in context.ModuleCode
                                                 join cm in context.CodeMapping on mc.ModuleCodeId equals cm.ModuleCodeId
                                                 join sc in context.StatusCode on cm.StatusCodeId equals sc.StatusCodeId
                                                 join m in context.Module on mc.ModuleId equals m.Id
                                                 where (mc.OriginalCode == originalCode) && (m.Type == moduleType) && (m.validatorId == validator)
                                                 select sc.Code).FirstOrDefault();

                    return StatusCodeToReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public string GetModuleCodeByStatusCode(string moduleType, string StatusCode, int? moduleValidator = null)
        {
            try
            {
                using (var context = new PGDataEntities())
                {

                    string ModuleCodeToReturn = (from mc in context.ModuleCode
                                                 join m in context.Module on mc.ModuleId equals m.Id
                                                 join cm in context.CodeMapping on mc.ModuleCodeId equals cm.ModuleCodeId
                                                 join sc in context.StatusCode on cm.StatusCodeId equals sc.StatusCodeId
                                                 where (sc.Code == StatusCode) && ((m.validatorId == moduleValidator) && (m.Type == moduleType))
                                                 select mc.OriginalCode).FirstOrDefault();
                    return ModuleCodeToReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());

            }
        }

        static public StatusCodeModel GetStatusCodeByCode(string code)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("code=", code);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    return Mapper.StatusCode_EFToModel(context.StatusCode.FirstOrDefault(sc => sc.Code == code && sc.IsActive));
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

        static public void UpdateTransactionStatus(long transactionIdPK, string originalCode, string typeMethod, int? validator = null)
        {
            // "webservice"
            // "payment"
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionIdPK=", transactionIdPK, ",statusCode=", originalCode);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    SetOldStatusOnNotActive_Payment(transactionIdPK);
                    var transStatus = new TransactionStatus();

                    transStatus.StatusCodeId = GetStatusCodeByCode(GetStatusCodeByOriginalCode(originalCode, typeMethod, validator)).StatusCodeId;


                    transStatus.TransactionsId = transactionIdPK;

                    transStatus.CreatedBy = "system";
                    transStatus.CreatedOn = DateTime.Now;
                    transStatus.IsActual = true;
                    transStatus.IsActive = true;
                    context.TransactionStatus.Add(transStatus);
                    context.SaveChanges();
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public PaymentClaimStatusModel UpdatePaymentClaimStatus(long paymentClaimId, string user, string moduleCode, string observations)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("paymentClaimId=", paymentClaimId, ",statusCode=", moduleCode, ",Observations=", observations);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    SetOldStatusOnNotActive_Claim(paymentClaimId);
                    var paymentClaimStatusEntity = new PaymentClaimStatus();
                    paymentClaimStatusEntity.PaymentClaimId = paymentClaimId;
                    paymentClaimStatusEntity.StatusCodeId = GetStatusCodeByCode(GetStatusCodeByOriginalCode(moduleCode, "claim")).StatusCodeId;
                    paymentClaimStatusEntity.CreatedBy = string.IsNullOrEmpty(user) ? "system" : user;
                    paymentClaimStatusEntity.CreatedOn = DateTime.Now;
                    paymentClaimStatusEntity.IsActual = true;
                    paymentClaimStatusEntity.IsActive = true;
                    paymentClaimStatusEntity.Observation = observations;
                    context.PaymentClaimStatus.Add(paymentClaimStatusEntity);
                    context.SaveChanges();

                    return Mapper.PaymentClaimStatus_EFToModel(paymentClaimStatusEntity);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return null;
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

        static public void SetTicketToPaymentClaimStatus(long paymentClaimNumber, string statusCode, int ticketNumber)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Format("paymentClaimNumber= {0}, statusCode= {1}, ticketNumber= {2}", paymentClaimNumber, statusCode, ticketNumber);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    PaymentClaim paymentClaimEntity = context.PaymentClaim.FirstOrDefault(pc => pc.PaymentClaimNumber == paymentClaimNumber);
                    StatusCode statusCodeEntity = context.StatusCode.FirstOrDefault(sc => sc.Code == statusCode);

                    PaymentClaimStatus paymentClaimStatusEntity = context.PaymentClaimStatus.FirstOrDefault(pcs => pcs.PaymentClaimId == paymentClaimEntity.PaymentClaimId && pcs.StatusCodeId == statusCodeEntity.StatusCodeId);
                    paymentClaimStatusEntity.TicketNumber = ticketNumber;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public StatusCodeModel GetStatusCodeOfPaymentClaim(long paymentClaimId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("paymentClaimId=", paymentClaimId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    PaymentClaim paymentClaimEntity = context.PaymentClaim.FirstOrDefault(p => p.PaymentClaimId == paymentClaimId && p.IsActive);
                    PaymentClaimStatus pcstatus = context.PaymentClaimStatus.FirstOrDefault(pcs => pcs.PaymentClaimId == paymentClaimEntity.PaymentClaimId && pcs.IsActual && pcs.IsActive);
                    StatusCode statusCodeEntity = pcstatus.StatusCode;
                    return Mapper.StatusCode_EFToModel(statusCodeEntity);
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

        static public StatusResponseMessageModel GetStatusMessageByStatusCode(string statusCode, string languageCode)
        {
            var response = new StatusResponseMessageModel();
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("statusCode=", statusCode);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    response = (from sm in context.StatusMessage
                                join sc in context.StatusCode on sm.StatusCodeId equals sc.StatusCodeId
                                join gc in context.GenericCode on sc.GenericCodeId equals gc.Id
                                join l in context.Language on sm.IdLanguage equals l.Id
                                where l.ISO6391 == languageCode && sc.Code == statusCode
                                select new StatusResponseMessageModel()
                                {
                                    ResponseStatusCode = sc.Code,
                                    ResponseGenericCode = gc.Type,
                                    ResponseGenericMessage = gc.Description,
                                    ResponseStatusMessage = sm.Message
                                }
                                ).FirstOrDefault();

                    return response;
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

        static public StatusMessageModel GetStatusMessageByPaymentClaim(long paymentClaimId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("paymentClaimId=", paymentClaimId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    PaymentClaimStatus paymentClaimStatusEntity = context.PaymentClaimStatus.FirstOrDefault(pcs => pcs.PaymentClaim.PaymentClaimId == paymentClaimId && pcs.IsActual && pcs.IsActive);
                    if (paymentClaimStatusEntity != null)
                    {
                        StatusCode statusCodeEntity = context.StatusCode.FirstOrDefault(sc => sc.StatusCodeId == paymentClaimStatusEntity.StatusCodeId);
                        if (statusCodeEntity != null)
                        {
                            StatusMessageModel statusMessage = Mapper.StatusMessage_EFToModel(context.StatusMessage.FirstOrDefault(sm => sm.StatusCodeId == statusCodeEntity.StatusCodeId));
                            return statusMessage;
                        }
                        else
                            return null;
                    }
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
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public string GetModuleCodeOfPaymentClaim(long paymentClaimId)
        {
            StatusCodeModel scm = GetStatusCodeOfPaymentClaim(paymentClaimId);
            if (scm != null)
            {
                return GetModuleCodeByStatusCode("claim", scm.Code);
            }
            else
            {
                return "PGCLAIMUNDEFINED";
            }
        }

        static public StatusCodeStatusMessageModel GetLastPaymentClaimStatus(long paymentClaimNumber, string languageCode)
        {
            using (var context = new PGDataEntities())
            {
                var message = (from pc in context.PaymentClaim
                               join pcs in context.PaymentClaimStatus on pc.PaymentClaimId equals pcs.PaymentClaimId
                               join sc in context.StatusCode on pcs.StatusCodeId equals sc.StatusCodeId
                               join sm in context.StatusMessage on sc.StatusCodeId equals sm.StatusCodeId
                               join l in context.Language on sm.IdLanguage equals l.Id
                               where l.ISO6391 == languageCode && pc.PaymentClaimNumber == paymentClaimNumber && pcs.IsActual
                               select new { sc.Code, sm.Message }).FirstOrDefault();

                var statusCodeStatusMessageModel = new StatusCodeStatusMessageModel();
                if (message != null)
                {
                    statusCodeStatusMessageModel.Code = message.Code;
                    statusCodeStatusMessageModel.Message = message.Message;
                    return statusCodeStatusMessageModel;
                }
                else
                    return null;
            }
        }

        static public List<StatusCodeStatusMessageModel> GetPaymentClaimStatusHistory(long paymentClaimNumber, string languageCode)
        {
            using (var context = new PGDataEntities())
            {
                var statusCodeStatusMessageModelList = new List<StatusCodeStatusMessageModel>();

                var statusCodeMessages = (from pc in context.PaymentClaim
                                          join pcs in context.PaymentClaimStatus on pc.PaymentClaimId equals pcs.PaymentClaimId
                                          join sc in context.StatusCode on pcs.StatusCodeId equals sc.StatusCodeId
                                          join sm in context.StatusMessage on sc.StatusCodeId equals sm.StatusCodeId
                                          join l in context.Language on sm.IdLanguage equals l.Id
                                          where l.ISO6391 == languageCode && pc.PaymentClaimNumber == paymentClaimNumber
                                          orderby pcs.PaymentClaimStatusId
                                          select new { sc.Code, sm.Message, pcs.CreatedBy, pcs.CreatedOn, pcs.Observation }).ToArray();

                foreach (var statusCodeMessage in statusCodeMessages)
                {
                    var statusCodeStatusMessageModel = new StatusCodeStatusMessageModel
                    {
                        Code = statusCodeMessage.Code,
                        Message = statusCodeMessage.Message,
                        CreatedBy = statusCodeMessage.CreatedBy,
                        CreatedOn = statusCodeMessage.CreatedOn,
                        Observation = statusCodeMessage.Observation
                    };

                    statusCodeStatusMessageModelList.Add(statusCodeStatusMessageModel);
                }
                return statusCodeStatusMessageModelList;
            }
        }

        static public GenericCodeModel GetGenericCodeByStatusCode(string statusCode)
        {
            using (var context = new PGDataEntities())
            {
                GenericCode genericCodeEntity = context.StatusCode.FirstOrDefault(sc => sc.Code == statusCode).GenericCode;
                return Mapper.GenericCode_EFToModel(genericCodeEntity);
            }
        }
        #region Private Methods

        static private void SetOldStatusOnNotActive_Claim(long paymentClaimId)
        {
            using (var context = new PGDataEntities())
            {
                List<PaymentClaimStatus> claims = context.PaymentClaimStatus.Where(pcs => pcs.PaymentClaimId == paymentClaimId).ToList();
                foreach (PaymentClaimStatus claim in claims)
                {
                    claim.IsActual = false;
                }
                context.SaveChanges();
            }
        }

        static private void SetOldStatusOnNotActive_Payment(long transactionIdPK)
        {
            using (var context = new PGDataEntities())
            {
                List<TransactionStatus> allTransactionsStatus = context.TransactionStatus.Where(t => t.TransactionsId == transactionIdPK).ToList();
                foreach (TransactionStatus transactionStatus in allTransactionsStatus)
                {
                    transactionStatus.IsActual = false;
                }
                context.SaveChanges();
            }
        }
        #endregion
    }
}