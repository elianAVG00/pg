using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Models;
using PGDataAccess.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;


namespace PGDataAccess.Repository
{
    static public class PaymentClaimRepository
    {
        public static bool CanUserWorkWithPaymentClaimByPaymentClaimNumber(string user, long claimnumber) {
            using (var context = new PGDataEntities())
            {
                var njm = (from tai in context.TransactionAdditionalInfo
                           join pc in context.PaymentClaim
                           on tai.TransactionIdPK equals pc.TransactionId
                           join us in context.UserService
                            on tai.ServiceId equals us.ServiceId
                           join u in context.User
                            on us.UserId equals u.Id
                           where u.IsActive && us.IsActive && u.username == user && pc.PaymentClaimNumber  == claimnumber
                           select tai.TransactionIdPK
                    ).FirstOrDefault();

                if (njm != null && njm != 0) {
                    return true;
                }
            }
            return false;
        }

        public static int? GetCommerceItemClaimIdByCommerceItemId(long id)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    CommerceItemClaim commerceItem = (from ci in context.CommerceItemClaim
                                                  where ci.CommerceItemId == id
                                                  select ci).FirstOrDefault();

                    if (commerceItem != null)
                    {
                       return commerceItem.CommerceItemClaimId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        static public AnnulmentResultInfoModel GetAnnulmentResultInfoByPaymentClaimId(long paymentClaimId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var annulmentInfo = (from ari in context.AnnulmentResultInfo
                                                         where ari.PaymentClaimId == paymentClaimId
                                                         select ari).FirstOrDefault();

                    return Mapper.AnnulmentResultInfo_EFToModel(annulmentInfo);
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

        static public void InsertAnnulmentResultInfo(AnnulmentResultInfoModel ARIToSave)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    ARIToSave.CreatedBy = "system";
                    ARIToSave.CreatedOn = DateTime.Now;
                    var relationToSave = Mapper.AnnulmentResultInfo_ModelToEF(ARIToSave);
                    context.AnnulmentResultInfo.Add(relationToSave);
                    context.SaveChanges();
                }
            }
            catch (NullReferenceException nullRefEx)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, nullRefEx);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Info, "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:", "Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage);
                        throw new Exception("Ocurrió un error interno", e.InnerException);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        public static bool LockUnlockPaymentClaim(long paymentClaimId, bool lockPaymentClaim)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    PaymentClaim paymentToLock = (from pc in context.PaymentClaim
                                                  where pc.PaymentClaimId == paymentClaimId
                                                  select pc).FirstOrDefault();
                    if (paymentToLock != null)
                    {
                        paymentToLock.IsLocked = lockPaymentClaim;
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogCommon(LogTypeModel.Warning, $"Failed to lock/unlock PaymentClaimId: {paymentClaimId}. Lock: {lockPaymentClaim}"
                    , ex.ToString());
            }
            return false;
        }

        public static bool IsPaymentClaimLocked(long paymentClaimId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    PaymentClaim paymentToLock = (from pc in context.PaymentClaim
                                                  where pc.PaymentClaimId == paymentClaimId
                                                  select pc).FirstOrDefault();
                    if (paymentToLock == null || paymentToLock.IsLocked)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        public static long? InsertPaymentClaim(PaymentClaimModel paymentClaim, ClaimerModel claimer, string moduleCode, string user)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string logParams = $"paymentClaim.PaymentClaimId={paymentClaim.PaymentClaimId}, claimer.DocNumber={claimer.DocNumber}, moduleCode={moduleCode}, user={user}";
                    LogRepository.InsertLogCommon(LogTypeModel.Info,  string.Concat("Parametros: ", logParams), TransactionNumber: paymentClaim.TransactionNumber.ToString(),logtransactionType:LogTransactionType.TransactionNumber);
                    
                    Claimer claimerEntity = Mapper.Claimer_ModelToEF(claimer);
                    claimerEntity.CreatedBy = string.IsNullOrEmpty(user) ? "system" : user;
                    claimerEntity.CreatedOn = DateTime.Now;

                    PaymentClaim paymentClaimEntity = Mapper.PaymentClaim_ModelToEF(paymentClaim);
                    var paymentClaimStatusEntity = new PaymentClaimStatus();
                    paymentClaimStatusEntity.PaymentClaimId = paymentClaimEntity.PaymentClaimId;
                    paymentClaimStatusEntity.StatusCodeId = StatusRepository.GetStatusCodeByCode(StatusRepository.GetStatusCodeByOriginalCode(moduleCode, "claim", null)).StatusCodeId;
                    paymentClaimStatusEntity.CreatedBy = string.IsNullOrEmpty(user) ? "system" : user;
                    paymentClaimStatusEntity.CreatedOn = DateTime.Now;
                    paymentClaimStatusEntity.IsActive = true;
                    paymentClaimStatusEntity.IsActual = true;
                    paymentClaimEntity.Claimer = claimerEntity;
                    paymentClaimEntity.IsActive = true;

                    context.Claimer.Add(claimerEntity);
                    context.PaymentClaim.Add(paymentClaimEntity);
                    context.PaymentClaimStatus.Add(paymentClaimStatusEntity);
                    context.SaveChanges();

                    //Insert CommerceItem relation
                    foreach (var commerceItem in paymentClaim.CommerceItems)
                    {
                        //Add relation

                        var commerceItemToClaim = new CommerceItemClaim() 
                        {
                            CommerceItemId = commerceItem.CommerceItemId,
                            PaymentClaimId = paymentClaimEntity.PaymentClaimId,
                            CreatedBy = "system",
                            CreatedOn = DateTime.Now,
                            IsActive = true
                        };
                        context.CommerceItemClaim.Add(commerceItemToClaim);

                        //Change state to open claim
                        //PaymentRepository.UpdateCommerceItemState(commerceItem.CommerceItemId, 1);
                    }
                    context.SaveChanges();
                    return paymentClaimEntity.PaymentClaimId;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        LogRepository.InsertLogCommon(LogTypeModel.Error, $"Entity: {validationErrors.Entry.Entity.GetType().Name}, State: {validationErrors.Entry.State}",
                                                                       $"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}", "PaymentClaimRepository.InsertPaymentClaim.Validation");
                    }
                }
                throw new Exception("Ocurrió un error de validación al insertar PaymentClaim", dbEx);
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                throw new Exception("Referencia nula");
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, ex);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        public static PaymentClaimModel GetActivePaymentClaimByTransactionId(string transactionId)
        {
            try
            {
                long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    PaymentClaim claimEntity = context.TransactionAdditionalInfo.FirstOrDefault(t => t.TransactionNumber == transactionNumber).Transactions.PaymentClaim.
                            FirstOrDefault(p => p.IsActive);
                    return Mapper.PaymentClaim_EFToModel(claimEntity);
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

        public static List<PaymentClaimModel> GetPaymentClaimsByTransactionId(string transactionId)
        {
            try
            {
                long transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("transactionId=", transactionId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    List<PaymentClaim> claimsEntities = (from c in context.PaymentClaim
                                                         join t in context.Transactions
                                                             on c.TransactionId equals t.Id
                                                         join tai in context.TransactionAdditionalInfo
                                                             on t.Id equals tai.TransactionIdPK
                                                         where
                                                             ((tai.TransactionNumber == transactionNumber) &&
                                                              (c.IsActive == true))
                                                         select (c)).ToList();

                    //context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId).PaymentClaim.ToList();

                    var claims = new List<PaymentClaimModel>();
                    foreach (var claimEntity in claimsEntities)
                    {
                        var paymentModelToAdd = new PaymentClaimModel();
                        paymentModelToAdd = Mapper.PaymentClaim_EFToModel(claimEntity);

                        paymentModelToAdd.ActualStateCode =
                            StatusRepository.GetStatusCodeOfPaymentClaim(paymentModelToAdd.PaymentClaimId).Code;

                        claims.Add(paymentModelToAdd);
                    }
                    return claims;
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


        public static long? GetPaymentClaimIdByNumber (long paymentClaimNumber)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    #region Log

                    string inputParameters = string.Concat("paymentClaimNumber=", paymentClaimNumber);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    #endregion Log

                    var paymentClaimId = context.PaymentClaim.FirstOrDefault(
                            c => c.PaymentClaimNumber == paymentClaimNumber && c.IsActive).PaymentClaimId;


                    return paymentClaimId;
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

        public static PaymentClaimModel GetPaymentClaimById(long paymentClaimId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    #region Log

                    string inputParameters = string.Concat("paymentClaimId=", paymentClaimId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    #endregion Log

                    var claimEntity = context.PaymentClaim.Include("Claimer").FirstOrDefault(
                            c => c.PaymentClaimId == paymentClaimId && c.IsActive);

                    var paymentModelToReturn = Mapper.PaymentClaim_EFToModel(claimEntity);
                    paymentModelToReturn.ActualStateCode = StatusRepository.GetStatusCodeOfPaymentClaim(paymentModelToReturn.PaymentClaimId).Code;

                    return paymentModelToReturn;
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


        public static List<PaymentClaimModel> GetAllPaymentClaimByStatusCode(string statusCode, bool isAutomitized)
        {
            using (var context = new PGDataEntities())
            {
                StatusCodeModel statusCodeModel = StatusRepository.GetStatusCodeByCode(statusCode);
                List<PaymentClaimStatus> paymentClaimStatusList =
                    context.PaymentClaimStatus.Where(
                        pcs => pcs.StatusCodeId == statusCodeModel.StatusCodeId && pcs.IsActual && pcs.IsActive).ToList();
                List<PaymentClaimModel> playmentClaimList = new List<PaymentClaimModel>();
                foreach (PaymentClaimStatus paymentClaimStatus in paymentClaimStatusList)
                {
                    PaymentClaim paymentClaimEntity =
                        context.PaymentClaim.FirstOrDefault(pc => pc.PaymentClaimId == paymentClaimStatus.PaymentClaimId);
                    if (paymentClaimEntity != null)
                        playmentClaimList.Add(Mapper.PaymentClaim_EFToModel(paymentClaimEntity));
                }
                return playmentClaimList;
            }
        }

        public static int InsertAnnulmentRequest(AnnulmentRequestModel annulmentRequest)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var annulment = Mapper.AnnulmentRequest_ModelToEF(annulmentRequest);
                    context.AnnulmentRequest.Add(annulment);
                    context.SaveChanges();
                    return annulment.AnnulmentRequestId;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
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

        public static void UpdateAnnulmentRequest(AnnulmentRequestModel annulmentRequest)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    AnnulmentRequest annulmentRequestEntity =
                        context.AnnulmentRequest.FirstOrDefault(
                            ar => ar.AnnulmentRequestId == annulmentRequest.AnnulmentRequestId);
                    annulmentRequestEntity.UpdatedBy = annulmentRequest.UpdatedBy;
                    annulmentRequestEntity.UpdatedOn = annulmentRequest.UpdatedOn;
                    annulmentRequestEntity.ResponseModuleCode = annulmentRequest.ResponseModuleCode;
                    annulmentRequestEntity.Result = annulmentRequest.Result;
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

        public static int? InsertAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var annulmentComm = Mapper.AnnulmentValidatorComm_ModelToEF(annulmentValidatorCommModel);
                    context.AnnulmentValidatorComm.Add(annulmentComm);
                    context.SaveChanges();
                    return annulmentComm.AnnulmentValidatorCommId;
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

        public static void UpdateAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var annulmentValidatorCommEntity =
                        context.AnnulmentValidatorComm.FirstOrDefault(
                            ar => ar.AnnulmentValidatorCommId == annulmentValidatorCommModel.AnnulmentValidatorCommId);
                    annulmentValidatorCommEntity.ResponseDate = annulmentValidatorCommModel.ResponseDate;
                    annulmentValidatorCommEntity.ResponseMessage = annulmentValidatorCommModel.ResponseMessage;
                    context.SaveChanges();
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
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static AnnulmentValidatorCommModel GetAnnulmentValidatorCommById(int annulmentValidatorCommId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("annulmentValidatorCommId=", annulmentValidatorCommId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, 
                                  string.Concat("Parametros: ", inputParameters));

                    AnnulmentValidatorComm annulmentValidatorCommEntity =
                        context.AnnulmentValidatorComm.FirstOrDefault(
                            avc => avc.AnnulmentValidatorCommId == annulmentValidatorCommId);
                    return Mapper.AnnulmentValidatorComm_EFToModel(annulmentValidatorCommEntity);
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, nullex);
                return null;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        public static ClaimerModel GetClaimerById(int claimerId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    string inputParameters = string.Concat("claimerId=", claimerId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));

                    Claimer claimerEntity =
                        context.Claimer.Include("DocType").FirstOrDefault(c => c.ClaimerId == claimerId);
                    ClaimerModel claimer = Mapper.Claimer_EFToModel(claimerEntity);
                    return (claimer);
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

        public static List<StatusTicketModel> GetTicketsOfPaymentClaim(long paymentClaimNumber)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var statusTicketList = new List<StatusTicketModel>();
                    PaymentClaim paymentClaimEntity = context.PaymentClaim.FirstOrDefault(pc => pc.PaymentClaimNumber == paymentClaimNumber);

                    if (paymentClaimEntity != null && paymentClaimEntity.PaymentClaimId != 0)
                    {
                        var statusList = context.PaymentClaimStatus.Where(pcs => pcs.PaymentClaimId == paymentClaimEntity.PaymentClaimId 
                                                                                                        && pcs.TicketNumber != null).Include(pcs => pcs.StatusCode).ToList();

                        foreach (PaymentClaimStatus paymentClaimStatusItem in statusList)
                        {
                            if (paymentClaimStatusItem.TicketNumber != null)
                            {
                                var ticketLogEntry = context.TicketLog.FirstOrDefault(t => t.TicketNumber == paymentClaimStatusItem.TicketNumber.Value);
                                var statusTicket = new StatusTicketModel()
                                {
                                    StatusCode = paymentClaimStatusItem.StatusCode?.Code,
                                    Ticket = ticketLogEntry.HtmlTicket
                                };
                                statusTicketList.Add(statusTicket);
                            }
                        }
                    }
                    if (statusTicketList.Count > 0)
                        return statusTicketList;
                    else
                        return null;

                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex, "PaymentClaimRepository.GetTicketsOfPaymentClaim");
                throw ex;
            }
        }

        public static List<PaymentClaimModel> GetPaymentClaimsByCriteria(FilterCriteriaModel criteria)
        {
            try
            {
                List<PaymentClaimModel> listadoDePaymentClaim = new List<PaymentClaimModel>();
                using (var context = new PGDataEntities())
                {

                    listadoDePaymentClaim = (
                        from paymentclaim in context.PaymentClaim
                        join trans in context.Transactions
                        on paymentclaim.TransactionId equals trans.Id
                        join tai in context.TransactionAdditionalInfo
                        on trans.Id equals tai.TransactionIdPK
                        join paymentClaimStatus in context.PaymentClaimStatus
                        on paymentclaim.PaymentClaimId equals paymentClaimStatus.PaymentClaimId
                        join statuscode in context.StatusCode
                        on paymentClaimStatus.StatusCodeId equals statuscode.StatusCodeId

                        select new PaymentClaimModel
                        {

                            PaymentClaimId = paymentclaim.PaymentClaimId,
                            PaymentClaimNumber = paymentclaim.PaymentClaimNumber,
                            ClaimerId = paymentclaim.ClaimerId,
                            TransactionIdPK = paymentclaim.TransactionId,
                            Amount = paymentclaim.Amount,
                            Observation = paymentclaim.Observation,
                            IsLocked = paymentclaim.IsLocked,
                            IsActive = paymentclaim.IsActive,
                            CreatedBy = paymentclaim.CreatedBy,
                            CreatedOn = paymentclaim.CreatedOn,
                            UpdatedBy = paymentclaim.UpdatedBy,
                            UpdatedOn = paymentclaim.UpdatedOn,
                            StatusCode = statuscode.Code,
                            //CHANGE 4.1 TransactionNumber PBI 2528
                            //Old
                            //TransactionId = trans.TransactionId, 
                            //New
                            TransactionNumber = tai.TransactionNumber,
                           
                            MerchantId = tai.MerchantId,
                            Product = trans.Product,
                            ProductId = tai.ProductId,
                        }
                        ).ToList();


                }

                foreach (PaymentClaimModel toUpdate in listadoDePaymentClaim)
                {
                    toUpdate.TransactionId = CustomTool.ConvertTNtoTransaction(toUpdate.TransactionNumber);
                }

                return listadoDePaymentClaim;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }


        public static List<CommerceItemModel> GetCommerceItemsToClaimByTransactionIdPK(long id)
        {
            List<CommerceItemModel> listToReturn = new List<CommerceItemModel>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    List<CommerceItems> commerceItems = new List<CommerceItems>();

                    commerceItems = (from ci in context.CommerceItems
                                     where ci.TransactionIdPK == id && ci.State == 0 //State 0 para no anulados
                                     select ci).ToList();

                    if (commerceItems != null)
                    {
                        foreach (CommerceItems commerceItem in commerceItems)
                        {
                            //TODO Migrate To Map
                            listToReturn.Add(new CommerceItemModel
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
                                TransactionId = commerceItem.TransactionIdPK
                            });
                        }
                        return listToReturn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }

        }

        //FGS2 - Devuelvo los CIs con un reclamo asociado iniciado
        public static List<CommerceItemModel> GetCommerceItemsClaimedByTransactionIdPK(long transactionIdPK)
        {
            using (var context = new PGDataEntities())
            {
                List<CommerceItemClaim> commerceItemsClaimsList = context.PaymentClaim.FirstOrDefault(pc => pc.TransactionId == transactionIdPK).CommerceItemClaim.ToList();
                List<CommerceItemModel> commerceItemList = new List<CommerceItemModel>();
                foreach (CommerceItemClaim commerceItemClaim in commerceItemsClaimsList)
                {
                    commerceItemList.Add(Mapper.CommerceItem_EFToModel(context.CommerceItems.FirstOrDefault(ci => ci.CommerceItemsId == commerceItemClaim.CommerceItemClaimId)));
                }
                return commerceItemList;
            }
            
        }

        //FGS2 - Devuelvo si la transaccion ya tiene un reclamo asociado que no 
        public static bool ExistClaimForCommerceItem(CommerceItemModel commerceItem)
        {
            using (var context = new PGDataEntities())
            {
                //Me traigo el reclamo que exista para ese commerceItem
                PaymentClaim claim = context.CommerceItemClaim.FirstOrDefault(ci => ci.CommerceItemId == commerceItem.CommerceItemId).PaymentClaim;
                //¿Tengo que preguntar si el reclamo esta en un estado espeficifo? 
                if (claim != null)
                    return true;
                else
                    return false; 
            }
        }
        
       //FGS2 - Devuelvo los CIs sin anular para una transaccion
        public static List<CommerceItemModel> GetCommerceItemNotAnnulledByTransactionIdPK(long transactionIdPK)       
        {
            using (var context = new PGDataEntities())
            {

                Transactions transaction = context.Transactions.Include("PaymentClaim").FirstOrDefault(t => t.Id == transactionIdPK);
                List<PaymentClaim> claims = transaction.PaymentClaim.ToList();
                var notAnnuledClaims = new List<PaymentClaim>();
 
                foreach (PaymentClaim claim in claims)
                {
                    var actualStatus = claim.PaymentClaimStatus?.FirstOrDefault(pc => pc.IsActual);
                    if (actualStatus?.StatusCodeId != 30)
                    {
                        notAnnuledClaims.Add(claim);
                    }
                }

                var resultCommerceItems = new List<CommerceItemModel>();
                foreach (PaymentClaim notAnnulledClaim in notAnnuledClaims)
                {
                    if (notAnnulledClaim.CommerceItemClaim != null)
                    {
                        foreach (CommerceItemClaim commerceItemClaim in notAnnulledClaim.CommerceItemClaim)
                        {
                            if (commerceItemClaim.CommerceItems != null)
                            {
                                resultCommerceItems.Add(Mapper.CommerceItem_EFToModel(commerceItemClaim.CommerceItems));
                            }
                        }
                    }
                }

                return resultCommerceItems;
            }
        }

        //FGS2 - Devuelvo los commerceItems asociados a un reclamo
        public static List<CommerceItemModel> GetCommerceItemByPaymentClaimId(long paymentClaimId)
        {
            List<CommerceItemModel> commerceItemList = new List<CommerceItemModel>();
            using (var context = new PGDataEntities())
            {
                List<CommerceItemClaim> commerceItemsClaimEntityList= context.PaymentClaim.FirstOrDefault(pc => pc.PaymentClaimId == paymentClaimId).CommerceItemClaim.ToList();
                foreach (var commerceItemClaimEntity in commerceItemsClaimEntityList)
                {
                    var aux = context.CommerceItems.FirstOrDefault(ci => ci.CommerceItemsId == commerceItemClaimEntity.CommerceItemId);
                    if (aux != null)
                        commerceItemList.Add(Mapper.CommerceItem_EFToModel(aux));
                }
                return commerceItemList;
            }
        }
    }
}