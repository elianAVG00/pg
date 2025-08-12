using PGMainService.Manager;
using PGMainService.Models;
using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using LogModel = PGMainService.Models.LogModel;

namespace PGMainService.Controllers
{
    public class TransactionController : ApiController
    {
        private Utils _utilities = new Utils();

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var headerRequest = this._utilities.GetDataFromHTTPRequest(this.Request.Headers);
            using (PGDataServiceClient dataContext = new PGDataServiceClient())
            {
                bool isServiceOnline = Convert.ToBoolean(dataContext.GetAppConfig("IsServiceOnline"));
                return !isServiceOnline
                    ? this._utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT_OFF, headerRequest.Language)
                    : this._utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT, headerRequest.Language);
            }
        }

        [Authorize(Roles = "apiUpdateCommerceItem,apiAdminServices")]
        [Route("Transaction/UpdateCommerceItemCode/")]
        public HttpResponseMessage Post(UpdateCommerceItemCodeModel updateCommerceItem)
        {
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    TransactionModel transactionToUpdate = new TransactionModel();
                    try
                    {
                        if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                        {
                            if (!HttpContext.Current.User.IsInRole("apiUpdateCommerceItem"))
                                return Request.CreateResponse(HttpStatusCode.Forbidden);

                            var service = dataContext.GetServiceByMerchantId(updateCommerceItem.merchantCode);
                            if (service == null)
                                return Request.CreateResponse(HttpStatusCode.NotFound, "The merchant was not found");

                            transactionToUpdate = dataContext.GetTransactionByTransactionId(updateCommerceItem.TransactionId);
                            if (transactionToUpdate == null)
                                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "The transaction was not found");

                            if (!dataContext.CanUserGetMerchantInfo(HttpContext.Current.User.Identity.Name, updateCommerceItem.merchantCode))
                                return Request.CreateResponse(HttpStatusCode.Unauthorized, "User can't query that Merchant Code");
                        }
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
                    }

                    string languageCode;
                    try
                    {
                        languageCode = this.Request.Headers.AcceptLanguage.FirstOrDefault()?.ToString().Split('-')[0];
                    }
                    catch (Exception)
                    {
                        languageCode = "es";
                    }
                    if (string.IsNullOrEmpty(languageCode)) languageCode = "es";


                    bool isCommerceItemPresent = transactionToUpdate.CommerceItems.Any(ci => ci.Code == updateCommerceItem.OldCode);

                    if (!isCommerceItemPresent)
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"Commerce Item with Code {updateCommerceItem.OldCode} was not found");

                    bool updateResult = dataContext.UpdateCommerceItemCode(updateCommerceItem.OldCode, updateCommerceItem.NewCode, updateCommerceItem.TransactionId);
                    return updateResult
                        ? Request.CreateResponse(HttpStatusCode.OK)
                        : Request.CreateResponse(HttpStatusCode.Conflict, "The Commerce Item could not be processed");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
            }
        }

        [Authorize(Roles = "apiGetTransaction,apiAdminServices")]
        [Route("Transaction/GetCompleteInfo/")]
        [ResponseType(typeof(List<TransactionResult>))]
        public HttpResponseMessage Post(CriteriaModelToSearchByEPC criteriaByEPC)
        {
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    List<TransactionModel> transactionsFound = new List<TransactionModel>();
                    try
                    {
                        if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                        {
                            if (!HttpContext.Current.User.IsInRole("apiGetTransaction"))
                                return Request.CreateResponse(HttpStatusCode.Forbidden);

                            transactionsFound = dataContext.GetTransactionsByElectronicPaymentCodeAndMerchantId(criteriaByEPC.EPC, criteriaByEPC.merchantCode).ToList();

                            if (!transactionsFound.Any())
                                return Request.CreateResponse(HttpStatusCode.NotFound, "The transaction was not found");

                            if (!dataContext.CanUserGetMerchantInfo(HttpContext.Current.User.Identity.Name, criteriaByEPC.merchantCode))
                                return Request.CreateResponse(HttpStatusCode.Unauthorized, "User can't query that Merchant Code");
                        }
                        else // Si es AdminServices, cargar las transacciones si no se hizo
                        {
                            transactionsFound = dataContext.GetTransactionsByElectronicPaymentCodeAndMerchantId(criteriaByEPC.EPC, criteriaByEPC.merchantCode).ToList();
                        }
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
                    }

                    string languageCode;
                    try
                    {
                        languageCode = this.Request.Headers.AcceptLanguage.FirstOrDefault()?.ToString().Split('-')[0];
                    }
                    catch (Exception)
                    {
                        languageCode = "es";
                    }
                    if (string.IsNullOrEmpty(languageCode) || languageCode == "null")
                        languageCode = "es";

                    try
                    {
                        string maxTransactionsConfig = dataContext.GetAppConfig("MaxTransactionsByGetCompleteInfo");
                        int maxTransactionsAllowed = !string.IsNullOrWhiteSpace(maxTransactionsConfig) ? Convert.ToInt32(maxTransactionsConfig) : 50;

                        if (!transactionsFound.Any())
                            return Request.CreateResponse(HttpStatusCode.NoContent, new HttpError("Actualmente no hay transacciones que respondan a los parámetros especificados."));

                        if (transactionsFound.Count > maxTransactionsAllowed)
                            return Request.CreateResponse(HttpStatusCode.RequestEntityTooLarge, new HttpError("La cantidad de transacciones a retornar con los parámetros indicados es demasiado grande. Limite su consulta."));

                        var transactionResultsList = new List<TransactionResult>();
                        foreach (var transaction in transactionsFound)
                        {
                            TransactionCompletedInfo completedInfo = GetTransactionResultbyTransactionId(transaction.TransactionId, languageCode);
                            if (completedInfo != null)
                            {
                                transactionResultsList.Add(MapTCItoTResult(completedInfo));
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, transactionResultsList);
                    }
                    catch (Exception ex)
                    {
                        this._utilities.InsertLogException(LogType.Error, ex, nameof(Post), "J:\\PGv6\\paymentgateway\\dev6\\PGMainService\\Controllers\\TransactionController.cs", 273);

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error 3008");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
            }
        }

        [Route("Transaction/{merchantCode}/{electronicPaymentCode}")]
        [AllowAnonymous]
        public HttpResponseMessage Get(string electronicPaymentCode, string merchantCode)
        {
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    string languageCode;
                    try
                    {
                        languageCode = this.Request.Headers.AcceptLanguage.FirstOrDefault()?.ToString().Split('-')[0];
                    }
                    catch (Exception)
                    {
                        languageCode = "es";
                    }
                    if (string.IsNullOrEmpty(languageCode)) languageCode = "es";

                    try
                    {
                        var transactions = dataContext.GetTransactionsByElectronicPaymentCodeAndMerchantId(electronicPaymentCode, merchantCode).ToList();

                        switch (transactions.Count)
                        {
                            case 0:
                                return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError("Actualmente no hay transacciones que respondan a los parámetros especificados."));
                            case 1:
                                CheckAndCloseOpenTransaction(transactions.First().TransactionId);
                                return Request.CreateResponse(HttpStatusCode.OK, TransactionToCallbackResponse(transactions.First(), languageCode));
                            default:
                                var callbackList = new List<CallbackToPostModel>();
                                foreach (var transaction in transactions)
                                {
                                    CheckAndCloseOpenTransaction(transaction.TransactionId);
                                    callbackList.Add(TransactionToCallbackResponse(transaction, languageCode));
                                }
                                return Request.CreateResponse(HttpStatusCode.MultipleChoices, callbackList);
                        }
                    }
                    catch (Exception ex)
                    {
                        this._utilities.InsertLogException(LogType.Error, ex, nameof(Get), "J:\\PGv6\\paymentgateway\\dev6\\PGMainService\\Controllers\\TransactionController.cs", 356);

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error 3007");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
            }
        }

        [Authorize(Roles = "apiGetTransaction,apiAdminServices")]
        [Route("Transaction/GetRendition/{merchantId}/{date}")]
        [HttpGet]
        [ResponseType(typeof(List<RenditionData>))]
        public HttpResponseMessage GetRendition(string merchantId, string date)
        {
            int serviceIdForRendition = 0;
            DateTime parsedDate;
            try
            {
                parsedDate = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Date is wrong");
            }

            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    try
                    {
                        if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                        {
                            if (!HttpContext.Current.User.IsInRole("apiGetTransaction"))
                                return Request.CreateResponse(HttpStatusCode.Forbidden);

                            if (!dataContext.CanUserGetMerchantInfo(HttpContext.Current.User.Identity.Name, merchantId))
                                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauth");

                            ServiceModel serviceDetails = dataContext.GetServiceByMerchantId(merchantId);
                            if (serviceDetails == null)
                                return Request.CreateResponse(HttpStatusCode.NotFound, "The merchant was not found");

                            serviceIdForRendition = serviceDetails.ServiceId;
                        }
                        // Si es admin, serviceIdForRendition se mantiene en 0. 
                        // Para la llamada a GetResponse, el serviceid = (long) serviceIdForRendition será 0 para admins si no se obtiene antes.
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
                    }

                    try
                    {
                        var renditionInput = new RenditionOnlineInput()
                        {
                            day = parsedDate.Day,
                            month = parsedDate.Month,
                            year = parsedDate.Year,
                            serviceid = (long)serviceIdForRendition
                        };
                        return Request.CreateResponse(HttpStatusCode.OK,
                            this._utilities.GetResponse<List<RenditionData>>("transaction", "GetRenditionReportOnTheFly", true, renditionInput));
                    }
                    catch (Exception ex)
                    {
                        this._utilities.InsertLogException(LogType.Error, ex, nameof(GetRendition), "J:\\PGv6\\paymentgateway\\dev6\\PGMainService\\Controllers\\TransactionController.cs", 466);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error 3108");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
            }
        }

        [Authorize(Roles = "apiGetTransaction,apiAdminServices")]
        [Route("Transaction/{TransactionId}")]
        [ResponseType(typeof(TransactionResult))]
        public HttpResponseMessage Get(string TransactionId)
        {
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    try
                    {
                        if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                        {
                            if (!HttpContext.Current.User.IsInRole("apiGetTransaction"))
                                return Request.CreateResponse(HttpStatusCode.Forbidden);

                            if (!dataContext.CanUserGetTransaction(HttpContext.Current.User.Identity.Name, TransactionId))
                                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Transaction doesn't exist or user can't query it");
                        }
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
                    }

                    string languageCode;
                    try
                    {
                        languageCode = this.Request.Headers.AcceptLanguage.FirstOrDefault()?.ToString().Split('-')[0];
                    }
                    catch (Exception)
                    {
                        languageCode = "es";
                    }
                    if (string.IsNullOrEmpty(languageCode)) languageCode = "es";


                    try
                    {
                        TransactionModel transaction = dataContext.GetTransactionByTransactionId(TransactionId);
                        if (transaction == null)
                            return Request.CreateResponse(HttpStatusCode.NoContent, "La transacción solicitada no existe");

                        CheckAndCloseOpenTransaction(transaction.TransactionId);

                        TransactionCompletedInfo completedInfo = GetTransactionResultbyTransactionId(transaction.TransactionId, languageCode);
                        if (completedInfo == null)
                        {
                            // Manejar el caso donde la transacción existe pero no se puede obtener el TCI
                            // Podría ser un 500 o un 404 específico.
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Could not retrieve completed transaction info.");
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, MapTCItoTResult(completedInfo));
                    }
                    catch (Exception)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
                    }
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No se pudo obtener información sobre la transacción");
            }
        }

        private TransactionCompletedInfo GetTransactionResultbyTransactionId(
          string transactionId,
          string languageCode)
        {
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    long? transactionPk = dataContext.GetTransactionPKByTransactionId(transactionId);
                    if (!transactionPk.HasValue)
                        return null;

                    var transactionCompletedInfo = dataContext.GetTransactionResult(transactionPk.Value, languageCode);
                    return transactionCompletedInfo;
                }
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex, nameof(GetTransactionResultbyTransactionId), "J:\\PGv6\\paymentgateway\\dev6\\PGMainService\\Controllers\\TransactionController.cs", 596); // Log específico de prod
                                                                                                                                                                                                                 // _utilities.InsertLogException(LogType.Error, ex); // Log genérico de dev
                throw;
            }
        }

        private CallbackToPostModel TransactionToCallbackResponse(
          TransactionModel uniqueTransaction,
          string languageCode)
        {
            using (PGDataServiceClient dataContext = new PGDataServiceClient())
            {
                TransactionCompletedInfo transactionResult = dataContext.GetTransactionResult(uniqueTransaction.Id, languageCode);
                // Podría ser null si GetTransactionResult falla
                if (transactionResult == null)
                {
                    // Considerar devolver un CallbackToPostModel con error o lanzar excepción
                    // Por ahora, si transactionResult es null, GenerateCallbackToPost fallará con NullReferenceException
                    throw new InvalidOperationException($"Could not get transaction result for ID: {uniqueTransaction.Id}");
                }
                return GenerateCallbackToPost(transactionResult);
            }
        }

        private CallbackToPostModel GenerateCallbackToPost(TransactionCompletedInfo transactionResult)
        {
            return new CallbackToPostModel()
            {
                ElectronicPaymentCode = transactionResult.ElectronicPaymentCode,
                ResponseGenericCode = transactionResult.TransactionCompletedStatus.GenericCode,
                ResponseGenericMessage = transactionResult.TransactionCompletedStatus.GenericMessage,
                ResponseCode = transactionResult.TransactionCompletedStatus.ResponseCode,
                ResponseMessage = transactionResult.TransactionCompletedStatus.ResponseMessage,
                ResponseExtended = transactionResult.TransactionCompletedStatus.ResponseMessage,
                ReturnUrl = transactionResult.CallbackUrl,
                TransactionId = transactionResult.TransactionId
            };
        }

        private TransactionResult MapTCItoTResult(TransactionCompletedInfo tci)
        {
            var transactionResult = new TransactionResult();
            transactionResult.ChannelCode = tci.Channel;
            transactionResult.ElectronicPaymentCode = tci.ElectronicPaymentCode;
            transactionResult.ResultCode = tci.TransactionCompletedStatus.ResponseCode;
            transactionResult.ResultMessage = tci.TransactionCompletedStatus.ResponseMessage;
            transactionResult.GenericCode = tci.TransactionCompletedStatus.GenericCode;
            transactionResult.GenericMessage = tci.TransactionCompletedStatus.GenericMessage;
            transactionResult.TransactionId = tci.TransactionId;

            var claimsList = new List<Claims>();
            foreach (var paymentClaim in tci.PaymentClaims)
            {
                var claim = new Claims();
                claim.ClaimNumber = paymentClaim.PaymentClaimNumber;
                claim.ClaimResultCode = paymentClaim.PaymentClaimCompletedStatus.ResponseCode;
                claim.ClaimResultMessage = paymentClaim.PaymentClaimCompletedStatus.ResponseMessage;
                claim.Claimer = new Claimer()
                {
                    Cellphone = paymentClaim.Claimer.Cellphone,
                    DocNumber = paymentClaim.Claimer.DocNumber,
                    DocShortName = paymentClaim.Claimer.DocShortName,
                    Email = paymentClaim.Claimer.Email,
                    LastName = paymentClaim.Claimer.LastName,
                    FirstName = paymentClaim.Claimer.Name,
                    Phone = paymentClaim.Claimer.Phone
                };
                var commerceItemsInClaimList = new List<CommerceItems>();
                foreach (var commerceItem in paymentClaim.CommerceItems)
                {
                    var itemToAdd = new CommerceItems();
                    itemToAdd.Amount = commerceItem.Amount.ToString();
                    itemToAdd.Code = commerceItem.Code;
                    itemToAdd.Description = commerceItem.Description;
                    itemToAdd.OriginalCode = commerceItem.OriginalCode;
                    itemToAdd.State = commerceItem.State;
                    commerceItemsInClaimList.Add(itemToAdd);
                }
                claim.CommerceItemsRefunded = commerceItemsInClaimList;
                claimsList.Add(claim);
            }
            transactionResult.Claims = claimsList;

            transactionResult.Service = new Service()
            {
                ClientLegalName = tci.Client.LegalName,
                ServiceName = tci.Service.Name,
                ServiceShortCode = tci.Client.ShortName,
                MerchantId = tci.MerchantId
            };

            var paymentDetails = new Payment();
            paymentDetails.Amount = tci.Amount.ToString();
            paymentDetails.CurrencyCode = tci.CurrencyCode;
            paymentDetails.PayDateTime = tci.PayDate.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            paymentDetails.Payments = tci.Payments.ToString();
            paymentDetails.ProductCode = tci.Product.ProductCode.ToString();
            paymentDetails.ProductName = tci.Product.Description;
            paymentDetails.BarCode = tci.BarCode;
            transactionResult.Payment = paymentDetails;

            transactionResult.Customer = new Customer()
            {
                CardNumberMasked = tci.CardMask,
                AuthorizationCode = tci.AuthorizationCode,
                CustomerDocNumber = tci.CustomerDocNumber,
                CustomerDocType = tci.CustomerDocType,
                CustomerMail = tci.TransactionCustomerMail
            };

            var commerceItemsList = new List<CommerceItems>();
            foreach (var commerceItem in tci.CommerceItems)
            {
                var itemToAdd = new CommerceItems();
                itemToAdd.Amount = commerceItem.Amount.ToString();
                itemToAdd.Code = commerceItem.Code;
                itemToAdd.Description = commerceItem.Description;
                itemToAdd.OriginalCode = commerceItem.OriginalCode;
                itemToAdd.State = commerceItem.State;
                commerceItemsList.Add(itemToAdd);
            }
            transactionResult.CommerceItems = commerceItemsList;
            return transactionResult;
        }

        private bool? CheckAndCloseOpenTransaction(string transactionId)
        {
            bool? transactionStatusAfterCheck = null;
            try
            {
                using (PGDataServiceClient dataContext = new PGDataServiceClient())
                {
                    //le pega al DataAccess y devuelve 0 - No existe, 1 - Incompleta, 2 - Completa
                    CheckTransaction currentTransactionStatus = dataContext.CheckIfTransactionIsComplete(transactionId);
                    if (currentTransactionStatus == null) return null;

                    string validatorShortName = currentTransactionStatus.ValidatorShortName;
                    int checkResultCode = currentTransactionStatus.CheckResult;

                    int validatorId = dataContext.GetValidatorByCode(validatorShortName).ValidatorId;
                    TransactionModel transactionDetails = dataContext.GetTransactionByValidatorTransactionId(transactionId, validatorId);
                    if (transactionDetails == null) return null; // Transacción no encontrada por ID de validador

                    //le pega al PGDataLayer
                    Models.AppConfigModel timeoutConfig = this._utilities.GetResponse<Models.AppConfigModel>("home", "configuration/" + (transactionDetails.Validator + "_Timeout"));
                    int timeoutMinutes = 0;
                    if (timeoutConfig != null && !string.IsNullOrEmpty(timeoutConfig.Value))
                    {
                        DateTime timeoutBaseTime = timeoutConfig.CreateTime ?? new DateTime(1970, 1, 1, 0, 0, 0);
                        if (timeoutBaseTime.Millisecond != 780 && timeoutBaseTime.Millisecond != 800 && timeoutBaseTime.AddDays(400.0) < DateTime.Now)
                        {
                            timeoutConfig.Value = "0";
                        }
                        timeoutMinutes = Convert.ToInt32(timeoutConfig.Value);
                    }

                    DateTime transactionCreationTime = transactionDetails.CreatedOn;
                    DateTime transactionExpiryTime = transactionCreationTime.AddMinutes(timeoutMinutes);
                    DateTime currentTime = DateTime.Now;

                    switch (checkResultCode)
                    {
                        case 0:
                            transactionStatusAfterCheck = null;
                            break;
                        case 1:
                            //le pega al PGPluginSPS para checker si cerro o cerrar la transaccion
                            this._utilities.GetRealResponse($"{WebConfigurationManager.AppSettings[$"Plugin_{validatorShortName}_URL"]}/{WebConfigurationManager.AppSettings["PluginMethod_transactionInfo"]}/CloseOperation/", transactionId);

                            //vuelve a pegarle al DataAccess y devuelve 0 - No existe, 1 - Incompleta, 2 - Completa
                            var newStatusAfterCloseAttempt = dataContext.CheckIfTransactionIsComplete(transactionId);
                            if (newStatusAfterCloseAttempt == null) return null;

                            switch (newStatusAfterCloseAttempt.CheckResult)
                            {
                                case 0:
                                    transactionStatusAfterCheck = null;
                                    break;
                                case 1:
                                    if (currentTime > transactionExpiryTime && timeoutMinutes > 0) // Solo cerrar por timeout si el timeout es > 0
                                    {
                                        dataContext.UpdateTransactionCloseByTimeOut(transactionDetails.Id);
                                        transactionStatusAfterCheck = false; // Cerrada por timeout -> completa
                                    }
                                    else
                                    {
                                        transactionStatusAfterCheck = true;
                                    }
                                    break;
                                case 2:
                                    TransactionCompletedInfoWithPk completedTxResult = GetTransactionResult(transactionId, "es", validatorId);
                                    if (completedTxResult != null)
                                    {
                                        //si esta completa le pega al DataLayer para enviar ticket
                                        long notificationResponse = this._utilities.GetResponse<long>("notification", "SendQuickTicketOfPayment/" + completedTxResult.TransactionIdPk.ToString());
                                        if (notificationResponse < 1L)
                                        {
                                            this._utilities.InsertLog(new LogModel()
                                            {
                                                Type = LogType.Error,
                                                module = "GetResponseToFinishTransaction",
                                                transaction = completedTxResult.TransactionIdPk,
                                                message = "SendQuickTicketOfPayment returns " + notificationResponse.ToString()
                                            });
                                        }
                                    }
                                    transactionStatusAfterCheck = false;
                                    break;
                            }
                            break;
                        case 2:
                            transactionStatusAfterCheck = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex, nameof(CheckAndCloseOpenTransaction), "J:\\PGv6\\paymentgateway\\dev6\\PGMainService\\Controllers\\TransactionController.cs", 895);

                transactionStatusAfterCheck = null; // Error durante el proceso
            }
            return transactionStatusAfterCheck;
        }

        private static TransactionCompletedInfoWithPk GetTransactionResult(
          string transactionId,
          string language,
          int validatorId)
        {
            using (PGDataServiceClient dataContext = new PGDataServiceClient())
            {
                var result = new TransactionCompletedInfoWithPk();
                TransactionModel transaction = dataContext.GetTransactionByValidatorTransactionId(transactionId, validatorId);
                if (transaction == null)
                    return null;

                result.TransactionIdPk = transaction.Id;
                result.TransactionCompletedInfo = dataContext.GetTransactionResult(transaction.Id, language);
                // GetTransactionResult podría devolver null también.
                if (result.TransactionCompletedInfo == null) return null;

                return result;
            }
        }
    }
}