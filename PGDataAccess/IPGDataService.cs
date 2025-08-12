using PGDataAccess.EF;
using PGDataAccess.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PGDataAccess
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPGDataService" in both code and config file together.
    [ServiceContract]
    public interface IPGDataService
    {
        [OperationContract]
        bool CanUserWorkWithPaymentClaimByPaymentClaimNumber(string user, long claimnumber);


        [OperationContract]
        List<TicketsModel> GetTicket_Payment(long transactionNumber, string lang);

        [OperationContract]
        string MapOriginalCodeFromValidatorToPGCode(string originalCode, string moduleTypeFrom, string moduleTypeTo, int? validatorFrom = null);
       

        [OperationContract]
        string GetValidationResponseByStatusCode(string StatusCode, string language);

        [OperationContract]
        bool CanUserAccessToService(string username, string serviceName);

        [OperationContract]
        bool CanUserGetMerchantInfo(string username, string merchantId);

        [OperationContract]
        bool CanUserGetTransaction(string username, string transactionId);

        [OperationContract]
        PaymentClaimModel GetActivePaymentClaimByTransactionId(string transactionId);

        [OperationContract]
        HTTPResponse_Model GetHTTPResponseByStatusCodeOrPGCode(string StatusCodeOrPGCode, string language);

        [OperationContract]
        Models.AnnulmentResultInfoModel GetAnnulmentResultInfoByPaymentClaimId(long paymentClaimId);

        [OperationContract]
        Models.AnnulmentValidatorCommModel GetAnnulmentValidatorCommById(int annulmentValidatorCommId);

        [OperationContract]
        string GetAppConfiguration(string setting);

        [OperationContract]
        void InsertAnnulmentResultInfo(AnnulmentResultInfoModel ARIToSave);

        [OperationContract]
        List<Models.ChannelModel> GetAllChannels();

        [OperationContract]
        List<PaymentClaimModel> GetAllPaymentClaimByStatusCode(string statusCode, bool isAutomitized);

        [OperationContract]
        List<ProductModel> GetAllProducts();

        [OperationContract]
        List<Models.ValidatorModel> GetAllValidators();

        [OperationContract]
        string GetAppConfig(string key);

        [OperationContract]
        Models.CallbackModel GetCallback(long transactionIdPK);

        [OperationContract]
        ChannelModel GetChannelByName(string channelName);

        [OperationContract]
        ClaimerModel GetClaimerById(int claimerId);

        [OperationContract]
        List<Models.ClientModel> GetClients();

        [OperationContract]
        ClientModel GetClienttById(int clientId);

        [OperationContract]
        ClientModel GetClienttByShortName(string shortName);

        [OperationContract]
        List<Models.ConfigurationModel> GetConfigurations();
        
        [OperationContract]
        List<CurrencyModel> GetCurrencies();

        [OperationContract]
        CurrencyModel GetCurrencyByIso(string iso);

        [OperationContract]
        DocTypeModel GetDocTypeByShortName(string shortName);

        [OperationContract]
        DocTypeModel GetDocTypeById(int shortName);

        [OperationContract]
        Models.GenericCodeModel GetGenericCodeByStatusCode(string statusCode);

        [OperationContract]
        StatusCodeStatusMessageModel GetLastPaymentClaimStatus(long paymentClaimNumber, string languageCode);

        [OperationContract]
        List<string> GetLastPayment();

        [OperationContract]
        string GetModuleCodeByStatusCode(string moduleType, string StatusCode, int? moduleValidator = null);

        [OperationContract]
        string GetModuleCodeOfPaymentClaim(long paymentClaimId);

        [OperationContract]
        string GetModuleCodeOfTransaction(string transactionId);

        [OperationContract]
        List<PaymentClaimModel> GetPaymentClaimsByCriteria(FilterCriteriaModel criteria);

        [OperationContract]
        long? GetPaymentClaimIdByNumber(long paymentClaimNumber);

        [OperationContract]
        PaymentClaimModel GetPaymentClaimById(long paymentClaimId);

        [OperationContract]
        List<PaymentClaimModel> GetPaymentClaimsByTransactionId(string transactionId);

        [OperationContract]
        List<StatusCodeStatusMessageModel> GetPaymentClaimStatusHistory(long paymentClaimNumber, string languageCode);

        [OperationContract]
        ProductModel GetProductById(int productId);

        [OperationContract]
        int? GetProductIdOfValidator(int productId, int validatorId);

        [OperationContract]
        List<Models.ProductModel> GetProductsByService(int serviceId);


        [OperationContract]
        List<ReporteRenditionModel> GetReportRendition(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta);


        [OperationContract]
        List<ReporteCentralizadorModel> GetReport814(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta);

        [OperationContract]
        List<ReporteCentralizadorOrRefundionRefundModel> GetReportRenditionRefunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta);


        [OperationContract]
        List<ReporteCentralizadorOrRefundionRefundModel> GetReport814Refunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta);



        [OperationContract]
        List<ReporteConciliacionModel> GetReportConciliation(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta);

        [OperationContract]
        List<RolModel> GetRolesByUsername(string username);

        [OperationContract]
        ServiceModel GetServiceById(int id);

        [OperationContract]
        ServiceModel GetServiceByMerchantId(string merchantId);

        [OperationContract]
        Models.ServiceModel GetServiceByTransactionId(long transactionIdPK);

        [OperationContract]
        List<ServiceModel> GetServices();

        [OperationContract]
        AppInfo GetStatus();

        [OperationContract]
        string GetStatusCodeByOriginalCode(string originalCode, string moduleType, int? validator);

        [OperationContract]
        StatusCodeModel GetStatusCodeOfPaymentClaim(long paymentClaimId);

        [OperationContract]
        StatusMessageModel GetStatusMessageByPaymentClaim(long paymentClaimId);

        [OperationContract]
        StatusResponseMessageModel GetStatusMessageByOriginalCode(string moduleType, string paymentGatewayCode, string languageCodeISO6391);

        [OperationContract]
        StatusResponseMessageModel GetStatusMessageByStatusCode(string statusCode, string languageCode);

        [OperationContract]
        List<TicketModel> GetTicket_PaymentClaim(long claimNumber, string lang);


        [OperationContract]
        TransactionModel GetTransactionById(long? idTransaction);

        [OperationContract]
        List<TransactionModel> GetTransactionsByElectronicPaymentCodeAndMerchantId(string electronicPaymentCode, string serviceId);

        [OperationContract]
        bool isDuplicatedElectronicPaymentCode(string electronicPaymentCode, int serviceId);

        [OperationContract]
        TransactionModel GetTransactionByTransactionId(string transactionId);

        [OperationContract]
        long? GetTransactionPKByTransactionId(string transactionId);

        [OperationContract]
        Models.TransactionCompletedInfo GetTransactionResult(long transactionIdPK, string language);

        [OperationContract]
        List<RePrintModel> GetTicketInformationToRePrint(DateTime PurchaseDate, int ExternalId, string CreditCard4LastDigits, string AuthorizationCode);

        [OperationContract]
        string GetUniqueCode(int serviceId, int channelId, int productId);

        [OperationContract]
        UserModel GetUser(int userId);

        [OperationContract]
        UserModel GetUserByUsername(string username);

        [OperationContract]
        ValidatorModel GetValidatorById(int validatorId);

        [OperationContract]
        ValidatorServiceConfigModel GetValidatorConfigByServiceId(int serviceId, int validatorId);

        [OperationContract]
        ValidatorModel GetValidatorFromConfiguration(int serviceId, int channelId, int productId);

        [OperationContract]
        int InsertAnnulmentRequest(AnnulmentRequestModel annulmentRequest);

        [OperationContract]
        int? InsertAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel);

        [OperationContract]
        void InsertConfiguration(Models.ConfigurationModel configuration);

        [OperationContract]
        long SaveLog(LogModel log);

        [OperationContract]
        int? InsertNotificationConfig(string serviceName, string statusCode, string templateName);

        [OperationContract]
        int InsertNotificationLog(NotificationLogModel notificationLog);

        [OperationContract]
        long? InsertPaymentClaim(PaymentClaimModel paymentClaim, ClaimerModel claimer, string moduleCode, string currentUser);

        //[OperationContract]
        //long InsertTicketLog(TicketLogModel ticketLog);

        [OperationContract]
        int InsertTransactionResultInfo(TransactionResultInfoModel transactionResultInformation);

        [OperationContract]
        bool IsPaymentClaimLocked(long paymentClaimId);

        [OperationContract]
        bool IsTransactionInTRIByTransactionId(string transactionId);

        [OperationContract]
        bool IsTransactionInTRIByTransactionIdPK(long transactionIdPK);

        [OperationContract]
        bool LockPaymentClaim(long paymentClaimId);

        [OperationContract]
        long InsertPaymentValidatorComm(Models.PaymentValidatorCommModel paymentValidatorComm);

        [OperationContract]
        int LoginUser(string username, string password);

        [OperationContract]
        void RemoveConfiguration(int configurationId);

        [OperationContract]
        int SaveTransaction(TransactionModel transaction, IEnumerable<CommerceItemModel> commerceItems);

        [OperationContract]
        long? SendNotification(long transactionIdPk, string statusCode, string moduleDescription, int serviceId, Dictionary<string, string> ticketModel, string finalUserMails, DateTime SendNotification, bool sentBySync);

        [OperationContract]
        void SendEmail(MailerModel basicMail);

        [OperationContract]
        void SetAppConfiguration(string setting, string newvalue);



        [OperationContract]
        void SetTicketToPaymentClaimStatus(long paymentClaimNumber, string statusCode, int ticketNumber);

        [OperationContract]
        string TestDAS();

        [OperationContract]
        string TestDBDelete();

        [OperationContract]
        string TestDBInsert();

        [OperationContract]
        string TestDBSelect();

        [OperationContract]
        string TestSMTP();

        [OperationContract]
        string TestDBUpdate();

        [OperationContract]
        bool UnLockPaymentClaim(long paymentClaimId);

        [OperationContract]
        void UpdateAnnulmentRequest(AnnulmentRequestModel annulmentRequest);

        [OperationContract]
        void UpdateAnnulmentValidatorComm(AnnulmentValidatorCommModel annulmentValidatorCommModel);

        [OperationContract]
        void UpdateClient(Models.ClientModel client);

        [OperationContract]
        void UpdatePaymentClaimStatus(long paymentClaimId, string user, string moduleCode, string observations);

        [OperationContract]
        void UpdatePaymentValidatorComm(Models.PaymentValidatorCommModel paymentValidatorComm);

        [OperationContract]
        void UpdatePaymentValidatorCommByIdTransaction(Models.PaymentValidatorCommModel paymentValidatorComm);

        [OperationContract]
        void UpdateService(ServiceModel service);

        [OperationContract]
        void UpdateTransaction(TransactionModel transaction);

        [OperationContract]
        void UpdateTransactionId(long transactionIdPK, string transactionId);

        [OperationContract]
        void UpdateTransactionStatus(long transactionIdPK, string originalCode, string typeMethod, int? validator = null);

        //TODO Ordenar alfabeticamente. Nuevos metodos para v4.1
        [OperationContract]
        void UpdateJobRunLog(JobLogModel jobToUpdate);
        [OperationContract]
        int InsertJobRunLog(JobLogModel jobToInsert);
        [OperationContract]
        bool IsPaymentOffline();
        [OperationContract]
        bool IsServiceOffline();
        [OperationContract]
        bool IsJobConsoleTaskOffline(string task);
        [OperationContract]
        List<AppConfigModel> GetAllConfigs();
        [OperationContract]
        void InsertReportJobRunLog(List<long> CommerceItemId, int JobRunLog, string ReportType);

        [OperationContract]
        void InsertRefundJobRunLog(List<long> CommerceItemId, int JobRunLog);
        [OperationContract]
        bool UpdateCommerceItemCode(string oldCode, string newCode, string TransactionId);

        [OperationContract]
        Models.ServiceConfigModel GetServiceConfigByServiceId(int id);

        [OperationContract]
        Models.ValidatorModel GetValidatorByCode(string validatorCode);

        [OperationContract]
        CommerceItemModel GetCommerceItemById(long id);

        [OperationContract]
        void UpdateCommerceItemState(long id, int state);

        [OperationContract]
        List<CommerceItemModel> GetCommerceItemsToClaimByTransactionIdPK(long id);

        //FGS2
        [OperationContract]
        void GetCommerceItemsClaimedByTransactionIdPK(long transactionIdPK);

        //FGS2
        [OperationContract]
        bool ExistClaimForCommerceItem(Models.CommerceItemModel commerceItem);

        //FGS2
        [OperationContract]
        List<CommerceItemModel> GetCommerceItemNotAnnulledByTransactionIdPK(long transactionIdPK);

        //FGS2
        [OperationContract]
        void InsertClient(Models.ClientModel client);

        [OperationContract]
        List<ServiceModel> GetServicesByUser(int userId);

        //FGS2
        [OperationContract]
        List<CommerceItemModel> GetCommerceItemByPaymentClaimId(long paymentClaimId);

        [OperationContract]
        CheckTransaction CheckIfTransactionIsComplete(string transactionId);

        [OperationContract]
        LanguageModel GetLanguageByCode(string code);

        [OperationContract]
        Models.TransactionModel GetTransactionByValidatorTransactionId(string validatorTransactionId, int validatorId);

        [OperationContract]
        bool UpdateTransactionValidatorData(long transactionIdPK, string validatorTransactionId, string internalNumber);

        [OperationContract]
        string GetAppVersion(string appVersion);

        [OperationContract]
        string SaveNewBuild(string tfschange);

        [OperationContract]
        List<TransactionValidatorNumber> GetNATransactions(DateTime desde, DateTime hasta);

        [OperationContract]
        void UpdateTransactionSyncByJob(long transactionNumber);

        [OperationContract]
        void UpdateTransactionCloseByTimeOut(long transactionIdPK);

        [OperationContract]
        List<ReporteRenditionExportModel> GetRenditionInformation();


    }
}