using Newtonsoft.Json;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PGSyncro.Validators
{
    public static class SPSCalls
    {
        public static TransactionOriginal GetXMLService(GetTransactionsToSync_Result transaction)
        {
            TransactionOriginal retorno = new TransactionOriginal();
            SyncroModel QueryResponse = new SyncroModel
            {
                ServiceType = "DECIDIR XML-RPC"
            };
            retorno.ModuleType = "webservice";
            retorno.TransactionIdPK = transaction.TransactionIdPK;
            try
            {

                string soapStr =
                                       @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                                xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                                xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soap:Body>
                                <p:Get xmlns:p=""urn:Decidir.net"">
                                <xs:IDSITE xmlns:xs=""urn:Decidir.net"">{0}</xs:IDSITE>
                                <xs:IDTRANSACTIONSIT xmlns:xs=""urn:Decidir.net"">{1}</xs:IDTRANSACTIONSIT>
                                </p:Get>
                                </soap:Body>
                                </soap:Envelope>";

                soapStr = string.Format(soapStr, transaction.UniqueCode, transaction.TransactionNumber);
                string MethodName = "Get";
                string Url = ConfigurationManager.AppSettings["SPS_Operation_GetData_URL"];
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Proxy = NetworkTools.GetWebProxy();

                req.Headers.Add("Authorization: " + ConfigurationManager.AppSettings["SPS_Operation_GetData_Authorization"]);
                req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + MethodName + "\"");
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";

                using (Stream stm = req.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soapStr);
                    }
                }
                QueryResponse.RequestLog = soapStr;
                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string result = responseReader.ReadToEnd();
                    QueryResponse.ResponseLog = result;

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        // return null;
                        QueryResponse.HasResponse = false;
                        QueryResponse.HasTransaction = false;
                    }

                    if (!result.Contains("IDTRANSACCIONSITE"))
                    {
                        //   return null;
                        QueryResponse.HasResponse = true;
                        QueryResponse.HasTransaction = false;
                        
                    }
                    else
                    {
                        try
                        {
                            QueryResponse.HasResponse = true;
                            QueryResponse.HasTransaction = true;
                            XDocument ResultXML;
                            ResultXML = XDocument.Parse(result);

                            var dictionary = new Dictionary<string, string>();
                            XNamespace xmlns = "urn:Decidir.net";
                            IEnumerable<XElement> list = ResultXML.Document.Descendants(xmlns + "Operation");
                            if (list.Count() > 0)
                            {

                                foreach (var element in list)
                                {
                                    if (element.HasElements)
                                    {
                                        IEnumerable<XElement> aaa = element.Descendants();
                                        foreach (XElement ala in aaa)
                                        {
                                            var key = ala;
                                            var name = key.Name.LocalName;

                                            string newvalue = (string)key.Value;
                                            dictionary[name] = newvalue;



                                        }
                                        retorno.Amount = Convert.ToInt64(ConversionTools.ConvertStringToDecimal(ConversionTools.GetKeyOrEmpty(dictionary, "MONTO")) * 100);
                                        retorno.Payments = ConversionTools.ToNullableInt32(ConversionTools.GetKeyOrEmpty(dictionary, "CUOTAS")) ??1;
                                        retorno.TicketNumber = ConversionTools.GetKeyOrEmpty(dictionary, "NROTICKET");
                                       retorno.CardHolder = ConversionTools.GetKeyOrEmpty(dictionary, "TITULAR"); 
                                       retorno.AuthorizationCode = ConversionTools.GetKeyOrEmpty(dictionary, "CODAUT"); 
                                       retorno.Card4LastDigits = ConversionTools.GetKeyOrEmpty(dictionary, "NROTARJ4"); 
                                        retorno.CardMask = ConversionTools.GetKeyOrEmpty(dictionary, "NROTARJ4");
                                        retorno.Mail = ConversionTools.GetKeyOrEmpty(dictionary, "MAIL"); 
                                       retorno.OriginalCode = ConversionTools.GetKeyOrEmpty(dictionary, "IDESTADO"); //ELEMENTO RESULTADO
                                        retorno.NroLote = ConversionTools.GetKeyOrEmpty(dictionary,"LOTE");

                                    }
                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            QueryResponse.HasResponse = true;
                            QueryResponse.HasTransaction = true;
                            QueryResponse.HasError = true;
                            QueryResponse.ErrorInQuery = ex.Message;
                         
                        }

                       // return responseToReturn;


                    }


                }
            }
            catch (Exception ex) {
                QueryResponse.HasError = true;
                QueryResponse.ErrorInQuery = ex.Message;
            }

            retorno.QueryResponse = QueryResponse;
            return retorno;


        }

        public static TransactionOriginal GetAPIService (GetTransactionsToSync_Result transaction, string apikeyPrivate)
        {
            TransactionOriginal retorno = new TransactionOriginal();
            SyncroModel QueryResponse = new SyncroModel
            {
                ServiceType = "DECIDIR API 2.0"
            };
            retorno.ModuleType = "api";
            retorno.TransactionIdPK = transaction.TransactionIdPK;

            try
            {
                string urlbase = Program.SPS_API_BASE;
                string urlrequest = Program.SPS_API_TRANSACTION;

                  System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(urlbase)
                {
                    Proxy = NetworkTools.GetWebProxy()
                };

                var request = new RestRequest(urlrequest, Method.GET);
                request.AddHeader("apikey", apikeyPrivate);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.RequestFormat = DataFormat.Json;
                        request.AddParameter("siteOperationId", transaction.TransactionNumber.ToString());
                        request.AddParameter("expand", "card_data");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                QueryResponse.RequestLog = GetRequestLog(request, client.BuildUri(request));
                IRestResponse response = client.Execute(request);
                QueryResponse.ResponseLog = GetResponseLog(response);


                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    //Socket OK 
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        try
                        {
                            SPSPaymentResponseModel resultados = JsonConvert.DeserializeObject<SPSPaymentResponseModel>(response.Content);
                            QueryResponse.HasResponse = true;
                            if (resultados.results.Any())
                            {
                                if (resultados.results.Count() > 1) {
                                    QueryResponse.HasError = true;
                                    QueryResponse.ErrorInQuery = "Existe más de una transacción para este ID";
                                } else {
                                    //OK
                                    QueryResponse.HasTransaction = true;
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
                                    retorno.OriginalCode = resultado.status;
  
                                }
                            }
                            else {
                                QueryResponse.HasTransaction = false;
                                QueryResponse.HasError = false;
                            }

                        }
                        catch (Exception ex) {
                            QueryResponse.HasResponse = false;
                            QueryResponse.HasError = true;
                            QueryResponse.HasTransaction = false;
                            QueryResponse.ErrorInQuery = "No se puede parsear el resultado - " + ex.Message;
                        }

                    }
                    else
                    {
                        QueryResponse.HasResponse = false;
                        QueryResponse.HasTransaction = false;
                        QueryResponse.HasError = true;
                        QueryResponse.ErrorInQuery = "STATUS CODE: " + response.StatusCode.ToString();


                            }

                }
                else
                {
                    QueryResponse.HasResponse = false;
                    QueryResponse.HasTransaction = false;
                    QueryResponse.HasError = true;
                    QueryResponse.ErrorInQuery = "RESPONSE STATUS: " + response.ResponseStatus.ToString();

                }
            }
            catch (Exception ex)
            {
                QueryResponse.HasTransaction = false;
                QueryResponse.HasError = true;
                QueryResponse.ErrorInQuery = ex.Message;
            }

            retorno.QueryResponse = QueryResponse;
            return retorno;
        }


        private static string GetRequestLog(IRestRequest request, Uri _uri)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = _uri // _restClient.BuildUri(request),
            };


            return JsonConvert.SerializeObject(requestToLog);
        }

        private static string GetResponseLog(IRestResponse response)
        {
            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };
            return JsonConvert.SerializeObject(responseToLog);
        }

    }
}
