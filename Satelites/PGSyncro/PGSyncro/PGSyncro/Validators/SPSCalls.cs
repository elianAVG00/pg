using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PGSyncro.Validators
{
    public class SPSCalls
    {
        private readonly ILogger<SPSCalls> _logger;
        private readonly IConfiguration _configuration;
        private readonly NetworkTools _networkTools;
        private readonly ConversionTools _conversionTools;

        public SPSCalls(
            ILogger<SPSCalls> logger,
            IConfiguration configuration,
            NetworkTools networkTools,
            ConversionTools conversionTools)
        {
            _logger = logger;
            _configuration = configuration;
            _networkTools = networkTools;
            _conversionTools = conversionTools;
        }

        public async Task<TransactionOriginal> GetXMLServiceAsync(GetTransactionsToSync_Result transaction)
        {
            var retorno = new TransactionOriginal();
            var queryResponse = new SyncroModel { ServiceType = "DECIDIR XML-RPC" };
            retorno.ModuleType = "webservice";
            retorno.TransactionIdPK = transaction.TransactionIdPK;

            try
            {
                string soapStr = string.Format(
                    @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body><p:Get xmlns:p=""urn:Decidir.net""><xs:IDSITE xmlns:xs=""urn:Decidir.net"">{0}</xs:IDSITE><xs:IDTRANSACTIONSIT xmlns:xs=""urn:Decidir.net"">{1}</xs:IDTRANSACTIONSIT></p:Get></soap:Body></soap:Envelope>",
                    transaction.UniqueCode, transaction.TransactionNumber);

                string methodName = "Get";
                string url = _configuration["DecidirSettings:SPS_Operation_GetData_URL"];
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = _networkTools.GetWebProxy();
                req.Headers.Add("Authorization: " + _configuration["DecidirSettings:SPS_Operation_GetData_Authorization"]);
                req.Headers.Add("SOAPAction", $"\"http://tempuri.org/{methodName}\"");
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";

                using (Stream requestStream = await req.GetRequestStreamAsync())
                using (StreamWriter streamWriter = new StreamWriter(requestStream))
                {
                    await streamWriter.WriteAsync(soapStr);
                }

                queryResponse.RequestLog = soapStr;

                using (var response = await req.GetResponseAsync())
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = await streamReader.ReadToEndAsync();
                    queryResponse.ResponseLog = result;

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        queryResponse.HasResponse = false;
                        queryResponse.HasTransaction = false;
                    }
                    else if (!result.Contains("IDTRANSACCIONSITE"))
                    {
                        queryResponse.HasResponse = true;
                        queryResponse.HasTransaction = false;
                    }
                    else
                    {
                        ParseXmlResponse(result, retorno, queryResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F25: Error en GetXMLService.");
                queryResponse.HasError = true;
                queryResponse.ErrorInQuery = ex.Message;
            }

            retorno.QueryResponse = queryResponse;
            return FixSPSBatch(retorno, transaction.ServiceId);
        }

        public async Task<TransactionOriginal> GetAPIServiceAsync(GetTransactionsToSync_Result transaction, string apikeyPrivate)
        {
            var retorno = new TransactionOriginal();
            var queryResponse = new SyncroModel { ServiceType = "DECIDIR API 2.0" };
            retorno.ModuleType = "api";
            retorno.TransactionIdPK = transaction.TransactionIdPK;

            try
            {
                string urlbase = _configuration["DecidirSettings:SPS_API_BASE"];
                string urlrequest = _configuration["DecidirSettings:SPS_API_TRANSACTION"];
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var options = new RestClientOptions(urlbase)
                {
                    Proxy = _networkTools.GetWebProxy()
                };
                var client = new RestClient(options);
                var request = new RestRequest(urlrequest, Method.Get);
                request.AddHeader("apikey", apikeyPrivate);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("siteOperationId", transaction.TransactionNumber.ToString());
                request.AddParameter("expand", "card_data");

                queryResponse.RequestLog = GetRequestLog(request, client.BuildUri(request));
                var response = await client.ExecuteAsync(request);
                queryResponse.ResponseLog = GetResponseLog(response);

                HandleRestResponse(response, retorno, queryResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F27: Error en GetAPIService.");
                queryResponse.HasTransaction = false;
                queryResponse.HasError = true;
                queryResponse.ErrorInQuery = ex.Message;
            }

            retorno.QueryResponse = queryResponse;
            return FixSPSBatch(retorno, transaction.ServiceId);
        }

        private void ParseXmlResponse(string xml, TransactionOriginal retorno, SyncroModel queryResponse)
        {
            try
            {
                queryResponse.HasResponse = true;
                queryResponse.HasTransaction = true;
                XDocument resultXML = XDocument.Parse(xml);
                var dictionary = new Dictionary<string, string>();
                XNamespace xmlns = "urn:Decidir.net";
                var operationElement = resultXML.Document.Descendants(xmlns + "Operation").FirstOrDefault();

                if (operationElement != null && operationElement.HasElements)
                {
                    foreach (XElement element in operationElement.Descendants())
                    {
                        dictionary[element.Name.LocalName] = element.Value;
                    }
                    retorno.Amount = Convert.ToInt64(_conversionTools.ConvertStringToDecimal(GetKeyOrEmpty(dictionary, "MONTO")) * 100M);
                    retorno.Payments = _conversionTools.ToNullableInt32(GetKeyOrEmpty(dictionary, "CUOTAS")) ?? 1;
                    retorno.TicketNumber = GetKeyOrEmpty(dictionary, "NROTICKET");
                    retorno.CardHolder = GetKeyOrEmpty(dictionary, "TITULAR");
                    retorno.AuthorizationCode = GetKeyOrEmpty(dictionary, "CODAUT");
                    retorno.Card4LastDigits = GetKeyOrEmpty(dictionary, "NROTARJ4");
                    retorno.CardMask = GetKeyOrEmpty(dictionary, "NROTARJ4");
                    retorno.Mail = GetKeyOrEmpty(dictionary, "MAIL");
                    retorno.OriginalCode = GetKeyOrEmpty(dictionary, "IDESTADO");
                    retorno.NroLote = GetKeyOrEmpty(dictionary, "LOTE");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error F24: Error al parsear respuesta XML de SPS.");
                queryResponse.HasError = true;
                queryResponse.ErrorInQuery = ex.Message;
            }
        }

        private void HandleRestResponse(RestResponse response, TransactionOriginal retorno, SyncroModel queryResponse)
        {
            if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
            {
                try
                {
                    SPSPaymentResponseModel resultados = JsonConvert.DeserializeObject<SPSPaymentResponseModel>(response.Content);
                    queryResponse.HasResponse = true;
                    if (resultados.results.Any())
                    {
                        if (resultados.results.Count > 1)
                        {
                            queryResponse.HasError = true;
                            queryResponse.HasTransaction = true;
                            queryResponse.ErrorInQuery = "Existe más de una transacción para este ID";
                        }
                        else
                        {
                            queryResponse.HasTransaction = true;
                            queryResponse.HasError = false;
                            Result resultado = resultados.results.FirstOrDefault();
                            retorno.Amount = resultado.amount;
                            retorno.AuthorizationCode = resultado.status_details.card_authorization_code;
                            retorno.Card4LastDigits = resultado.card_data.card_number.GetLast(4);
                            retorno.CardHolder = resultado.card_data.card_holder.name;
                            retorno.CardMask = resultado.card_data.card_number;
                            retorno.Mail = resultado.customer.email;
                            retorno.OriginalCode = resultado.status;
                            retorno.Payments = resultado.installments;
                            retorno.TicketNumber = resultado.status_details.ticket;
                        }
                    }
                    else
                    {
                        queryResponse.HasTransaction = false;
                        queryResponse.HasError = false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error F26: No se puede parsear el resultado de la API.");
                    queryResponse.HasResponse = false;
                    queryResponse.HasError = true;
                    queryResponse.HasTransaction = false;
                    queryResponse.ErrorInQuery = "No se puede parsear el resultado - " + ex.Message;
                }
            }
            else
            {
                queryResponse.HasResponse = false;
                queryResponse.HasTransaction = false;
                queryResponse.HasError = true;
                queryResponse.ErrorInQuery = $"STATUS CODE: {response.StatusCode}. RESPONSE STATUS: {response.ResponseStatus}";
            }
        }

        private string GetRequestLog(RestRequest request, Uri _uri)
        {
            var requestToLog = new { /* ... */ };
            return JsonConvert.SerializeObject(requestToLog);
        }

        private string GetResponseLog(RestResponse response)
        {
            var responseToLog = new { /* ... */ };
            return JsonConvert.SerializeObject(responseToLog);
        }

        private TransactionOriginal FixSPSBatch(TransactionOriginal input, int serviceid)
        {
            if (AppConfig.ActivateSPSBatchNumberFixed && DateTime.Now > AppConfig.FixedSPSBatchBeginDate)
            {
                try
                {
                    if (input.QueryResponse.HasTransaction && !input.QueryResponse.HasError)
                    {
                        if (AppConfig.servicesidWithConciliation.Contains(serviceid) &&
                            !string.IsNullOrEmpty(input.AuthorizationCode) &&
                            !string.IsNullOrEmpty(input.TicketNumber) &&
                            !string.IsNullOrEmpty(input.Card4LastDigits) &&
                            !string.IsNullOrEmpty(input.CardMask) &&
                            input.Card4LastDigits.Length == 4 &&
                            input.CardMask.Length > 12 &&
                            input.CardMask.Length < 19 &&
                            string.IsNullOrEmpty(input.NroLote))
                        {
                            input.NroLote = AppConfig.SPSBatchNumberFixed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error F28: Error en FixSPSBatch.");
                    return input;
                }
            }
            return input;
        }

        private string GetKeyOrEmpty(Dictionary<string, string> dictionary, string key)
        {
            return dictionary.TryGetValue(key, out string value) ? value : "";
        }
    }
}