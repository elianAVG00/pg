using PGPluginSPS.Models;
using PGPluginSPS.PGDataAccess;
using PGPluginSPS.Utils;
using SPS_ServiceReference;
using SPS_ServiceReference.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Http;

namespace PGPluginSPS.Controllers
{
    public class RequestController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "RequestSPS - Service Online.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(PaymentInput paymentOrder)
        {
            if (paymentOrder == null)
            {
                LogTool.InsertLogCommon(LogTypeModel.Error, "PaymentInput es null en RequestController.Post", CallerMemberName: "Post");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "PaymentInput es null");
            }
            try
            {
                using (var context = new PGDataServiceClient())
                {
                    //nuevos cambios
                    var serviceInterface = new WebServiceInterface();
                    Transaction transactionTAI = serviceInterface.GetResponse<Transaction>("transaction", "TAI/" + paymentOrder.TransactionId.ToString());

                    if (transactionTAI == null) // Añadido chequeo de nulo
                    {
                        LogTool.InsertLogCommon(LogTypeModel.Error, $"TAI no encontrado para ID: {paymentOrder.TransactionId}", CallerMemberName: nameof(Post));
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Transaction details not found.");
                    }
                    long transNumberTAI = transactionTAI.TransactionNumber;
                    if (transNumberTAI < 1L)
                    {
                        LogTool.InsertLogCommon(LogTypeModel.Warning, $"PG Transaction ID (IdPK) inválido: {transNumberTAI} para TAI ID: {paymentOrder.TransactionId}", CallerMemberName: nameof(Post));
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "TN (PG Transaction ID) does not exist");
                    }

                    var request = CreateRequest(transactionTAI);
                    context.UpdateTransactionValidatorData(transactionTAI.IdPK, transactionTAI.TransactionIdFromValidator, transactionTAI.UniqueCode + "." + transactionTAI.TransactionIdFromValidator);

                    string SPSRequest = "";
                    try
                    {
                        request.OperationNumber = transNumberTAI.ToString();

                        //Encrypted SHA Requirement for PluginSPS in Payment Gateway v3.3
                        request.EncryptedData = GetTransactionId(
                            request.CommerceNumber,
                            request.Amount,
                            request.ProductId.ToString(),
                            request.OperationNumber,
                            request.Payments,
                            ""
                        );

                        // var spsRequest = this.MapRequestToSpsServiceRequest(request);

                        //Si pertenece a la nueva API
                        string serviciosAPI = context.GetAppConfig("SPSAPIMethods");
                        string[] servicessplitted = serviciosAPI.Split(',');
                        int serviceIdFromTAI = transactionTAI.ServiceId;

                        if (servicessplitted.Contains(transactionTAI.ServiceId.ToString()))
                        {
                            string apikeypublic = context.GetAppConfig("ApiKeyPublicServiceId_" + serviceIdFromTAI.ToString());
                            string templateId = context.GetAppConfig("TemplateServiceId_" + serviceIdFromTAI.ToString());
                            ValidatorServiceConfigModel config = context.GetValidatorConfigByServiceId(serviceIdFromTAI, transactionTAI.ValidatorId);

                            long monto_corregido = Convert.ToInt64(transactionTAI.Amount * 100);

                            SPSRequest = CreateFormToSendToSPS(
                                            GetAPIPaymentURL(
                                                amount: monto_corregido,
                                                paymentmethod: request.ProductId,
                                                cuotas: Convert.ToInt32(request.Payments),
                                                mail: request.MailAddress,
                                                apikeyprivate: config.HashKey,
                                                apikeypublic: apikeypublic,
                                                transactionId: request.OperationNumber,
                                                templateId: templateId));
                        }
                        else
                        {
                            bool useHashInForm = context.GetAppConfig("SPSUseHashMethod") == "on";
                            SPSRequest = CreateFormToSendToDecidir(request, useHashInForm);
                        }

                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        var errors = new Dictionary<string, string>();

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                errors.Add(validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, errors);
                    }
                    catch (Exception exception)
                    {
                        LogTool.InsertLogException(LogTypeModel.Error, exception);

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, exception.ToString());
                    }

                    //Return Happy path

                    context.InsertPaymentValidatorComm(new PaymentValidatorCommModel()
                    {
                        RequestDate = DateTime.Now,
                        RequestMessage = SPSRequest,
                        TransactionId = transactionTAI.IdPK
                    });

                    serviceInterface.GetResponse<bool>("transaction", "UpdateTransactionIdFromTransactionNumber/" + transactionTAI.IdPK.ToString());

                    return Request.CreateResponse(HttpStatusCode.OK, new PaymentResponse()
                    {
                        HTMLToResponse = SPSRequest,
                        ResponseStatus = 1,
                        TransactionIdPK = transactionTAI.IdPK,
                        TransactionNumber = transactionTAI.TransactionNumber,
                        ValidatorTransactionId = transactionTAI.TransactionIdFromValidator
                    });
                }
            }
            catch (Exception exception)
            {
                LogTool.InsertLogException(LogTypeModel.Error, exception);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message);
            }

        }

        #region Private Methods

        private Request CreateRequest(Transaction paymentOrder)
        {
            return new Request()
            {
                CommerceNumber = paymentOrder.UniqueCode,
                Amount = paymentOrder.Amount,
                CallbackUrl = WebConfigurationManager.AppSettings["SPSCallback"],
                MailAddress = paymentOrder.CustomerMail,
                Payments = paymentOrder.payments.ToString(),
                ProductId = paymentOrder.ValidatorCardCode,
                ReturnUrl = paymentOrder.CallbackURL
            };
        }

        private string CreateFormToSendToSPS(string pspUrl)
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><meta http-equiv='refresh' content='0;url=");
            sb.Append(pspUrl);
            sb.Append("' /><title>Cargando...</title></head><body>Cargando...</body></html>");

            LogTool.InsertLogCommon(LogTypeModel.Debug, sb.ToString());

            return Regex.Replace(sb.ToString(), "^\"|\"$", "");
        }

        private string CreateFormToSendToDecidir(Request request, bool useHashAlgorithm)
        {
            var context = new PGDataServiceClient();
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>Cargando...");
            sb.AppendFormat("<form action='{0}' method='post'>", WebConfigurationManager.AppSettings["SPSEntryPoint"]);

            sb.AppendFormat("<input type='hidden' name='NROCOMERCIO' value='{0}'>", request.CommerceNumber);
            sb.AppendFormat("<input type='hidden' name='NROOPERACION' value='{0}'>", request.OperationNumber);
            string amountInCents = Convert.ToInt64(request.Amount * 100M).ToString();
            sb.AppendFormat("<input type='hidden' name='MONTO' value='{0}'>", amountInCents);
            sb.AppendFormat("<input type='hidden' name='CUOTAS' value='{0}'>", request.Payments);
            sb.AppendFormat("<input type='hidden' name='URLDINAMICA' value='{0}'>", request.CallbackUrl); //SPSCallback
            sb.AppendFormat("<input type='hidden' name='MEDIODEPAGO' value='{0}'>", request.ProductId);
            sb.AppendFormat("<input type='hidden' name='EMAILCLIENTE' value='{0}'>", request.MailAddress);
            sb.AppendFormat("<input type='hidden' name='PARAMSITIO' value='{0}'>", FormatDecimal(request.Amount));
            if (useHashAlgorithm)
                sb.AppendFormat("<input type='hidden' name='IDTRANSACCION' value='{0}'>", request.EncryptedData.ToLower());

            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            LogTool.InsertLogCommon(LogTypeModel.Debug, sb.ToString());

            return Regex.Replace(sb.ToString(), "^\"|\"$", "");

            //OLD VERSION
            //    string result = System.Text.Encoding.UTF8.GetString(response);
            //    result = result.Replace("SeleccionTarjeta",WebConfigurationManager.AppSettings["SPSEntryPoint"] + "/" + "SeleccionTarjeta");
            //    return result;
            //}
        }
        private static string FormatDecimal(Decimal input)
        {
            var provider = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            provider.NumberGroupSeparator = " ";
            provider.NumberDecimalDigits = 2;
            provider.NumberDecimalSeparator = ",";
            return input.ToString("#,0.00", provider);
        }

        public WebProxy getWebProxy()
        {
            //            <!-- Web Proxy Server -->
            if (WebConfigurationManager.AppSettings["proxyOn"] == "on")
            {
                string proxyServer = WebConfigurationManager.AppSettings["proxyServer"];
                string proxyPort = WebConfigurationManager.AppSettings["proxyPort"];
                string proxyDomain = WebConfigurationManager.AppSettings["proxyDomain"];
                string proxyUsername = WebConfigurationManager.AppSettings["proxyUsername"];
                string proxyPassword = WebConfigurationManager.AppSettings["proxyPassword"];
                WebProxy wbProxy;

                try
                {
                    wbProxy = new WebProxy(proxyServer, Int32.Parse(proxyPort))
                    {
                        Credentials = new NetworkCredential(proxyUsername, proxyPassword, proxyDomain)
                    };

                }
                catch (Exception ex)
                {
                    LogTool.InsertLogException(LogTypeModel.Error, ex);
                    return null;
                }
                return wbProxy;
            }
            return null;
        }

        public string GetTransactionId(string commerceNumber, decimal amount, string productId, string operationNumber, string payments, string encSHA1key)
        {
            string hash = "";
            string amountCents = Convert.ToInt64(amount * 100M).ToString();

            try
            {
                string stringToEncrypt = $"|NROCOMERCIO:{commerceNumber}|MONTO:{amountCents}|MEDIODEPAGO:{productId}|NROOPERACION:{operationNumber}|CUOTAS:{payments}|CLAVE:{encSHA1key}|";

                using (var sha1Provider = new SHA1CryptoServiceProvider())
                {
                    byte[] dataBuffer = new ASCIIEncoding().GetBytes(stringToEncrypt);
                    hash = BitConverter.ToString(sha1Provider.ComputeHash(dataBuffer)).Replace("-", "");
                }
            }
            catch (Exception exception)
            {
                LogTool.InsertLogException(LogTypeModel.Error, exception);
            }

            return hash;
        }


        private string GetAPIPaymentURL(string transactionId, long amount, int paymentmethod, int cuotas, string mail, string apikeyprivate, string apikeypublic, string templateId)
        {
            SPSAPIClient cliente = new SPSAPIClient();
            SPSClientConfigurationModel config = new SPSClientConfigurationModel();

            config.apikeyPrivate = apikeyprivate;
            config.urlbase = ConfigurationManager.AppSettings["SPS_API_BASE"];
            config.urlrequest = ConfigurationManager.AppSettings["SPS_API_REQUEST"];
            config.ProxyConfiguration = new ProxySettings
            {
                ProxyActive = ConfigurationManager.AppSettings["proxyOn"].Equals("on"),
                Domain = ConfigurationManager.AppSettings["proxyDomain"],
                Password = ConfigurationManager.AppSettings["proxyPassword"],
                Port = ConfigurationManager.AppSettings["proxyPort"],
                Server = ConfigurationManager.AppSettings["proxyServer"],
                Username = ConfigurationManager.AppSettings["proxyUsername"]
            };

            LogTool.InsertLogCommon(LogTypeModel.Debug, "CONFIG SOLICITUD API" + config.ToString());

            var payment = new SPSPaymentModel();
            payment.public_apikey = apikeypublic;
            payment.cancel_url = ConfigurationManager.AppSettings["PG_REDIRECT"];
            payment.redirect_url = ConfigurationManager.AppSettings["PG_REDIRECT"];
            //int templateSPS = Convert.ToInt32(ConfigurationManager.AppSettings["templateSPS"]); version anterior
            payment.site = new SPSPaymentModelsite
            {
                transaction_id = transactionId,
                template = new SPSPaymentModeltemplate
                {
                    id = Convert.ToInt32(templateId)
                }
            };
            payment.customer = new SPSPaymentModelcustomer
            {
                email = mail,
                ip_address = "1.1.1.1",
                id = "001"
            };
            payment.payment = new SPSPaymentModelpayment
            {
                amount = amount,
                currency = "ARS",
                installments = cuotas,
                payment_method_id = paymentmethod,
                payment_type = "single",
                sub_payments = new List<dynamic>()
            };

            try
            {
                return cliente.GetLinkHashedFromTransaction(config, payment);
            }
            catch (HttpRequestException ex) when (ex.InnerException is AuthenticationException)
            {
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogTool.InsertLogException(LogTypeModel.Debug, ex);
                throw ex;
            }
        }

        #endregion Private Methods
    }
}