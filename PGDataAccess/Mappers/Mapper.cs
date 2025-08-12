using System;
using System.Collections.Generic;
using System.Linq;
using PGDataAccess.Tools;
using PGDataAccess.Repository;
using PGDataAccess.Models;
namespace PGDataAccess.Mappers
{
    static public class Mapper
    {

        static public RolModel Rol_EFToModel(EF.Rol rolToMap)
        {
            return new RolModel()
            {
                Id = rolToMap.Id,
                shortName = rolToMap.shortName,
                Description = rolToMap.Description,
                IsActive = rolToMap.IsActive,
            };
        }

        static public UserModel User_EFToModel(EF.User userToMap)
        {
            return new UserModel
            {
                Id = userToMap.Id,
                username = userToMap.username,
                IsActive = userToMap.IsActive,
                email = userToMap.email
            };
        }

        static public ValidatorServiceConfigModel ValidatorServiceConfig_EFToModel(EF.ValidatorServiceConfig vsconfigToMap)
        {
            return new ValidatorServiceConfigModel
            {
                HashKey = vsconfigToMap.HashKey,
                ServiceId = vsconfigToMap.ServiceId,
                ValidatorId = vsconfigToMap.ValidatorId,
                ValidatorPass = vsconfigToMap.ValidatorPass,
                ValidatorServiceConfigId = vsconfigToMap.ValidatorServiceConfigId,
                ValidatorUser = vsconfigToMap.ValidatorUser
            };
        }

        static public EF.Transactions Transaction_ModelToEF(TransactionModel transaction)
        {
            EF.Transactions transactionEntity = new EF.Transactions();
            transactionEntity.Id = transaction.Id;
            transactionEntity.InternalNbr = transaction.InternalNbr;
            transactionEntity.Channel = transaction.Channel;
            transactionEntity.Client = transaction.Client;
            transactionEntity.SalePoint = transaction.SalePoint;
            transactionEntity.Service = transaction.Service;
            transactionEntity.Product = transaction.Product;
            transactionEntity.Amount = transaction.Amount;
            transactionEntity.Validator = transaction.Validator;
            transactionEntity.WebSvcMethod = transaction.WebSvcMethod;
            transactionEntity.TransactionId = transaction.ValidatorTransactionId; //CHANGE 4.1 TransactionNumber PBI 2528
            transactionEntity.JSonObject = transaction.JSonObject;
            transactionEntity.CreatedOn = transaction.CreatedOn;
            transactionEntity.SettingId = transaction.SettingId;
            transactionEntity.MerchantId = transaction.MerchantId;
            transactionEntity.ElectronicPaymentCode = transaction.ElectronicPaymentCode;
            transactionEntity.CurrencyCode = transaction.CurrencyCode;
            transactionEntity.TrxCurrencyCode = transaction.TrxCurrencyCode;
            transactionEntity.TrxAmount = transaction.TrxAmount;
            transactionEntity.ConvertionRate = transaction.ConvertionRate;
            return transactionEntity;
        }

        static public EF.Transactions Transaction_ModelToEF(EF.Transactions originalTransaction, TransactionModel transaction)
        {

            originalTransaction.InternalNbr = transaction.InternalNbr;
            originalTransaction.Channel = transaction.Channel;
            originalTransaction.Client = transaction.Client;
            originalTransaction.SalePoint = transaction.SalePoint;
            originalTransaction.Service = transaction.Service;
            originalTransaction.Product = transaction.Product;
            originalTransaction.Amount = transaction.Amount;
            originalTransaction.Validator = transaction.Validator;
            originalTransaction.WebSvcMethod = transaction.WebSvcMethod;
            originalTransaction.TransactionId = transaction.ValidatorTransactionId; //CHANGE 4.1 TransactionNumber PBI 2528
            originalTransaction.JSonObject = transaction.JSonObject;
            originalTransaction.CreatedOn = transaction.CreatedOn;
            originalTransaction.SettingId = transaction.SettingId;
            originalTransaction.MerchantId = transaction.MerchantId;
            originalTransaction.ElectronicPaymentCode = transaction.ElectronicPaymentCode;
            originalTransaction.CurrencyCode = transaction.CurrencyCode;
            originalTransaction.TrxCurrencyCode = transaction.TrxCurrencyCode;
            originalTransaction.TrxAmount = transaction.TrxAmount;
            originalTransaction.ConvertionRate = transaction.ConvertionRate;

            return originalTransaction;
        }

        static public TransactionModel Transaction_EFToModel(EF.Transactions transactionEntity)
        {
            TransactionModel transactionMapped = new TransactionModel();

            // PG 3.5
            // Add TransactionAdditionalInfo to Transaction Model
            EF.TransactionAdditionalInfo AdditionalInfo = new EF.TransactionAdditionalInfo();
            AdditionalInfo = GetTransactionAdditionalInfoById(transactionEntity.Id);
            if (AdditionalInfo != null)
            {
                decimal? nullabledecimal = AdditionalInfo.CurrentAmount;
                decimal currentAmount = nullabledecimal ?? 0;
                transactionMapped.ProdVersionUsed = AdditionalInfo.ProdVersionUsed;
                transactionMapped.CurrentAmount = currentAmount;
                transactionMapped.CustomerMail = AdditionalInfo.CustomerMail;
                transactionMapped.AppVersion = AdditionalInfo.VersionUsed;
                transactionMapped.ClientId = AdditionalInfo.ClientId;
                transactionMapped.ProductId = AdditionalInfo.ProductId;
                transactionMapped.ServiceId = AdditionalInfo.ServiceId;
                transactionMapped.ChannelId = AdditionalInfo.ChannelId;
                transactionMapped.ExternalAppId = AdditionalInfo.ExternalApp ?? 0;
                transactionMapped.ValidatorId = AdditionalInfo.ValidatorId;
                transactionMapped.LanguageId = AdditionalInfo.LanguageId;
                transactionMapped.BarCode = AdditionalInfo.BarCode;
                transactionMapped.CallbackUrl = AdditionalInfo.CallbackUrl;
                transactionMapped.Payments = AdditionalInfo.Payments;
                transactionMapped.IsEPCValidated = AdditionalInfo.IsEPCValidated;
                transactionMapped.EPCValidateURL = AdditionalInfo.EPCValidateURL;
                transactionMapped.IsCommerceItemValidated = AdditionalInfo.IsCommerceItemValidated;
                transactionMapped.IsSimulation = AdditionalInfo.IsSimulation;
                transactionMapped.UniqueCode = AdditionalInfo.UniqueCode;
               transactionMapped.CurrencyId = AdditionalInfo.CurrencyId;
               transactionMapped.TransactionId = CustomTool.ConvertTNtoTransaction(AdditionalInfo.TransactionNumber); //CHANGE 4.1 TransactionNumber PBI 2528
            }
            else
            {
                transactionMapped.CustomerMail = "";
                transactionMapped.AppVersion = "unknown";
            }

            transactionMapped.Id = transactionEntity.Id;
            transactionMapped.InternalNbr = transactionEntity.InternalNbr;
            transactionMapped.Channel = transactionEntity.Channel;
            transactionMapped.Client = transactionEntity.Client;
            transactionMapped.SalePoint = transactionEntity.SalePoint;
            transactionMapped.Service = transactionEntity.Service;
            transactionMapped.Product = transactionEntity.Product;
            transactionMapped.Amount = transactionEntity.Amount;
            transactionMapped.Validator = transactionEntity.Validator;
            transactionMapped.WebSvcMethod = transactionEntity.WebSvcMethod;
            transactionMapped.ValidatorTransactionId = transactionEntity.TransactionId; //CHANGE 4.1 TransactionNumber PBI 2528
            transactionMapped.JSonObject = transactionEntity.JSonObject;
            transactionMapped.CreatedOn = transactionEntity.CreatedOn;
            transactionMapped.SettingId = transactionEntity.SettingId;
            transactionMapped.MerchantId = transactionEntity.MerchantId;
            transactionMapped.ElectronicPaymentCode = transactionEntity.ElectronicPaymentCode;
            transactionMapped.CurrencyCode = transactionEntity.CurrencyCode;
            transactionMapped.TrxCurrencyCode = transactionEntity.TrxCurrencyCode;
            transactionMapped.TrxAmount = transactionEntity.TrxAmount;
            transactionMapped.ConvertionRate = transactionEntity.ConvertionRate;
            transactionMapped.CommerceItems = GetTransactionCommerceItems(transactionEntity);
            return transactionMapped;
        }

        static public EF.TransactionResultInfo TransactionResultInfo_ModelToEF(TransactionResultInfoModel TransactionResultInfoToEF)
        {
            return new EF.TransactionResultInfo
            {
                Amount = TransactionResultInfoToEF.Amount,
                AuthorizationCode = TransactionResultInfoToEF.AuthorizationCode,
                CardHolder = TransactionResultInfoToEF.CardHolder,
                CardMask = TransactionResultInfoToEF.CardMask,
                CardNbrLfd = TransactionResultInfoToEF.CardNbrLfd,
                Country = TransactionResultInfoToEF.Country,
                CreatedBy = TransactionResultInfoToEF.CreatedBy,
                CreatedOn = TransactionResultInfoToEF.CreatedOn,
                Currency = TransactionResultInfoToEF.Currency,
                CustomerDocNumber = TransactionResultInfoToEF.CustomerDocNumber,
                CustomerDocType = TransactionResultInfoToEF.CustomerDocType,
                CustomerEmail = TransactionResultInfoToEF.CustomerEmail,
                ResponseCode = TransactionResultInfoToEF.ResponseCode,
                Payments = TransactionResultInfoToEF.Payments,
                StateExtendedMessage = TransactionResultInfoToEF.StateExtendedMessage,
                StateMessage = TransactionResultInfoToEF.StateMessage,
                TicketNumber = TransactionResultInfoToEF.TicketNumber,
                TransactionIdPK = TransactionResultInfoToEF.TransactionIdPK,
                TransactionResultInfoId = TransactionResultInfoToEF.TransactionResultInfoId,
                BatchNbr = TransactionResultInfoToEF.BatchNbr
            };

        }

        static public ValidatorModel Validator_EFToModel(EF.Validators validatorEntity)
        {
            return new ValidatorModel
            {
                ValidatorId = validatorEntity.ValidatorId,
                Name = validatorEntity.Name,
                Description = validatorEntity.Description,
                GenerateTransactionId = validatorEntity.GenerateTransactionId,
                SendMail = validatorEntity.SendMail,
                IsActive = validatorEntity.IsActive,
                CreatedBy = validatorEntity.CreatedBy,
                CreatedOn = validatorEntity.CreatedOn,
                UpdatedBy = validatorEntity.UpdatedBy,
                UpdatedOn = validatorEntity.UpdatedOn

            };
        }

        static public ServiceModel Service_EFToModel(EF.Services serviceEntity)
        {
            return new ServiceModel
            {
                ServiceId = serviceEntity.ServiceId,
                Name = serviceEntity.Name,
                Description = serviceEntity.Description,
                MerchantId = serviceEntity.MerchantId,
                IsActive = serviceEntity.IsActive,
                CreatedBy = serviceEntity.CreatedBy,
                CreatedOn = serviceEntity.CreatedOn,
                UpdatedBy = serviceEntity.UpdatedBy,
                UpdatedOn = serviceEntity.UpdatedOn,
                ClientId = serviceEntity.ClientId,
                IsCommerceItemsValidated = serviceEntity.ServicesConfig.FirstOrDefault().IsCommerceItemValidated

            };
        }

        static public EF.Services Service_ModelToEF(ServiceModel service)
        {
            return new EF.Services
            {
                ServiceId = service.ServiceId,
                ClientId = service.ClientId,
                Name = service.Name,
                Description = service.Description,
                MerchantId = service.MerchantId,
                IsActive = service.IsActive,
                CreatedBy = service.CreatedBy,
                CreatedOn = service.CreatedOn,
                UpdatedBy = service.UpdatedBy,
                UpdatedOn = service.UpdatedOn
            };
        }

        static public EF.Services Service_ModelToEF(EF.Services originalService, ServiceModel service)
        {
            originalService.ServiceId = service.ServiceId;
            originalService.ClientId = service.ClientId;
            originalService.Name = service.Name;
            originalService.Description = service.Description;
            originalService.MerchantId = service.MerchantId;
            originalService.IsActive = service.IsActive;
            originalService.CreatedBy = service.CreatedBy;
            originalService.CreatedOn = service.CreatedOn;
            originalService.UpdatedBy = service.UpdatedBy;
            originalService.UpdatedOn = service.UpdatedOn;

            return originalService;
        }

        static public EF.ServicesConfig ServiceConfig_ModelToEF(ServiceConfigModel serviceConfig)
        {
            return new EF.ServicesConfig
            {
                BranchId = serviceConfig.BranchId,
                ExternalId = serviceConfig.ExternalId,
                TerminalId = serviceConfig.TerminalId,
                RptToCentralizer = serviceConfig.RptToCentralizer,
                RptToConciliation = serviceConfig.RptToConciliation,
                RptToRendition = serviceConfig.RptToRendition,
                IsCallbackPosted = serviceConfig.IsCallbackPosted,
                SenderMail = serviceConfig.SenderMail,
                SenderURL = serviceConfig.SenderURL,
                IsActive = serviceConfig.IsActive,
                CreatedBy = serviceConfig.CreatedBy,
                CreatedOn = serviceConfig.CreatedOn,
                UpdatedBy = serviceConfig.UpdatedBy,
                UpdatedOn = serviceConfig.UpdatedOn,
                IsCommerceItemValidated = serviceConfig.IsCommerceItemValidated,
                IsPaymentSecured = serviceConfig.IsPaymentSecured,
                ReportsPath = serviceConfig.ReportsPath
            };
        }

        static public ServiceConfigModel ServiceConfig_EFToModel(EF.ServicesConfig servconfigEntity)
        {
            return new ServiceConfigModel
            {
                BranchId = servconfigEntity.BranchId,
                CreatedBy = servconfigEntity.CreatedBy,
                CreatedOn = servconfigEntity.CreatedOn,
                IsCallbackPosted = servconfigEntity.IsCallbackPosted,
                ExternalId = servconfigEntity.ExternalId,
                IsActive = servconfigEntity.IsActive,
                RenditionType = servconfigEntity.RenditionType,
                RptToCentralizer = servconfigEntity.RptToCentralizer,
                RptToConciliation = servconfigEntity.RptToConciliation,
                RptToRendition = servconfigEntity.RptToRendition,
                SenderMail = servconfigEntity.SenderMail,
                SenderURL = servconfigEntity.SenderURL,
                ServiceConfigId = servconfigEntity.ServiceConfigId,
                ServiceId = servconfigEntity.ServiceId,
                TerminalId = servconfigEntity.TerminalId,
                UpdatedBy = servconfigEntity.UpdatedBy,
                UpdatedOn = servconfigEntity.UpdatedOn,
                IsCommerceItemValidated = servconfigEntity.IsCommerceItemValidated,
                IsPaymentSecured = servconfigEntity.IsPaymentSecured,
                ReportsPath = servconfigEntity.ReportsPath,
                //ResponseTransactionNumber = servconfigEntity.ResponseWithTransactionNumber
            };
        }

        static public ProductModel Product_EFToModel(EF.Products productEntity)
        {
            return new ProductModel
            {
                ProductId = productEntity.ProductId,
                ProductCode = productEntity.ProductCode,
                Type = productEntity.Type,
                Description = productEntity.Description,
                IsActive = productEntity.IsActive,
                CreatedBy = productEntity.CreatedBy,
                CreatedOn = productEntity.CreatedOn,
                UpdatedBy = productEntity.UpdatedBy,
                UpdatedOn = productEntity.UpdatedOn,

            };
        }

        static public ClientModel Client_EFToModel(EF.Clients clientEntity)
        {
            return new ClientModel
            {
                ClientId = clientEntity.ClientId,
                TributaryCode = clientEntity.TributaryCode,
                ShortName = clientEntity.ShortName,
                LegalName = clientEntity.LegalName,
                SupportMail = clientEntity.SupportMail,
                IsActive = clientEntity.IsActive,
                CreatedBy = clientEntity.CreatedBy,
                CreatedOn = clientEntity.CreatedOn,
                UpdatedBy = clientEntity.UpdatedBy,
                UpdatedOn = clientEntity.UpdatedOn

            };
        }

        static public EF.Clients Client_ModelToEF(EF.Clients originalClient, ClientModel client)
        {

            originalClient.ClientId = client.ClientId;
            originalClient.TributaryCode = client.TributaryCode;
            originalClient.ShortName = client.ShortName;
            originalClient.LegalName = client.LegalName;
            originalClient.SupportMail = client.SupportMail;
            originalClient.IsActive = client.IsActive;
            originalClient.CreatedBy = client.CreatedBy;
            originalClient.CreatedOn = client.CreatedOn;
            originalClient.UpdatedBy = client.UpdatedBy;
            originalClient.UpdatedOn = client.UpdatedOn;

            return originalClient;
        }

        static public EF.Clients Client_ModelToEF(ClientModel client)
        {
            return new EF.Clients
            {
                ClientId = client.ClientId,
                TributaryCode = client.TributaryCode,
                ShortName = client.ShortName,
                LegalName = client.LegalName,
                SupportMail = client.SupportMail,
                IsActive = client.IsActive,
                CreatedBy = client.CreatedBy,
                CreatedOn = client.CreatedOn,
                UpdatedBy = client.UpdatedBy,
                UpdatedOn = client.UpdatedOn
            };
            
        }

        static public ChannelModel Channel_EFToModel(EF.Channels channelEntity)
        {
            return new ChannelModel
            {
                ChannelId = channelEntity.ChannelId,
                ChannelCode = channelEntity.ChannelCode,
                Name = channelEntity.Name,
                Description = channelEntity.Description,
                IsActive = channelEntity.IsActive,
                CreatedBy = channelEntity.CreatedBy,
                CreatedOn = channelEntity.CreatedOn,
                ModifiedBy = channelEntity.UpdatedBy,
                ModifiedOn = channelEntity.UpdatedOn
            };
        }

        static public CurrencyModel Currency_EFToModel(EF.Currency currencyEntity)
        {
            return new CurrencyModel
            {
                CurrencyId = currencyEntity.CurrencyId,
                IsoCode = currencyEntity.IsoCode,
                Description = currencyEntity.Description,
                IsActive = currencyEntity.IsActive,
                CreatedBy = currencyEntity.CreatedBy,
                CreatedOn = currencyEntity.CreatedOn,
            };
        }

        static public EF.PaymentClaim PaymentClaim_ModelToEF(PaymentClaimModel paymentClaim)
        {
            return new EF.PaymentClaim
            {
                PaymentClaimId = paymentClaim.PaymentClaimId,
                PaymentClaimNumber = paymentClaim.PaymentClaimNumber,
                ClaimerId = paymentClaim.ClaimerId,
                TransactionId = paymentClaim.TransactionIdPK,
                CurrencyId = paymentClaim.CurrencyId,
                Amount = paymentClaim.Amount,
                Observation = paymentClaim.Observation,
                IsLocked = paymentClaim.IsLocked,
                IsActive = paymentClaim.IsActive,
                CreatedBy = paymentClaim.CreatedBy,
                CreatedOn = paymentClaim.CreatedOn,
                UpdatedBy = paymentClaim.UpdatedBy,
                UpdatedOn = paymentClaim.UpdatedOn,
            };
        }

        static public PaymentClaimModel PaymentClaim_EFToModel(EF.PaymentClaim paymentClaimEntity)
        {
            return new PaymentClaimModel
            {
                PaymentClaimId = paymentClaimEntity.PaymentClaimId,
                ClaimerId = paymentClaimEntity.ClaimerId,
                TransactionIdPK = paymentClaimEntity.TransactionId,
                PaymentClaimNumber = paymentClaimEntity.PaymentClaimNumber,
                CurrencyId = paymentClaimEntity.CurrencyId,
                Amount = paymentClaimEntity.Amount,
                Observation = paymentClaimEntity.Observation,
                IsLocked = paymentClaimEntity.IsLocked,
                IsActive = paymentClaimEntity.IsActive,
                CreatedBy = paymentClaimEntity.CreatedBy,
                CreatedOn = paymentClaimEntity.CreatedOn,
                UpdatedBy = paymentClaimEntity.UpdatedBy,
                UpdatedOn = paymentClaimEntity.UpdatedOn
            };
        }

        static public EF.Claimer Claimer_ModelToEF(ClaimerModel claimer)
        {
            return new EF.Claimer
            {
                ClaimerId = claimer.ClaimerId,
                DocTypeId = claimer.DocTypeId,
                DocNumber = claimer.DocNumber,
                LastName = claimer.LastName,
                Name = claimer.Name,
                Email = claimer.Email,
                Phone = claimer.Phone,
                Cellphone = claimer.Cellphone
            };
        }

        static public ClaimerModel Claimer_EFToModel(EF.Claimer claimerEntity)
        {
            return new ClaimerModel
            {
                ClaimerId = claimerEntity.ClaimerId,
                DocNumber = claimerEntity.DocNumber,
                LastName = claimerEntity.LastName,
                Name = claimerEntity.Name,
                Email = claimerEntity.Email,
                Phone = claimerEntity.Phone,
                Cellphone = claimerEntity.Cellphone
            };
        }

        static public AnnulmentResultInfoModel AnnulmentResultInfo_EFToModel(EF.AnnulmentResultInfo AnnulmentResultInfoEntity)
        {
            return new AnnulmentResultInfoModel
            {
                TransactionId = AnnulmentResultInfoEntity.TransactionId,
                PaymentClaimId = AnnulmentResultInfoEntity.PaymentClaimId,
                ValidatorId = AnnulmentResultInfoEntity.ValidatorId,
                OperationNumber = AnnulmentResultInfoEntity.OperationNumber,
                IsActive = AnnulmentResultInfoEntity.IsActive,
                AuthorizationCode = AnnulmentResultInfoEntity.AuthorizationCode,
                CreatedBy = AnnulmentResultInfoEntity.CreatedBy,
                OriginalDateTime = AnnulmentResultInfoEntity.OriginalDateTime
            };
        }

        static public EF.AnnulmentResultInfo AnnulmentResultInfo_ModelToEF(AnnulmentResultInfoModel AnnulmentResultInfoModel)
        {
            return new EF.AnnulmentResultInfo
            {
                TransactionId = AnnulmentResultInfoModel.TransactionId,
                PaymentClaimId = AnnulmentResultInfoModel.PaymentClaimId,
                ValidatorId = AnnulmentResultInfoModel.ValidatorId,
                OperationNumber = AnnulmentResultInfoModel.OperationNumber,
                IsActive = AnnulmentResultInfoModel.IsActive,
                AuthorizationCode = AnnulmentResultInfoModel.AuthorizationCode,
                CreatedBy = AnnulmentResultInfoModel.CreatedBy,
                OriginalDateTime = AnnulmentResultInfoModel.OriginalDateTime
            };
        }



        static public DocTypeModel DocType_EFToModel(EF.DocType docTypeEntity)
        {
            return new DocTypeModel
            {
                DocTypeId = docTypeEntity.DocTypeId,
                ShortName = docTypeEntity.ShortName,
                Name = docTypeEntity.Name,
            };
        }

        static public PaymentClaimStatusModel PaymentClaimStatus_EFToModel(EF.PaymentClaimStatus paymentClaimStatusEntity)
        {
            return new PaymentClaimStatusModel
            {
                PaymentClaimStatusId = paymentClaimStatusEntity.PaymentClaimStatusId,
                PaymentClaimId = paymentClaimStatusEntity.PaymentClaimId,
                StatusCodeId = paymentClaimStatusEntity.StatusCodeId,
                Observation = paymentClaimStatusEntity.Observation,
                TicketNumber = paymentClaimStatusEntity.TicketNumber,
                IsActual = paymentClaimStatusEntity.IsActual,
                CreatedBy = paymentClaimStatusEntity.CreatedBy,
                CreatedOn = paymentClaimStatusEntity.CreatedOn,
                UpdatedBy = paymentClaimStatusEntity.UpdatedBy,
                UpdatedOn = paymentClaimStatusEntity.UpdatedOn
            };
        }

        //static public EF.PaymentClaimStatus PaymentClaimStatus_EFToModel(Models.PaymentClaimStatusModel paymentClaimStatus)
        //{
        //    return new EF.PaymentClaimStatus
        //    {
        //        PaymentClaimStatusId = paymentClaimStatus.PaymentClaimStatusId,
        //        PaymentClaimId = paymentClaimStatus.PaymentClaimId,
        //        StatusCodeId = paymentClaimStatus.StatusCodeId,
        //        ExternalAppId = paymentClaimStatus.ExternalAppId,
        //        Observation = paymentClaimStatus.Observation,
        //        TicketNumber = paymentClaimStatus.TicketNumber,
        //        IsActual = paymentClaimStatus.IsActual,
        //        CreatedBy = paymentClaimStatus.CreatedBy,
        //        CreatedOn = paymentClaimStatus.CreatedOn,
        //        UpdatedBy = paymentClaimStatus.UpdatedBy,
        //        UpdatedOn = paymentClaimStatus.UpdatedOn
        //    };
        //}

        static public StatusCodeModel StatusCode_EFToModel(EF.StatusCode statusCodeEntity)
        {
            return new StatusCodeModel
            {
                StatusCodeId = statusCodeEntity.StatusCodeId,
                Code = statusCodeEntity.Code,
                GenericCodeId = statusCodeEntity.GenericCodeId,
                IsActive = statusCodeEntity.IsActive,
            };
        }

        static public NotificationTemplateModel NotificationTemplateModel_EFToModel(EF.NotificationTemplate notificationTemplateEntity)
        {
            return new NotificationTemplateModel
            {
                NotificationTemplateId = notificationTemplateEntity.NotificationTemplateId,
                TemplateSubject = notificationTemplateEntity.TemplateSubject,
                TemplateBody = notificationTemplateEntity.TemplateBody,
                TemplateTicket = notificationTemplateEntity.TemplateTicket,
                IsActive = notificationTemplateEntity.IsActive,
                CreatedBy = notificationTemplateEntity.CreatedBy,
                CreatedOn = notificationTemplateEntity.CreatedOn,
                UpdatedBy = notificationTemplateEntity.UpdatedBy,
                UpdatedOn = notificationTemplateEntity.UpdatedOn
            };
        }

        static private IEnumerable<CommerceItemModel> GetTransactionCommerceItems(EF.Transactions transactionEntity)
        {
            var commerceItemsCollection = new List<CommerceItemModel>();

            if (transactionEntity.CommerceItems.Any())
            {
                foreach (var commerceItem in transactionEntity.CommerceItems)
                {
                    var cItem = new CommerceItemModel
                    {
                        Code = commerceItem.Code,
                        CreatedBy = commerceItem.CreatedBy,
                        CreatedOn = commerceItem.CreatedOn,
                        Description = commerceItem.Description,
                        CommerceItemId = commerceItem.CommerceItemsId,
                        Amount = commerceItem.Amount,
                        TransactionId = commerceItem.TransactionIdPK
                    };

                    commerceItemsCollection.Add(cItem);
                }
            }
            return commerceItemsCollection;
        }

        static public StatusMessageModel StatusMessage_EFToModel(EF.StatusMessage statusMessageEntity)
        {
            return new StatusMessageModel
            {
                Id = statusMessageEntity.Id,
                IdLanguage = statusMessageEntity.IdLanguage,
                StatusCodeId = statusMessageEntity.StatusCodeId,
                Message = statusMessageEntity.Message,
                IsActive = statusMessageEntity.IsActive,
                CreatedBy = statusMessageEntity.CreatedBy,
                CreatedOn = statusMessageEntity.CreatedOn,
                UpdatedBy = statusMessageEntity.UpdatedBy,
                UpdatedOn = statusMessageEntity.UpdatedOn
            };
        }

        static public AnnulmentRequestModel AnnulmentRequest_EFToModel(EF.AnnulmentRequest annulmentRequestEntity)
        {
            return new AnnulmentRequestModel
            {
                AnnulmentRequestId = annulmentRequestEntity.AnnulmentRequestId,
                Date = annulmentRequestEntity.Date,
                Result = annulmentRequestEntity.Result,
                PaymentClaimId = annulmentRequestEntity.PaymentClaimId,
                ResponseModuleCode = annulmentRequestEntity.ResponseModuleCode,
                IsActive = annulmentRequestEntity.IsActive,
            };
        }

        static public EF.AnnulmentRequest AnnulmentRequest_ModelToEF(AnnulmentRequestModel annulmentRequest)
        {
            return new EF.AnnulmentRequest
            {
                AnnulmentRequestId = annulmentRequest.AnnulmentRequestId,
                Date = annulmentRequest.Date,
                PaymentClaimId = annulmentRequest.PaymentClaimId,
                ResponseModuleCode = annulmentRequest.ResponseModuleCode,
                Result = annulmentRequest.Result,
                IsActive = annulmentRequest.IsActive,
                CreatedBy = annulmentRequest.CreatedBy,
                CreatedOn = annulmentRequest.CreatedOn,
                UpdatedBy = annulmentRequest.UpdatedBy,
                UpdatedOn = annulmentRequest.UpdatedOn
            };
        }

        static public EF.AnnulmentValidatorComm AnnulmentValidatorComm_ModelToEF(AnnulmentValidatorCommModel annulmentValidatorCommModel)
        {
            return new EF.AnnulmentValidatorComm
            {
                AnnulmentValidatorCommId = annulmentValidatorCommModel.AnnulmentValidatorCommId,
                AnnulmentRequestId = annulmentValidatorCommModel.AnnulmentRequestId,
                RequestDate = annulmentValidatorCommModel.RequestDate,
                RequestMessage = annulmentValidatorCommModel.RequestMessage,
                ResponseDate = annulmentValidatorCommModel.ResponseDate,
                ResponseMessage = annulmentValidatorCommModel.ResponseMessage,
            };
        }

        static public AnnulmentValidatorCommModel AnnulmentValidatorComm_EFToModel(EF.AnnulmentValidatorComm annulmentValidatorCommEntity)
        {
            return new AnnulmentValidatorCommModel
            {
                AnnulmentValidatorCommId = annulmentValidatorCommEntity.AnnulmentValidatorCommId,
                AnnulmentRequestId = annulmentValidatorCommEntity.AnnulmentRequestId,
                RequestDate = annulmentValidatorCommEntity.RequestDate,
                RequestMessage = annulmentValidatorCommEntity.RequestMessage,
                ResponseDate = annulmentValidatorCommEntity.RequestDate,
                ResponseMessage = annulmentValidatorCommEntity.ResponseMessage,
            };
        }

        static public EF.TicketLog TicketLog_ModelToEF(TicketLogModel ticketLog)
        {
            return new EF.TicketLog
            {
                TicketLogId = ticketLog.TicketLogId,
                TicketNumber = ticketLog.TicketNumber,
                TypeFormat = ticketLog.TypeFormat,
                HtmlTicket = ticketLog.HtmlTicket,
                CreatedBy = ticketLog.CreatedBy,
                CreatedOn = ticketLog.CreatedOn
            };
        }

        static public TicketLogModel TicketLog_EFToModel(EF.TicketLog ticketLogEntity)
        {
            return new TicketLogModel
            {
                TicketLogId = ticketLogEntity.TicketLogId,
                TicketNumber = ticketLogEntity.TicketNumber,
                TypeFormat = ticketLogEntity.TypeFormat,
                HtmlTicket = ticketLogEntity.HtmlTicket,
                CreatedBy = ticketLogEntity.CreatedBy,
                CreatedOn = ticketLogEntity.CreatedOn
            };
        }

        static public EF.NotificationLog NotificationLog_ModelToEF(NotificationLogModel notificationLog)
        {
            return new EF.NotificationLog
            {
                NotificationLogId = notificationLog.NotificationLogId,
                ModuleId = notificationLog.ModuleId,
                TicketLogId = notificationLog.TicketLogId,
                TypeFormat = notificationLog.TypeFormat,
                HtmlNotification = notificationLog.HtmlNotification,
                CreatedBy = notificationLog.CreatedBy,
                CreatedOn = notificationLog.CreatedOn
            };
        }

        static public NotificationLogModel NotificationLog_EFToModel(EF.NotificationLog notificationLogEntity)
        {
            return new NotificationLogModel
            {
                NotificationLogId = notificationLogEntity.NotificationLogId,
                ModuleId = notificationLogEntity.ModuleId,
                TicketLogId = notificationLogEntity.TicketLogId,
                TypeFormat = notificationLogEntity.TypeFormat,
                HtmlNotification = notificationLogEntity.HtmlNotification,
                CreatedBy = notificationLogEntity.CreatedBy,
                CreatedOn = notificationLogEntity.CreatedOn
            };
        }

        static public GenericCodeModel GenericCode_EFToModel(EF.GenericCode genericCodeEntity)
        {
            return new GenericCodeModel
            {
                Id = genericCodeEntity.Id,
                Description = genericCodeEntity.Description,
                Type = genericCodeEntity.Type,
                CreatedBy = genericCodeEntity.CreatedBy,
                CreatedOn = genericCodeEntity.CreatedOn,
                UpdatedBy = genericCodeEntity.UpdatedBy,
                UpdatedOn = genericCodeEntity.UpdatedOn
            };
        }

        static public EF.TransactionAdditionalInfo GetTransactionAdditionalInfoById(long TransactionId)
        {
            try
            {
                EF.TransactionAdditionalInfo addInfo = new EF.TransactionAdditionalInfo();
                using (EF.PGDataEntities context = new EF.PGDataEntities())
                {
                    addInfo = context.TransactionAdditionalInfo.FirstOrDefault(ai => ai.TransactionIdPK == TransactionId);
                    return addInfo;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(PGDataAccess.Models.LogTypeModel.Error, ex,TransactionNumber: TransactionId.ToString(),logtransactionType:LogTransactionType.TransactionIdPK);
                throw new Exception("Ocurrió un error interno", ex.InnerException);
            }
        }

        static public EF.PaymentValidatorComm PaymentValidatorComm_ModelToEF(PaymentValidatorCommModel paymentValidatorCommEntity)
        {
            return new EF.PaymentValidatorComm
            {
                PaymentValidatorCommId = paymentValidatorCommEntity.PaymentValidatorCommId,
                TransactionIdPK = paymentValidatorCommEntity.TransactionId,
                RequestDate = paymentValidatorCommEntity.RequestDate,
                RequestMessage = paymentValidatorCommEntity.RequestMessage,
                ResponseDate = paymentValidatorCommEntity.ResponseDate,
                ResponseMessage = paymentValidatorCommEntity.ResponseMessage
            };
        }

        static public EF.PaymentValidatorComm PaymentValidatorComm_ModelToEF(EF.PaymentValidatorComm originalPaymentValidatorComm, PaymentValidatorCommModel paymentValidatorCommEntity)
        {
            originalPaymentValidatorComm.TransactionIdPK = paymentValidatorCommEntity.TransactionId;
            originalPaymentValidatorComm.RequestDate = paymentValidatorCommEntity.RequestDate;
            originalPaymentValidatorComm.RequestMessage = paymentValidatorCommEntity.RequestMessage;
            originalPaymentValidatorComm.ResponseDate = paymentValidatorCommEntity.ResponseDate;
            originalPaymentValidatorComm.ResponseMessage = paymentValidatorCommEntity.ResponseMessage;

            return originalPaymentValidatorComm;
        }

        static public CommerceItemModel CommerceItem_EFToModel(EF.CommerceItems commerceItemEntity)
        {
            return new CommerceItemModel
            {
                CommerceItemId = commerceItemEntity.CommerceItemsId,
                Amount = commerceItemEntity.Amount,
                Code = commerceItemEntity.Code,
                Description = commerceItemEntity.Description,
                OriginalCode = commerceItemEntity.OriginalCode,
                TransactionId = commerceItemEntity.TransactionIdPK,
                State = commerceItemEntity.State,
                CreatedBy = commerceItemEntity.CreatedBy,
                CreatedOn = commerceItemEntity.CreatedOn,
                UpdatedBy = commerceItemEntity.UpdatedBy,
                UpdatedOn = commerceItemEntity.UpdatedOn
            };
        }

        static public ConfigurationModel Configuration_EFToModel(EF.Configurations configurations)
        {
            return new ConfigurationModel
            {
                ConfigurationId = configurations.ConfigurationId,
                ServiceId = configurations.ServiceId,
                ChannelId = configurations.ChannelId,
                ProductId = configurations.ProductId,
                ValidatorId = configurations.ValidatorId,
                UniqueCode = configurations.UniqueCode,
                CommerceNumber = configurations.CommerceNumber,
                IsActive = configurations.IsActive,
                CreatedBy = configurations.CreatedBy,
                CreatedOn = configurations.CreatedOn,
                UpdatedBy = configurations.UpdatedBy,
                UpdatedOn = configurations.UpdatedOn
            };
        }

        static public EF.Configurations Configuration_ModelToEF(ConfigurationModel configuration)
        {
            return new EF.Configurations
            {
                ConfigurationId = configuration.ConfigurationId,
                ServiceId = configuration.ServiceId,
                ChannelId = configuration.ChannelId,
                ProductId = configuration.ProductId,
                ValidatorId = configuration.ValidatorId,
                UniqueCode = configuration.UniqueCode,
                CommerceNumber = configuration.CommerceNumber,
                IsActive = configuration.IsActive,
                CreatedBy = configuration.CreatedBy,
                CreatedOn = configuration.CreatedOn,
                UpdatedBy = configuration.UpdatedBy,
                UpdatedOn = configuration.UpdatedOn
            };
        }



    }
}