using PGDataAccess.EF;
using PGDataAccess.Models;
using PGDataAccess.Repository;
using PGDataAccess.Services;
using System;
using System.Collections.Generic;

namespace PGDataAccess
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PGDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PGDataService.svc or PGDataService.svc.cs at the Solution Explorer and start debugging.
    public class PGDataService : IPGDataService
    {

        #region Job Methods


        public void InsertRefundJobRunLog(List<long> CommerceItemId, int JobRunLog)
        {
            JobRepository.InsertRefundJobRunLog(CommerceItemId, JobRunLog);
        }


        public void InsertReportJobRunLog(List<long> CommerceItemId, int JobRunLog, string ReportType)
        {
            JobRepository.InsertReportJobRunLog(CommerceItemId, JobRunLog, ReportType);
        }

        public List<ReporteRenditionExportModel> GetRenditionInformation()
        {
            return JobRepository.GetRenditionInformation();
        }


        public int InsertJobRunLog(JobLogModel jobToInsert)
        {
            return JobRepository.InsertJobRunLog(jobToInsert);
        }
        public void UpdateJobRunLog(JobLogModel jobToUpdate)
        {
            JobRepository.UpdateJobRunLog(jobToUpdate);
        }

        #endregion


        #region Mail

        public void SendEmail(MailerModel basicMail)
        {
            MailerService.SendEmail(basicMail);
        }


        #endregion

        #region Status App Config and Test Methods

        public List<AppConfigModel> GetAllConfigs()
        {
            return AppConfigRepository.GetAllConfigs();
        }

        public bool IsJobConsoleTaskOffline(string task)
        {
            return AppConfigRepository.IsJobConsoleTaskOffline(task);
        }



        public bool IsPaymentOffline()
        {
            return AppConfigRepository.IsPaymentOffline();
        }

        public bool IsServiceOffline()
        {
            return AppConfigRepository.IsServiceOffline();
        }

        public List<string> GetLastPayment()
        {
            return AppConfigRepository.GetLastPayment();
        }

        public string SaveNewBuild(string tfschange)
        {
            return AppConfigRepository.SaveNewBuild(tfschange);
        }

        public void SetAppConfiguration(string setting, string newvalue)
        {
            AppConfigRepository.SetAppConfiguration(setting, newvalue);
        }

        public string TestDAS()
        {
            return "OK";
        }

        public string TestDBUpdate()
        {
            return AppConfigRepository.TestDBUpdate();
        }

        public string TestDBInsert()
        {
            return AppConfigRepository.TestDBInsert();
        }

        public string TestDBSelect()
        {
            return AppConfigRepository.TestDBSelect();
        }

        public string TestDBDelete()
        {
            return AppConfigRepository.TestDBDelete();
        }

        public AppInfo GetStatus()
        {
            return AppConfigRepository.GetStatus();
        }

        public string GetAppVersion(string appVersion)
        {
            return AppConfigRepository.GetAppVersion(appVersion);
        }

        public string GetAppConfiguration(string setting)
        {
            return AppConfigRepository.GetAppConfiguration(setting);
        }

        public string TestSMTP()
        {
            return AppConfigRepository.TestSMTP();
        }


        #endregion

        #region Report Methods



        public List<RePrintModel> GetTicketInformationToRePrint(DateTime PurchaseDate, int ExternalId, string CreditCard4LastDigits, string AuthorizationCode)
        {
            return ReportRepository.GetTicketInformationToRePrint(PurchaseDate, ExternalId, CreditCard4LastDigits, AuthorizationCode);
        }

        public List<ReporteCentralizadorModel> GetReport814(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            return ReportRepository.Get814(transactionId, CommerceItemCode, merchantId, RangoDesde, RangoHasta);
        }

        public List<ReporteCentralizadorOrRefundionRefundModel> GetReport814Refunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            return ReportRepository.Get814Refunds(transactionId, CommerceItemCode, merchantId, RangoDesde, RangoHasta);
        }

        public List<ReporteConciliacionModel> GetReportConciliation(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            return ReportRepository.GetConciliation(transactionId, CommerceItemCode, merchantId, RangoDesde, RangoHasta);
        }

        public List<ReporteRenditionModel> GetReportRendition(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            return ReportRepository.GetRendition(transactionId, CommerceItemCode, merchantId, RangoDesde, RangoHasta);
        }

        public List<ReporteCentralizadorOrRefundionRefundModel> GetReportRenditionRefunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            return ReportRepository.GetRenditionRefunds(transactionId, CommerceItemCode, merchantId, RangoDesde, RangoHasta);
        }


        #endregion

        #region Log Methods







        public long SaveLog(LogModel log)
        {
            return LogRepository.SaveLog(log);
        }

        #endregion Log Methods

        #region Security Methods

        public List<ServiceModel> GetServicesByUser(int userId)
        {
            return SecurityRepository.GetServicesByUser(userId);
        }

        public ValidatorServiceConfigModel GetValidatorConfigByServiceId(int serviceId, int validatorId)
        {
            return SecurityRepository.GetValidatorConfigByServiceId(serviceId, validatorId);
        }

        public List<Models.RolModel> GetRolesByUsername(string username)
        {
            return SecurityRepository.GetRolesByUsername(username);
        }

        public Models.UserModel GetUser(int userId)
        {
            return SecurityRepository.GetUser(userId);
        }

        public Models.UserModel GetUserByUsername(string username)
        {
            return SecurityRepository.GetUserbyUsername(username);
        }

        public int LoginUser(string username, string password)
        {
            return SecurityRepository.LoginUser(username, password);
        }

        public bool CanUserGetMerchantInfo(string username, string merchantId)
        {
            return SecurityRepository.CanUserGetMerchantInfo(username, merchantId);
        }

        #endregion Security Methods

        #region Ticket Methods

        public List<TicketsModel> GetTicket_Payment(long transactionNumber, string lang)
        {
            return TicketRepository.GetTicket_Payment(transactionNumber, lang);
        }

            public List<TicketModel> GetTicket_PaymentClaim(long claimNumber, string lang)
        {
            return TicketRepository.GetTicket_PaymentClaim(claimNumber, lang);
        }

        #endregion Ticket Methods

        #region Common Methods

        public List<Models.ChannelModel> GetAllChannels()
        {
            return CommonRepository.GetAllChannels();
        }

        public List<Models.ValidatorModel> GetAllValidators()
        {
            return CommonRepository.GetAllValidators();
        }

        public Models.ClientModel GetClienttByShortName(string shortName)
        {
            return CommonRepository.GetClienttByShortName(shortName);
        }

        public Models.ClientModel GetClienttById(int clientId)
        {
            return CommonRepository.GetClienttById(clientId);
        }

        public LanguageModel GetLanguageByCode(string code)
        {
            return CommonRepository.GetLanguageByCode(code);
        }

        public Models.CurrencyModel GetCurrencyByIso(string iso)
        {
            return CommonRepository.GetCurrencyByIso(iso);
        }

        public List<CurrencyModel> GetCurrencies()
        {
            return CommonRepository.GetCurrencies();
        }

        public DocTypeModel GetDocTypeByShortName(string shortName)
        {
            return CommonRepository.GetDocTypeByShortName(shortName);
        }

        public DocTypeModel GetDocTypeById(int docTypeId)
        {
            return CommonRepository.GetDocTypeById(docTypeId);
        }

        public bool CanUserGetTransaction(string username, string transactionId)
        {
            return CommonRepository.CanUserGetTransaction(username, transactionId);
        }

        public bool CanUserAccessToService(string username, string serviceName)
        {
            return CommonRepository.CanUserAccessToService(username, serviceName);
        }

        public List<Models.ClientModel> GetClients()
        {
            return CommonRepository.GetClients();
        }

        public List<Models.ServiceModel> GetServices()
        {
            return CommonRepository.GetServices();
        }

        public Models.ServiceConfigModel GetServiceConfigByServiceId(int id)
        {
            return CommonRepository.GetServiceConfigByServiceId(id);
        }

        public Models.ServiceModel GetServiceById(int id)
        {
            return CommonRepository.GetServiceById(id);
        }

        public Models.ServiceModel GetServiceByMerchantId(string merchantId)
        {
            return CommonRepository.GetServiceByMerchantId(merchantId);
        }

        public Models.ValidatorModel GetValidatorById(int validatorId)
        {
            return CommonRepository.GetValidatorById(validatorId);
        }

        public Models.ValidatorModel GetValidatorByCode(string validatorCode)
        {
            return CommonRepository.GetValidatorByCode(validatorCode);
        }

        public Models.ProductModel GetProductById(int productId)
        {
            return CommonRepository.GetProductById(productId);
        }

        public List<Models.ProductModel> GetAllProducts()
        {
            return CommonRepository.GetAllProducts();
        }

        public Models.ChannelModel GetChannelByName(string channelName)
        {
            return CommonRepository.GetChannelByName(channelName);
        }

        public void UpdateService(Models.ServiceModel service)
        {
            CommonRepository.UpdateService(service);
        }

        public void UpdateClient(Models.ClientModel client)
        {
            CommonRepository.UpdateClient(client);
        }

        public int? GetProductIdOfValidator(int productId, int validatorId)
        {
            return CommonRepository.GetProductIdOfValidator(productId, validatorId);
        }

        public string GetAppConfig(string key)
        {
            return CommonRepository.GetAppConfig(key);
        }

        public void InsertClient(Models.ClientModel client)
        {
            CommonRepository.InsertClient(client);
        }

        public List<Models.ConfigurationModel> GetConfigurations()
        {
            return CommonRepository.GetConfigurations();
        }

        public void InsertConfiguration(Models.ConfigurationModel configuration)
        {
            CommonRepository.InsertConfiguration(configuration);
        }

        public void RemoveConfiguration(int configurationId)
        {
            CommonRepository.RemoveConfiguration(configurationId);
        }

        #endregion Common Methods

        #region Configuration Methods

        public ValidatorModel GetValidatorFromConfiguration(int serviceId, int channelId, int productId)
        {
            return ConfigurationsRepository.GetValidatorFromConfiguration(serviceId, channelId, productId);
        }

        public string GetUniqueCode(int serviceId, int channelId, int productId)
        {
            return ConfigurationsRepository.GetUniqueCode(serviceId, channelId, productId);
        }

        #endregion Configuration Methods

        #region Payment Methods

        public void UpdateTransactionSyncByJob(long transactionNumber)
        {
            PaymentRepository.UpdateTransactionSyncByJob(transactionNumber);
        }

        public void UpdateTransactionCloseByTimeOut(long transactionIdPK)
        {
            PaymentRepository.UpdateTransactionCloseByTimeOut(transactionIdPK);
        }

        public List<TransactionValidatorNumber> GetNATransactions(DateTime desde, DateTime hasta)
        {
            return PaymentRepository.GetNATransactions(desde, hasta);
        }

        public CheckTransaction CheckIfTransactionIsComplete(string transactionId)
        {
            return PaymentRepository.CheckIfTransactionIsComplete(transactionId);
        }

        public void UpdateCommerceItemState(long id, int state)
        {
            PaymentRepository.UpdateCommerceItemState(id, state);
        }

        public CommerceItemModel GetCommerceItemById(long id)
        {
            return PaymentRepository.GetCommerceItemById(id);
        }

        public bool UpdateCommerceItemCode(string oldCode, string newCode, string TransactionId)
        {
            return PaymentRepository.UpdateCommerceItemCode(oldCode, newCode, TransactionId);
        }

        public bool IsTransactionInTRIByTransactionId(string transactionId)
        {
            return PaymentRepository.IsTransactionInTRIByTransactionId(transactionId);
        }

        public bool IsTransactionInTRIByTransactionIdPK(long transactionIdPK)
        {
            return PaymentRepository.IsTransactionInTRIByTransactionIdPK(transactionIdPK);
        }

        public int InsertTransactionResultInfo(TransactionResultInfoModel transactionResultInformation)
        {
            return PaymentRepository.InsertTransactionResultInfo(transactionResultInformation);
        }

        public int SaveTransaction(Models.TransactionModel transaction, IEnumerable<Models.CommerceItemModel> commerceItems)
        {
            return PaymentRepository.SaveTransaction(transaction, commerceItems);
        }

        public void UpdateTransactionId(long transactionIdPK, string transactionId)
        {
            PaymentRepository.UpdateTransactionId(transactionIdPK, transactionId);
        }

        public bool UpdateTransactionValidatorData(long transactionIdPK, string validatorTransactionId, string internalNumber)
        {
            return PaymentRepository.UpdateTransactionValidatorData(transactionIdPK, validatorTransactionId, internalNumber);
        }

        public Models.TransactionModel GetTransactionByTransactionId(string transactionId)
        {
            return PaymentRepository.GetTransactionByTransactionId(transactionId);
        }

        public TransactionModel GetTransactionById(long? transactionId)
        {
            return PaymentRepository.GetTransactionById(transactionId);
        }

        public long? GetTransactionPKByTransactionId(string transactionId)
        {
            return PaymentRepository.GetTransactionPKByTransactionId(transactionId);
        }

        public Models.TransactionCompletedInfo GetTransactionResult(long transactionIdPK, string language)
        {
            return PaymentRepository.GetTransactionResult(transactionIdPK, language);
        }

        public List<Models.TransactionModel> GetTransactionsByElectronicPaymentCodeAndMerchantId(string electronicPaymentCode, string merchantId)
        {
            return PaymentRepository.GetTransactionsByElectronicPaymentCodeAndMerchantId(electronicPaymentCode, merchantId);
        }

        public bool isDuplicatedElectronicPaymentCode(string electronicPaymentCode, int serviceId)
        {
            return PaymentRepository.isDuplicatedElectronicPaymentCode(electronicPaymentCode, serviceId);
        }

        public void UpdateTransaction(Models.TransactionModel transaction)
        {
            PaymentRepository.UpdateTransaction(transaction);
        }

        public List<Models.ProductModel> GetProductsByService(int serviceId)
        {
            return CommonRepository.GetProductsByService(serviceId);
        }

        public long InsertPaymentValidatorComm(Models.PaymentValidatorCommModel paymentValidatorComm)
        {
            return PaymentRepository.InsertPaymentValidatorComm(paymentValidatorComm);
        }

        public void UpdatePaymentValidatorComm(Models.PaymentValidatorCommModel paymentValidatorComm)
        {
            PaymentRepository.UpdatePaymentValidatorComm(paymentValidatorComm);
        }

        public void UpdatePaymentValidatorCommByIdTransaction(Models.PaymentValidatorCommModel paymentValidatorComm)
        {
            PaymentRepository.UpdatePaymentValidatorCommByIdTransaction(paymentValidatorComm);
        }

        public Models.CallbackModel GetCallback(long transactionIdPK)
        {
            return PaymentRepository.GetCallback(transactionIdPK);
        }

        public Models.ServiceModel GetServiceByTransactionId(long transactionIdPK)
        {
            return PaymentRepository.GetServiceByTransactionId(transactionIdPK);
        }

        public Models.TransactionModel GetTransactionByValidatorTransactionId(string validatorTransactionId, int validatorId)
        {
            return PaymentRepository.GetTransactionByValidatorTransactionId(validatorTransactionId, validatorId);
        }

        //public bool IsValidTransaction(string serviceName, string electronicPaymentCode, string amount, string productId, string payments, string hashedTransaction)
        //{
        //    try
        //    {
        //        using (var context = new EF.PGDataEntities())
        //        {
        //            string inputParameters = string.Concat("serviceName=", serviceName, ",electronicPaymentCode=",
        //                                                   electronicPaymentCode, ",amount=", amount, ",productId=",
        //                                                   productId, ",payments=", payments, ",hashedTransaction=",
        //                                                   hashedTransaction);
        //            Log.InsertLog(LogTypeModel.Info, "DataAccesService.IsValidTransaction",
        //                          string.Concat("Parametros: ", inputParameters), "");

        //            string originalServiceName = serviceName;
        //            serviceName = GetServiceNameByMerchantId(serviceName);
        //            //FGS: Si en la var "ServiceName" llega un merchantId, se mapea al serviceName, caso contrario, queda el valor por defecto.
        //            var serviceKey = from s in context.Services
        //                             join hk in context.HashKeys on s.Id equals hk.ServiceId
        //                             where s.Name == serviceName && s.IsActive && hk.IsActive
        //                             select hk.HashCode;
        //            string strToHash = string.Concat("MERCHID:", originalServiceName, "|", "EPC:", electronicPaymentCode,
        //                                             "|", "MONTO:", amount, "|", "IDPRODUCTO:", productId, "|",
        //                                             "CUOTAS:", payments, "|", "HKEY:", serviceKey);
        //            string strHashed = CustomTool.HashCode(strToHash);
        //            return (strHashed == hashedTransaction);
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Log.InsertLog(LogTypeModel.Info, "DataAccesService.IsValidTransaction", "", "Null Object");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertLog(LogTypeModel.Error, "DataAccesService.IsValidTransaction", "", ex.Message);
        //        throw new Exception("Ocurrió un error interno", ex.InnerException);
        //    }
        //}



        #endregion Payment Methods

        #region Notification Methods

        public long? SendNotification(long transactionIdPk, string statusCode, string moduleDescription, int serviceId, Dictionary<string, string> ticketModel, string finalUserMails, DateTime paymentDate, bool sentBySync = false)
        {
            return NotificationRepository.SendNotification(transactionIdPk, statusCode, moduleDescription, serviceId, ticketModel, finalUserMails, paymentDate, sentBySync);
        }

        public int InsertNotificationLog(Models.NotificationLogModel notificationLog)
        {
            return NotificationRepository.InsertNotificationLog(notificationLog);
        }

        //public long InsertTicketLog(Models.TicketLogModel ticketLog)
        //{
        //    // TODO: mryll - refactor
        //    //return NotificationRepository.InsertTicketLog(ticketLog);
        //    return 0;
        //}

        public int? InsertNotificationConfig(string serviceName, string statusCode, string templateName)
        {
            return NotificationRepository.InsertNotificationConfig(serviceName, statusCode, templateName);
        }

        public int? InsertStatusTemplate(string statusCode, string templateName)
        {
            return NotificationRepository.InsertStatusTemplate(statusCode, templateName);
        }

        #endregion Notification Methods

        #region Claim Methods

        public bool CanUserWorkWithPaymentClaimByPaymentClaimNumber(string user, long claimnumber)
        {
            return PaymentClaimRepository.CanUserWorkWithPaymentClaimByPaymentClaimNumber(user, claimnumber);
        }


        public AnnulmentResultInfoModel GetAnnulmentResultInfoByPaymentClaimId(long paymentClaimId)
        {
            return PaymentClaimRepository.GetAnnulmentResultInfoByPaymentClaimId(paymentClaimId);
        }

        public void InsertAnnulmentResultInfo(AnnulmentResultInfoModel ARIToSave)
        {
            PaymentClaimRepository.InsertAnnulmentResultInfo(ARIToSave);
        }

        public bool IsPaymentClaimLocked(long paymentClaimId)
        {
            return PaymentClaimRepository.IsPaymentClaimLocked(paymentClaimId);
        }

        public bool UnLockPaymentClaim(long paymentClaimId)
        {
            return PaymentClaimRepository.LockUnlockPaymentClaim(paymentClaimId, false);
        }

        public bool LockPaymentClaim(long paymentClaimId)
        {
            return PaymentClaimRepository.LockUnlockPaymentClaim(paymentClaimId, true);
        }

        public long? InsertPaymentClaim(PaymentClaimModel paymentClaim, ClaimerModel claimer, string moduleCode, string user)
        {
            return PaymentClaimRepository.InsertPaymentClaim(paymentClaim, claimer, moduleCode, user);
        }

        public PaymentClaimModel GetActivePaymentClaimByTransactionId(string transactionId)
        {
            return GetActivePaymentClaimByTransactionId(transactionId);
        }

        public List<PaymentClaimModel> GetPaymentClaimsByTransactionId(string transactionId)
        {
            return PaymentClaimRepository.GetPaymentClaimsByTransactionId(transactionId);
        }

        public long? GetPaymentClaimIdByNumber(long paymentClaimNumber)
        {
            return PaymentClaimRepository.GetPaymentClaimIdByNumber(paymentClaimNumber);
        }

        public PaymentClaimModel GetPaymentClaimById(long paymentClaimId)
        {
            return PaymentClaimRepository.GetPaymentClaimById(paymentClaimId);
        }

        public List<PaymentClaimModel> GetAllPaymentClaimByStatusCode(string statusCode, bool isAutomitized)
        {
            return PaymentClaimRepository.GetAllPaymentClaimByStatusCode(statusCode, isAutomitized);
        }

        public int InsertAnnulmentRequest(AnnulmentRequestModel annulmentRequest)
        {
            return PaymentClaimRepository.InsertAnnulmentRequest(annulmentRequest);
        }

        public void UpdateAnnulmentRequest(AnnulmentRequestModel annulmentRequest)
        {
            PaymentClaimRepository.UpdateAnnulmentRequest(annulmentRequest);
        }

        public int? InsertAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel)
        {
            return PaymentClaimRepository.InsertAnnulmentValidatorComm(annulmentValidatorCommModel);
        }

        public void UpdateAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel)
        {
            PaymentClaimRepository.UpdateAnnulmentValidatorComm(annulmentValidatorCommModel);
        }

        public Models.AnnulmentValidatorCommModel GetAnnulmentValidatorCommById(int annulmentValidatorCommId)
        {
            return PaymentClaimRepository.GetAnnulmentValidatorCommById(annulmentValidatorCommId);
        }

        public ClaimerModel GetClaimerById(int claimerId)
        {
            return PaymentClaimRepository.GetClaimerById(claimerId);
        }

        public List<Models.StatusTicketModel> GetTicketsOfPaymentClaim(long paymentClaimNumber)
        {
            return PaymentClaimRepository.GetTicketsOfPaymentClaim(paymentClaimNumber);
        }

        public List<PaymentClaimModel> GetPaymentClaimsByCriteria(FilterCriteriaModel criteria)
        {
            return PaymentClaimRepository.GetPaymentClaimsByCriteria(criteria);
        }

        //FGS2
        public void GetCommerceItemsClaimedByTransactionIdPK(long transactionIdPK)
        {
            PaymentClaimRepository.GetCommerceItemsClaimedByTransactionIdPK(transactionIdPK);
        }

        //FGS2
        public bool ExistClaimForCommerceItem(Models.CommerceItemModel commerceItem)
        {
            return PaymentClaimRepository.ExistClaimForCommerceItem(commerceItem);
        }

        //FGS2
        public List<CommerceItemModel> GetCommerceItemNotAnnulledByTransactionIdPK(long transactionIdPK)
        {
            return PaymentClaimRepository.GetCommerceItemNotAnnulledByTransactionIdPK(transactionIdPK);
        }

        //ALT2
        public List<CommerceItemModel> GetCommerceItemsToClaimByTransactionIdPK(long id)
        {
            return PaymentClaimRepository.GetCommerceItemsToClaimByTransactionIdPK(id);
        }

        //FGS2
        public List<CommerceItemModel> GetCommerceItemByPaymentClaimId(long paymentClaimId)
        {
            return PaymentClaimRepository.GetCommerceItemByPaymentClaimId(paymentClaimId);
        }

        #endregion Claim Methods

        #region Status Methods

        public HTTPResponse_Model GetHTTPResponseByStatusCodeOrPGCode(string StatusCodeOrPGCode, string language) {
            return StatusRepository.GetHTTPResponseByStatusCodeOrPGCode(StatusCodeOrPGCode, language);
        }

        public string GetValidationResponseByStatusCode(string StatusCode, string language)
        {
            return StatusRepository.GetValidationResponseByStatusCode(StatusCode, language);
        }


        //# FEATURE 523 - Get Transaction Result by any validator
        //public GetTransaction_Result GetTransactionResultByTransactionId(string transactionId)
        //{
        //    return StatusRepository.GetTransactionResult(transactionId);
        //}

        //# FEATURE 523 - Get Transaction Result by any validator
        //public GetTransaction_Result GetTransactionResultByTransactionIdPk(long? transactionId)
        //{
        //    var trans = PaymentRepository.GetTransactionById(transactionId);

        //    return StatusRepository.GetTransactionResult(trans.TransactionId);
        //}

        public string GetModuleCodeOfTransaction(string transactionId)
        {
            return StatusRepository.GetModuleCodeOfTransaction(transactionId);
        }

        public StatusResponseMessageModel GetStatusMessageByOriginalCode(string moduleType, string originalCode, string languageCodeISO6391)
        {
            return StatusRepository.GetStatusMessageByOriginalCode(moduleType, originalCode, languageCodeISO6391);
        }

        public string GetModuleCodeByStatusCode(string moduleType, string statusCode, int? moduleValidator = null)
        {
            return StatusRepository.GetModuleCodeByStatusCode(moduleType, statusCode, moduleValidator);
        }

        public string GetStatusCodeByOriginalCode(string originalCode, string moduleType, int? validator)
        {
            return StatusRepository.GetStatusCodeByOriginalCode(originalCode, moduleType, validator);
        }

        public string MapOriginalCodeFromValidatorToPGCode(string originalCode, string moduleTypeFrom, string moduleTypeTo, int? validatorFrom = null) { 
            return StatusRepository.MapOriginalCodeFromValidatorToPGCode(originalCode,  moduleTypeFrom,  moduleTypeTo, validatorFrom);
        }
      

        public Models.StatusCodeModel GetStatusCodeByCode(string code)
        {
            return StatusRepository.GetStatusCodeByCode(code);
        }

        public void UpdatePaymentClaimStatus(long paymentClaimId, string user, string moduleCode, string observations)
        {
            Models.PaymentClaimStatusModel paymentClaimStatus = StatusRepository.UpdatePaymentClaimStatus(paymentClaimId, user, moduleCode, observations);
        }

        public void SetTicketToPaymentClaimStatus(long paymentClaimNumber, string statusCode, int ticketLogId)
        {
            StatusRepository.SetTicketToPaymentClaimStatus(paymentClaimNumber, statusCode, ticketLogId);
        }

        public void UpdateTransactionStatus(long transactionIdPk, string originalCode, string typeMethod, int? validator = null)
        {
            StatusRepository.UpdateTransactionStatus(transactionIdPk, originalCode, typeMethod, validator);
        }

        public Models.StatusCodeModel GetStatusCodeOfPaymentClaim(long paymentClaimId)
        {
            return StatusRepository.GetStatusCodeOfPaymentClaim(paymentClaimId);
        }

        public string GetModuleCodeOfPaymentClaim(long paymentClaimId)
        {
            return StatusRepository.GetModuleCodeOfPaymentClaim(paymentClaimId);
        }

        public StatusResponseMessageModel GetStatusMessageByStatusCode(string statusCode, string languageCode)
        {
            return StatusRepository.GetStatusMessageByStatusCode(statusCode, languageCode);
        }

        public Models.StatusMessageModel GetStatusMessageByPaymentClaim(long paymentClaimNumber)
        {
            return (StatusRepository.GetStatusMessageByPaymentClaim(paymentClaimNumber));
        }

        public StatusCodeStatusMessageModel GetLastPaymentClaimStatus(long paymentClaimNumber, string languageCode)
        {
            try
            {
                return StatusRepository.GetLastPaymentClaimStatus(paymentClaimNumber, languageCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StatusCodeStatusMessageModel> GetPaymentClaimStatusHistory(long paymentClaimNumber, string languageCode)
        {
            try
            {
                return StatusRepository.GetPaymentClaimStatusHistory(paymentClaimNumber, languageCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Models.GenericCodeModel GetGenericCodeByStatusCode(string statusCode)
        {
            try
            {
                return StatusRepository.GetGenericCodeByStatusCode(statusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Status Methods

        #region Simulation

        //public void SentAnnulmentTicket(int channelId, int claimId, int serviceId, string finalUser)
        //{
        //    using (var context = new EF.PGDataEntities())
        //    {
        //        Models.ChannelModel channel = Mapper.Channel_EFToModel(context.Channels.FirstOrDefault(c => c.ChannelId == channelId));
        //        Models.PaymentClaimModel claim = Mapper.PaymentClaim_EFToModel(context.PaymentClaim.FirstOrDefault(c => c.PaymentClaimId == claimId));
        //        EF.PaymentClaimStatus claimStatus = context.PaymentClaimStatus.FirstOrDefault(
        //                pcs => pcs.PaymentClaimId == claim.PaymentClaimId && pcs.IsActual);
        //        SendNotification(Mapper.StatusCode_EFToModel(claimStatus.StatusCode).Code, "claim",
        //                         GetServiceByID(serviceId).Name, ToDictionary(claim), finalUser);
        //    }
        //}

        #endregion Simulation

        #region Old Methods

        //public ServiceModel GetServiceByUniqueCode(string uniqueCode)
        //{
        //    try
        //    {
        //        using (var context = new EF.PGDataEntities())
        //        {
        //            string inputParameters = string.Format("uniqueCode = {0}", uniqueCode);
        //            Log.InsertLog(LogTypeModel.Info, "DataAccesService.GetServiceByUniqueCode",
        //                          string.Format("Parametros: {0} ", inputParameters));

        //            var serviceId =
        //                context.ServiceProductsValidators.FirstOrDefault(s => s.UniqueCode == uniqueCode).ServiceId;

        //            var service = this.GetServiceByID(serviceId);

        //            return service;
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Log.InsertLog(LogTypeModel.Info, "DataAccesService.GetServiceByUniqueCode", "", "Null object");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertLog(LogTypeModel.Error, "DataAccesService.GetServiceByUniqueCode", "", ex.Message);
        //        throw new Exception("Ocurrió un error interno", ex.InnerException);
        //    }
        //}

        //public int GetValidatorProductIdWithValidatorId(int productId, int validatorId)
        //{
        //    try
        //    {
        //        using (var context = new EF.PGDataEntities())
        //        {
        //            string inputParameters = string.Concat("productId=", productId, ",validatorId=", validatorId);
        //            Log.InsertLog(LogTypeModel.Info, "DataAccesService.GetValidatorProductIdWithValidatorId",
        //                          string.Concat("Parametros: ", inputParameters), "");

        //            int validatorProductId;

        //            string validatorName = context.Validators.FirstOrDefault(v => v.ValidatorId == validatorId).Name;
        //            var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
        //            switch (validatorName)
        //            {
        //                case "NPS":
        //                    validatorProductId = int.Parse(product.NPS.ToString());
        //                    break;

        //                case "SPS":
        //                    validatorProductId = int.Parse(product.SPS.ToString());
        //                    break;

        //                case "HSR":
        //                    validatorProductId = int.Parse(product.HSR.ToString());
        //                    break;

        //                default:
        //                    validatorProductId = 0;
        //                    break;
        //            }
        //            return validatorProductId;
        //        }
        //    }
        //    catch (NullReferenceException)
        //    {
        //        Log.InsertLog(LogTypeModel.Info, "DataAccesService.GetValidatorProductIdWithValidatorId", "", "Null Object");
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertLog(LogTypeModel.Error, "DataAccesService.GetValidatorProductIdWithValidatorId", "", ex.Message);
        //        throw new Exception("Ocurrió un error interno", ex.InnerException);
        //    }
        //}



        #endregion Old Methods
    }
}