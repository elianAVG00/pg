using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PGMainService.Manager;
using PGMainService.Models;
using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Script.Serialization;
using LogModel = PGMainService.Models.LogModel;

namespace PGMainService.Controllers
{
    [AllowAnonymous]
    public class PaymentController : ApiController
    {
        private Utils _utilities = new Utils();
        private List<string> validationResults = new List<string>();

        [AppConfig]
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var header = this._utilities.GetDataFromHTTPRequest(this.Request.Headers);
            using (var dataContext = new PGDataServiceClient())
            {
                bool isPaymentOnline = Convert.ToBoolean(dataContext.GetAppConfig("IsPaymentOnline"));
                return !isPaymentOnline
                    ? this._utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT_OFF, header.Language)
                    : this._utilities.GetHTTPResponse("PG_HTTP_OK", header.Language);
            }
        }

        [AppConfig]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody] PaymentInput paymentInput)
        {
            var header = this._utilities.GetDataFromHTTPRequest(this.Request.Headers);

            using (var dataContext = new PGDataServiceClient())
            {
                bool isPaymentOnline = Convert.ToBoolean(dataContext.GetAppConfig("IsPaymentOnline"));
                if (!isPaymentOnline)
                    return this._utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT_OFF, header.Language);

                try
                {
                    if (dataContext.GetAppConfig("DebugMode") == "1")
                        this._utilities.InsertLog(new LogModel()
                        {
                            Type = LogType.Debug,
                            module = "PAYMENT LOG FOR DEBUG",

                            message = $"HEADERS: {JsonConvert.SerializeObject(this.Request.Headers)}||||BODY: {JsonConvert.SerializeObject(paymentInput)}"
                        });
                }
                catch (Exception) { }

            }

            if (!ModelState.IsValid || paymentInput == null)
                return _utilities.GetHTTPResponse(Constants.PG_HTTP_BADREQUEST, header.Language);

            if (header.HasAuthentication)
            {
                if (!header.IsAuthorized)
                    return _utilities.GetHTTPResponse(Constants.PG_HTTP_NO_CREDENTIALS, header.Language);
                try
                {
                    bool canUserAccessMerchant = _utilities.GetResponse<bool>("security", "CanUserGetMerchantInfo", true, new MerchantUserInfoInput()
                    {
                        username = header.User,
                        merchantId = paymentInput.MerchantId
                    });
                    if (!canUserAccessMerchant)
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_NOAUTH, header.Language);
                }
                catch (Exception ex)
                {
                    _utilities.InsertLogException(LogType.Error, ex);
                    return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language, "json", "Petición irrecuperable");
                }
            }
            else // Usuario anónimo
            {
                try
                {
                    bool canAnonymousAccessMerchant = _utilities.GetResponse<bool>("security", "CanUserGetMerchantInfo", true, new MerchantUserInfoInput()
                    {
                        merchantId = paymentInput.MerchantId
                    });
                    if (!canAnonymousAccessMerchant)
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_NOAUTH, header.Language);
                }
                catch (Exception ex)
                {
                    _utilities.InsertLogException(LogType.Error, ex);
                    return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language, "json", "Petición irrecuperable");
                }
            }
            var response = this.ProcessPaymentInput(paymentInput, header);
            return response;
        }

        private List<CommerceItemsModel> ConvertMetadataToNewCommerceItems(string json)
        {
            const string schema = "{\"type\":\"object\",\"$schema\": \"http://json-schema.org/draft-03/schema\",\"id\": \"http://jsonschema.net\",\"required\":true,\"properties\":{ \"CommerceItems\": { \"type\":\"array\", \"id\": \"http://jsonschema.net/CommerceItems\", \"required\":true, \"items\": { \"type\":\"object\", \"id\": \"http://jsonschema.net/CommerceItems/0\", \"required\":true, \"properties\":{ \"Code\": { \"type\":\"string\", \"id\": \"http://jsonschema.net/CommerceItems/0/Code\", \"required\":true }, \"Description\": { \"type\":\"string\", \"id\": \"http://jsonschema.net/CommerceItems/0/Description\", \"required\":true }, \"Price\": { \"type\":\"string\", \"id\": \"http://jsonschema.net/CommerceItems/0/Price\", \"required\":true }}}}}}";
            try
            {
                var jsonSchema = JsonSchema.Parse(schema);
                var jsonObject = JObject.Parse(json);
                if (jsonObject.IsValid(jsonSchema))
                {
                    List<CommerceItemsModel> listToReturn = new List<CommerceItemsModel>();

                    dynamic dynJson = JsonConvert.DeserializeObject(json);
                    //var dessarializedJson = JsonConvert.DeserializeObject(json);

                    foreach (var item in dynJson.CommerceItems)
                    {
                        CommerceItemsModel ciToAdd = new CommerceItemsModel();
                        ciToAdd.Amount = item.Price;
                        ciToAdd.Code = item.Code;
                        ciToAdd.Description = item.Description;
                        listToReturn.Add(ciToAdd);

                    }
                    return listToReturn;
                }
                else { return null; }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool CheckCommerceItem(List<CommerceItemsModel> listOfCommerceItems, string transactionAmountString)
        {
            decimal transactionAmount = this._utilities.ConvertStringToDecimal(transactionAmountString);
            bool valueToReturn = false;

            //Valido codigos repetidos
            var duplicatedCodes = (from lci in listOfCommerceItems
                                   group lci by lci.Code into ciGroup
                                   where ciGroup.Count() > 1
                                   select ciGroup.Key);

            if (duplicatedCodes.Any())
            {
                valueToReturn = false;
                validationResults.Add(string.Format("Existen códigos duplicados en los Commerce Items"));
            }

            //Valido montos incorrectos
            var validateAmounts = (from ciok in listOfCommerceItems
                                   where (ciok.Amount <= 0)
                                   select ciok.Amount).Any();

            if (validateAmounts)
            {
                valueToReturn = false;
                validationResults.Add("Los montos de los ítems de comercio no pueden ser nulos o menores a cero");
            }

            //Valido sumatoria total
            decimal? totalAmountToCheck = (from ci in listOfCommerceItems select ci.Amount).Sum();

            if (totalAmountToCheck != null)
            {
                if (totalAmountToCheck <= 0)
                {
                    valueToReturn = false;
                    validationResults.Add("El monto total de la suma de los items de comercio no puede ser menor o igual a cero");
                }

                if (totalAmountToCheck != transactionAmount)
                {
                    valueToReturn = false;
                    validationResults.Add(string.Format("El monto total de la transacción no coincide con la suma de los elementos que la componen"));
                }
                return true;
            }
            else
            {
                valueToReturn = false;
                validationResults.Add(string.Format("Los montos de los ítems de comercio no son correctos"));
            }

            return valueToReturn;

        }
        private HttpResponseMessage ProcessPaymentInput(PaymentInput paymentInput, HeaderRequestModel header)
        {
            ValidatorModel pgValidator;
            ServiceModel pgService;
            ChannelModel pgChannel;
            PaymentInputModel sanitizedPaymentInput;

            try
            {
                using (var dataContext = new PGDataServiceClient())
                {
                    _utilities.InsertLogCommon(LogType.Info, $"ProcessPaymentInput - paymentInput: {paymentInput}");

                    sanitizedPaymentInput = SanitizePaymentInput(paymentInput, header);
                    pgService = dataContext.GetServiceByMerchantId(paymentInput.MerchantId);

                    if (pgService == null)
                    {
                        validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_NOSERVICE, header.Language));
                    }

                    pgChannel = GetChannel(sanitizedPaymentInput.Channel, header.Language);

                    if (pgService != null && pgChannel != null)
                    {
                        pgValidator = GetValidator(pgChannel.ChannelId, pgService.ServiceId, sanitizedPaymentInput.GetProductId(), header);
                    }
                    else
                    {
                        pgValidator = null;
                        if (pgService == null) validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_NOSERVICE, header.Language));
                        if (pgChannel == null && !validationResults.Any(vr => vr.Contains("canal"))) validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDCHAN, header.Language));
                    }

                    if (validationResults.Any())
                    {
                        string errors = string.Join("; ", validationResults);
                        _utilities.InsertLogCommon(LogType.Warning, errors);
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_NOTACCEPTABLE, header.Language, "json", errors);
                    }
                    // si pgService, pgChannel o pgValidator son null se tienen que añadir a validationResults
                    if (pgService == null || pgChannel == null || pgValidator == null)
                    {
                        _utilities.InsertLogCommon(LogType.Error, "Error de configuración crítico: servicio, canal o validador no encontrado después de validaciones iniciales.");
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_CONFIG_ERROR, header.Language);
                    }
                }
            }
            catch (Exception ex)
            {
                _utilities.InsertLogException(LogType.Error, ex, nameof(ProcessPaymentInput));
                return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language);
            }

            bool isEPCValidationRequired = !string.IsNullOrEmpty(sanitizedPaymentInput.ValidateEPCreturnUrl);
            string urlToValidateEPC = isEPCValidationRequired ? sanitizedPaymentInput.ValidateEPCreturnUrl : "";

            long transactionIdPK = 0;

            try
            {
                using (var dataContext = new PGDataServiceClient())
                {
                    if (isEPCValidationRequired)
                    {
                        if (ValidateEPC(pgService.ServiceId, sanitizedPaymentInput.ElectronicPaymentCode))
                        {
                            return _utilities.GetHTTPResponse(Constants.PG_HTTP_REDIRECT, header.Language, newUrl: sanitizedPaymentInput.ValidateEPCreturnUrl);
                        }
                    }

                    string uniqueCode = dataContext.GetUniqueCode(pgService.ServiceId, pgChannel.ChannelId, int.Parse(sanitizedPaymentInput.ProductId));

                    if (uniqueCode == null)
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_CONFIG_ERROR, header.Language);

                    sanitizedPaymentInput.MerchantId = uniqueCode;

                    ProductModel pgProduct = dataContext.GetProductById(int.Parse(sanitizedPaymentInput.ProductId));
                    ClientModel pgClient = dataContext.GetClienttById(pgService.ClientId);
                    LanguageModel pgLanguage = dataContext.GetLanguageByCode(header.Language);
                    CurrencyModel pgCurrency = dataContext.GetCurrencyByIso(paymentInput.CurrencyCode);


                    bool shouldCheckCommerceItem;
                    try
                    {
                        var serviceConfig = dataContext.GetServiceConfigByServiceId(pgService.ServiceId);
                        if (serviceConfig == null)
                        {
                            _utilities.InsertLogCommon(LogType.Warning, $"ServiceConfig no encontrado para ServiceId: {pgService.ServiceId}");
                            return _utilities.GetHTTPResponse(Constants.PG_HTTP_CONFIG_ERROR, header.Language);
                        }
                        shouldCheckCommerceItem = serviceConfig.IsCommerceItemValidated;
                    }
                    catch (Exception ex)
                    {
                        _utilities.InsertLogException(LogType.Warning, ex, nameof(ProcessPaymentInput));
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_CONFIG_ERROR, header.Language);
                    }

                    List<CommerceItemsModel> commerceItemsToProcess = null;
                    bool commerceItemsAreValid = true;

                    if (string.IsNullOrWhiteSpace(sanitizedPaymentInput.Metadata))
                    {
                        if (string.IsNullOrWhiteSpace(sanitizedPaymentInput.CommerceItems))
                        {
                            if (shouldCheckCommerceItem)
                            {
                                validationResults.Add("El campo commerceitem es requerido.");
                                commerceItemsAreValid = false;
                            }
                        }
                        else
                        {
                            if (shouldCheckCommerceItem)
                            {
                                try
                                {
                                    commerceItemsToProcess = new JavaScriptSerializer().Deserialize<List<CommerceItemsModel>>(sanitizedPaymentInput.CommerceItems);
                                    if (commerceItemsToProcess == null || !commerceItemsToProcess.Any())
                                    {
                                        validationResults.Add("El formato de Commerce Item es inválido o la lista está vacía.");
                                        commerceItemsAreValid = false;
                                    }
                                    else
                                    {
                                        if (!CheckCommerceItem(commerceItemsToProcess, sanitizedPaymentInput.Amount))
                                        {
                                            commerceItemsAreValid = false;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    validationResults.Add("El formato de Commerce Item es inválido al deserializar.");
                                    commerceItemsAreValid = false;
                                }
                            }
                            else
                            {
                                validationResults.Add("El servicio debe validar sus commerce items. Contacte con el administrador.");
                                commerceItemsAreValid = false;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(sanitizedPaymentInput.CommerceItems))
                        {
                            commerceItemsToProcess = ConvertMetadataToNewCommerceItems(sanitizedPaymentInput.Metadata);
                            if (commerceItemsToProcess != null)
                            {
                                if (shouldCheckCommerceItem)
                                {
                                    if (!CheckCommerceItem(commerceItemsToProcess, sanitizedPaymentInput.Amount))
                                    {
                                        commerceItemsAreValid = false;
                                    }
                                }
                            }
                            else
                            {
                                validationResults.Add("El campo metadata no tiene el formato correcto.");
                                commerceItemsAreValid = false;
                            }
                        }
                        else // Hay Metadata Y CommerceItems
                        {
                            validationResults.Add("El campo metadata es inválido si también se provee CommerceItems. Utilice SOLAMENTE el campo CommerceItems.");
                            commerceItemsAreValid = false;
                        }
                    }

                    if (!commerceItemsAreValid || validationResults.Any()) // Chequear de nuevo por si CheckCommerceItem añadió errores
                    {
                        string errors = string.Join("; ", validationResults);
                        _utilities.InsertLogCommon(LogType.Warning, errors, null, nameof(ProcessPaymentInput));
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_NOTACCEPTABLE, header.Language, "json", errors);
                    }

                    bool finalIsCommerceItemValidated = commerceItemsAreValid && (commerceItemsToProcess != null && commerceItemsToProcess.Any());

                    var transactionToSave = CreateTransaction(header.User, sanitizedPaymentInput, pgChannel, pgService, pgClient, pgProduct, pgValidator, pgLanguage, pgCurrency);

                    try
                    {
                        transactionToSave.CustomerMail = paymentInput.MailAddress;
                        transactionToSave.BarCode = paymentInput.BarCode;
                        transactionToSave.UniqueCode = uniqueCode;
                        transactionToSave.IsEPCValidated = isEPCValidationRequired;
                        transactionToSave.EPCValidateURL = urlToValidateEPC;
                        transactionToSave.IsCommerceItemValidated = finalIsCommerceItemValidated;

                        var commerceItemsForDb = new List<CommerceItemModel>();
                        if (commerceItemsToProcess != null)
                        {
                            foreach (var ciSource in commerceItemsToProcess)
                                commerceItemsForDb.Add(new CommerceItemModel()
                                { State = 0, Amount = ciSource.Amount, Description = ciSource.Description, Code = ciSource.Code });
                        }

                        transactionIdPK = dataContext.SaveTransaction(transactionToSave, commerceItemsForDb.ToArray());

                        if (transactionIdPK != 0)
                        {
                            sanitizedPaymentInput.TransactionIdPK = transactionIdPK;
                            var savedTransaction = dataContext.GetTransactionById(transactionIdPK);
                            sanitizedPaymentInput.TransactionId = savedTransaction?.TransactionId ?? transactionIdPK.ToString();

                            dataContext.UpdateTransactionStatus(transactionIdPK, "PGPAYMENTOPEN", "payment", null);
                        }
                        else
                        {
                            _utilities.InsertLogCommon(LogType.Error, "Transaction con Id 0 - No salvada.", null, nameof(ProcessPaymentInput));
                            return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language);
                        }
                    }
                    catch (Exception ex)
                    {
                        _utilities.InsertLogException(LogType.Error, ex, nameof(ProcessPaymentInput));
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language);
                    }

                    _utilities.InsertLogCommon(LogType.Info, $"PluginServiceRequestURL: {pgValidator.Name}", transactionNumber: transactionIdPK.ToString());

                    PaymentResponseInternal pluginResponse = _utilities.PostPayment(sanitizedPaymentInput, pgValidator.Name);

                    if (pluginResponse == null)
                    {
                        dataContext.UpdateTransactionStatus(transactionIdPK, "PGPAYMENTSENTERROR", "payment", null);
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language, "json", "IDF-891");
                    }

                    _utilities.InsertLogCommon(LogType.Debug, "Respuesta retornada del plugin: " + pluginResponse.HTMLToResponse, null, nameof(ProcessPaymentInput));

                    if (pluginResponse.ResponseStatus != 200 || !string.IsNullOrWhiteSpace(pluginResponse.ValidatorTransactionId))
                    {
                        dataContext.UpdateTransactionStatus(transactionIdPK, "PGPAYMENTSENT", "payment", null);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(pluginResponse.ValidatorTransactionId))
                            _utilities.InsertLogCommon(LogType.Error, $"No hay ValidatorTransactionId generado para {pgValidator.Name}", null, nameof(ProcessPaymentInput));

                        dataContext.UpdateTransactionStatus(transactionIdPK, "PGPAYMENTSENTERROR", "payment", null);
                        _utilities.InsertLogCommon(LogType.Error, $"El envio al plugin {pgValidator.Name} devolvió error o faltó ValidatorTransactionId. Status: {pluginResponse.ResponseStatus}", null, nameof(ProcessPaymentInput));
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language, "json", "IDF-891");
                    }

                    _utilities.InsertLogCommon(LogType.Info, $"CONVERT DATA TO RESPONSE: PluginServiceResponseType: {pluginResponse.HTMLToResponse}", null, nameof(ProcessPaymentInput));

                    bool shouldReturnTransactionId = paymentInput.WithTransactionNumber.GetValueOrDefault();

                    if (shouldReturnTransactionId)
                    {
                        var paymentResponseObject = new PaymentResponse
                        {
                            CardHolderInterface = pluginResponse.HTMLToResponse,
                            // Es mejor devolver el TransactionId de PG que es el que el cliente conoce.
                            TransactionId = sanitizedPaymentInput.TransactionId
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, paymentResponseObject, new JsonMediaTypeFormatter());
                    }
                    else
                    {
                        return _utilities.GetHTTPResponse(Constants.PG_HTTP_OK, header.Language, "html", Regex.Replace(pluginResponse.HTMLToResponse, "^\"|\"$", ""));
                    }

                }
            }
            catch (Exception ex)
            {
                long errorLogReference = _utilities.InsertLogException(LogType.Error, ex, nameof(ProcessPaymentInput));
                return _utilities.GetHTTPResponse(Constants.PG_HTTP_ERROR, header.Language, reference: errorLogReference);
            }
        }

        private TransactionModel CreateTransaction(
            string currentUser,
            PaymentInput paymentOrder,
            ChannelModel channelModel,
            ServiceModel service,
            ClientModel client,
            ProductModel product,
            ValidatorModel validator,
            LanguageModel language,
            CurrencyModel currency)
        {
            TransactionModel transaction = new TransactionModel();
            Utils utils = new Utils();
            transaction.CurrentAmount = utils.ConvertStringToDecimal(paymentOrder.Amount);
            transaction.Amount = utils.ConvertStringToDecimal(paymentOrder.Amount);
            transaction.Channel = channelModel.Name;
            transaction.Client = client.ShortName;
            transaction.CreatedOn = DateTime.Now;
            transaction.InternalNbr = "";
            transaction.Validator = validator.Name;
            transaction.WebSvcMethod = "PayOnline";
            transaction.MerchantId = service.MerchantId;
            transaction.CurrencyCode = currency.IsoCode;
            transaction.ElectronicPaymentCode = paymentOrder.ElectronicPaymentCode;
            transaction.ConvertionRate = Decimal.Parse("0.00");
            transaction.SalePoint = "0";
            transaction.Service = service.Description;
            transaction.JSonObject = string.Empty;
            transaction.SettingId = 0;
            transaction.TrxCurrencyCode = paymentOrder.CurrencyCode;
            transaction.TrxAmount = utils.ConvertStringToDecimal(paymentOrder.Amount);
            transaction.Product = product.Description;
            transaction.AppVersion = paymentOrder.AppVersion;
            transaction.CreatedBy = currentUser;
            transaction.ChannelId = channelModel.ChannelId;
            transaction.ProductId = product.ProductId;
            transaction.ValidatorId = validator.ValidatorId;
            transaction.ServiceId = service.ServiceId;
            transaction.ClientId = client.ClientId;
            transaction.CallbackUrl = paymentOrder.CallbackUrl;
            transaction.BarCode = paymentOrder.BarCode;
            transaction.Payments = new int?(paymentOrder.Payments);
            transaction.LanguageId = language.Id;
            transaction.CurrencyId = currency.CurrencyId;
            transaction.ValidatorTransactionId = "";
            transaction.IsSimulation = Convert.ToBoolean((object)paymentOrder.IsSimulation);
            return transaction;
        }
        private bool ValidateEPC(int serviceId, string EPC)
        {
            if (!_utilities.GetResponse<bool>("transaction", "isEPCvalid", true, (object)new IsEPCValidModel()
            {
                EPC = EPC,
                serviceId = serviceId
            }))
                return false;
            _utilities.InsertLogCommon(LogType.Debug, $"Ya existe una transacción para el ElectronicPaymentCode {EPC}");
            return true;
        }

        // --- SanitizePaymentInput ---
        private PaymentInputModel SanitizePaymentInput(PaymentInput paymentToSanitize, HeaderRequestModel header)
        {
            if (paymentToSanitize.IsSimulation == null)
                paymentToSanitize.IsSimulation = 0;

            if (string.IsNullOrWhiteSpace(paymentToSanitize.Channel))
                paymentToSanitize.Channel = "WEB";

            using (var dataContext = new PGDataServiceClient())
            {
                _utilities.InsertLogCommon(LogType.Info, $"SanitizePaymentInput - PaymentToSanitize: {paymentToSanitize}");

                #region Validate Amount
                try
                {
                    decimal sanitizedAmount = _utilities.ConvertStringToDecimal(paymentToSanitize.Amount);
                    if (sanitizedAmount < 1M)
                        this.validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDAMOUNT, header.Language));
                    paymentToSanitize.Amount = sanitizedAmount.ToString();
                }
                catch (Exception)
                {
                    this.validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDAMOUNT, header.Language));
                    _utilities.InsertLogCommon(LogType.Error, "Error al sanitizar monto: " + paymentToSanitize.Amount);
                }
                #endregion

                #region Validate Product
                var product = dataContext.GetProductById(paymentToSanitize.GetProductId());
                if (product == null)
                {
                    _utilities.InsertLogCommon(LogType.Warning, "El valor del ProductId no es válido: " + paymentToSanitize.ProductId);
                    validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDPRODUCT, header.Language));
                }
                #endregion

                #region Validate Channel
                var channel = dataContext.GetChannelByName(paymentToSanitize.Channel);
                if (channel == null)
                {
                    _utilities.InsertLogCommon(LogType.Warning, "El valor del Channel no es válido: " + paymentToSanitize.Channel);
                    validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDCHAN, header.Language));
                }
                #endregion

                #region Validate CurrencyCode
                CurrencyModel currency = dataContext.GetCurrencyByIso(paymentToSanitize.CurrencyCode);
                if (currency == null)
                {
                    _utilities.InsertLogCommon(LogType.Warning, "El valor del CurrencyCode no es válido: " + paymentToSanitize.CurrencyCode);
                    validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDCURRENCY, header.Language));
                }
                #endregion

                if (paymentToSanitize.Payments == 0)
                    paymentToSanitize.Payments = 1;
            }
            return MapPaymentInputToModel(paymentToSanitize);
        }

        // --- MapPaymentInputToModel ---
        private PaymentInputModel MapPaymentInputToModel(PaymentInput paymentToSanitize)
        {
            var model = new PaymentInputModel
            {
                Amount = paymentToSanitize.Amount,
                AppVersion = paymentToSanitize.AppVersion,
                BarCode = paymentToSanitize.BarCode,
                CallbackUrl = paymentToSanitize.CallbackUrl,
                Channel = paymentToSanitize.Channel,
                CurrencyCode = paymentToSanitize.CurrencyCode,
                ElectronicPaymentCode = paymentToSanitize.ElectronicPaymentCode,
                MailAddress = paymentToSanitize.MailAddress,
                MerchantId = paymentToSanitize.MerchantId,
                Metadata = paymentToSanitize.Metadata,
                Payments = paymentToSanitize.Payments,
                ProductId = paymentToSanitize.ProductId,
                ValidateEPCreturnUrl = paymentToSanitize.ValidateEPCreturnUrl,
                CommerceItems = paymentToSanitize.CommerceItems
                // Si PaymentInputModel tiene IsSimulation, debería mapearse.
                // Asumiendo que PaymentInputModel SÍ tiene IsSimulation:
                // IsSimulation = paymentToSanitize.IsSimulation
            };
            return model;
        }

        // --- GetChannel ---
        private ChannelModel GetChannel(string channelName, string lang)
        {
            using (var dataContext = new PGDataServiceClient())
            {
                if (channelName != null)
                {
                    _utilities.InsertLogCommon(LogType.Info, "GetChannel - channelName: " + channelName);
                    var channel = dataContext.GetChannelByName(channelName);
                    if (channel == null)
                    {
                        _utilities.InsertLogCommon(LogType.Warning, "El campo identificador del canal no es válido. Channel: " + channelName);
                        validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDCHAN, lang));
                    }
                    return channel;
                }
                else
                {
                    _utilities.InsertLogCommon(LogType.Warning, "El campo identificador del canal (Channel) es requerido");
                    validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_INVALIDCHAN, lang));
                    return null;
                }
            }
        }

        // --- GetValidator ---
        public ValidatorModel GetValidator(int channelId, int serviceId, int productId, HeaderRequestModel header)
        {
            ValidatorModel validatorToReturn = new ValidatorModel();
            try
            {
                using (var dataContext = new PGDataServiceClient())
                {
                    _utilities.InsertLogCommon(LogType.Info, $"GetValidator - channelId: {channelId}; serviceId: {serviceId}; productId: {productId}");
                    validatorToReturn = dataContext.GetValidatorFromConfiguration(serviceId, channelId, productId);

                    if (validatorToReturn == null)
                    {
                        _utilities.InsertLogCommon(LogType.Warning, "No se encontró una configuración definida para los datos suministrados - ERR11021.");
                        validationResults.Add(_utilities.GetValidationResponse(Constants.PAYMENT_NOCONFIG, header.Language));
                    }
                }
            }
            catch (Exception ex)
            {
                _utilities.InsertLogException(LogType.Error, ex);
            }
            return validatorToReturn;
        }
    }
}