using CookComputing.XmlRpc;
using Newtonsoft.Json;
using PGPluginSPS.Models;
using PGPluginSPS.PGDataAccess;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace PGPluginSPS.Utils
{

    public static class Tools
    {

        public static string ObjectToString(object myObj, string separator)
        {
            return DictionaryToString(ToDictionary(myObj), separator);
        }

        public static Dictionary<string, string> ToDictionary(object myObj)
        {
            return
                (from x in myObj.GetType().GetProperties() select x).ToDictionary(x => x.Name,
                    x =>
                        (x.GetGetMethod().Invoke(myObj, null) == null
                            ? ""
                            : x.GetGetMethod().Invoke(myObj, null).ToString()));
        }

        public static string DictionaryToString(Dictionary<string, string> dictionary, string separator)
        {

            string retorno = "";

            foreach (KeyValuePair<string, string> data in dictionary)
            {
                retorno = data.Key + separator + data.Value + separator;
            }
            retorno = retorno.Remove(retorno.Length - separator.Length);
            return retorno;

        }

    }

    public class SPSWebServiceResponse
    {
        public string idResultado { get; set; }
        public string idEstado { get; set; }
        public string idMotivo { get; set; }
        public string idOperacion { get; set; }
        public string originalRequest { get; set; }
    }

    public class SPSWebServiceRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Idsite { get; set; }
        public string Nrooperacion { get; set; }
        public string Importe { get; set; }
    }

    public class WebServiceInterface
    {
        public enum Operation
        {
            GetComplete,
            Query,
            Annulment,
            TotalRefund,
            PartialRefund
        }

        public WebProxy getWebProxy()
        {
            //<!-- Web Proxy Server -->
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

        public decimal ConvertStringToDecimal(string input)
        {

            string S_centavos = "0";
            //Ajusto String
            input = input.Replace(".", ",");

            if (!input.Contains(","))
            {
                S_centavos = input + "00";
            }
            else
            {
                int posicion_ultima_coma = input.LastIndexOf(",");
                string integerPart = input.Substring(0, posicion_ultima_coma);
                string decimalPart = input.Substring(posicion_ultima_coma, input.Length - posicion_ultima_coma);
                integerPart = integerPart.Replace(",", "");
                decimalPart = decimalPart.Replace(",", "");
                switch (decimalPart.Length)
                {
                    case 0:
                        decimalPart = decimalPart + "00";
                        break;
                    case 1:
                        decimalPart = decimalPart + "0";
                        break;
                    default:
                        break;
                }
                S_centavos = integerPart + decimalPart;
            }

            long centavos = Convert.ToInt64(S_centavos);
            decimal retorno = Decimal.Divide(centavos, 100);
            return retorno;

        }
        public int? ToNullableInt32(string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return null;
        }

        public CallbackResponse GetCompleteData(SPSWebServiceRequest requestData)
        {
            var responseToReturn = new CallbackResponse();
            string soapStr = string.Format(
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
                </soap:Envelope>",
                requestData.Idsite, requestData.Nrooperacion
            );

            string MethodName = "Get";
            string Url = WebConfigurationManager.AppSettings["SPS_Operation_GetData_URL"];

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Proxy = getWebProxy();

            req.Headers.Add("Authorization: " + WebConfigurationManager.AppSettings["SPS_Operation_GetData_Authorization"]);
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

            try
            {
                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string result = responseReader.ReadToEnd();

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        LogTool.InsertLogCommon(LogTypeModel.Warning, "Respuesta SOAP vacía en GetCompleteData.", CallerMemberName: nameof(GetCompleteData));
                        return null;
                    }

                    if (!result.Contains("IDTRANSACCIONSITE"))
                    {
                        LogTool.InsertLogCommon(LogTypeModel.Warning, "Respuesta SOAP no contiene IDTRANSACCIONSITE.", CallerMemberName: nameof(GetCompleteData));
                        return null;
                    }

                    XDocument ResultXML = XDocument.Parse(result);

                    var dictionary = new Dictionary<string, string>();
                    XNamespace xmlns = "urn:Decidir.net";
                    IEnumerable<XElement> list = ResultXML.Document.Descendants(xmlns + "Operation");
                    if (list.Count() > 0)
                    {
                        foreach (var element in list)
                        {
                            if (element.HasElements)
                            {
                                foreach (XElement ala in element.Descendants())
                                {
                                    var key = ala;
                                    var name = key.Name.LocalName;

                                    string newvalue = (string)key.Value;
                                    dictionary[name] = newvalue;
                                }

                                responseToReturn.NOperacion = dictionary["IDTRANSACCIONSITE"];
                                DateTime? fechaoriginal = new DateTime();
                                if (string.IsNullOrWhiteSpace(dictionary["FECHA_ORIGINAL"]))
                                {
                                    fechaoriginal = null;
                                }
                                else
                                {
                                    DateTime.ParseExact(dictionary["FECHA_ORIGINAL"], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                responseToReturn.FechaHora = fechaoriginal;
                                responseToReturn.Monto = ConvertStringToDecimal(dictionary["MONTO"]);
                                responseToReturn.Cuotas = ToNullableInt32(dictionary["CUOTAS"]);
                                responseToReturn.NroTicket = dictionary["NROTICKET"];
                                responseToReturn.EstadoEntrega = dictionary["ESTADO_DESCRI"];
                                responseToReturn.IdMotivo = HttpUtility.HtmlDecode(dictionary["IDMOTIVO"]);
                                responseToReturn.MotivoAdicional = dictionary["MOTIVO_ADICIONAL"]; //falta
                                responseToReturn.Titular = dictionary["TITULAR"];
                                responseToReturn.TipoDoc = dictionary["IDTIPODOC"];
                                responseToReturn.NroDoc = ToNullableInt32(dictionary["NRODOC"]);
                                responseToReturn.TipoDocDescri = dictionary["TIPODOC"];
                                responseToReturn.CodAutorizacion = dictionary["CODAUT"];
                                responseToReturn.NroTarjetaVisible = dictionary["NROTARJ4"];
                                responseToReturn.Motivo = dictionary["MOTIVO"];
                                responseToReturn.EmailComprador = dictionary["MAIL"];
                                responseToReturn.Resultado = dictionary["IDESTADO"]; //ELEMENTO RESULTADO
                                responseToReturn.Tarjeta = dictionary["IDMEDIOPAGO"];
                                responseToReturn.Direccionentrega = dictionary["CALLE"] + " " + dictionary["NROPUERTA"];
                                responseToReturn.ParamSitio = dictionary["PARAMSITIO"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTool.InsertLogException(LogTypeModel.Error, ex);
                return null;
            }
            return responseToReturn;
        }

        public SPSWebServiceResponse Communicate(Operation operationToPerform, SPSWebServiceRequest requestData)
        {
            SPSWebServiceResponse respuesta = null;
            try
            {

                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                string serviceUrl = WebConfigurationManager.AppSettings["SPSWebService_SacXmlRpcServer"];
                bool certificateOn = WebConfigurationManager.AppSettings["CertificateOn"] == "on";
                string certificatePath = WebConfigurationManager.AppSettings["CertificatePath"];

                switch (operationToPerform)
                {

                    case Operation.Query:

                        IgetEstadoTransaccion proxyQ = XmlRpcProxyGen.Create<IgetEstadoTransaccion>();
                        proxyQ.Proxy = getWebProxy();
                        XmlRpcClientProtocol cpQ = (XmlRpcClientProtocol)proxyQ;
                        cpQ.Url = serviceUrl;
                        if (certificateOn)
                        {
                            cpQ.ClientCertificates.Add(new X509Certificate(certificatePath));
                        }
                        cpQ.KeepAlive = false;

                        string querytrans = proxyQ.getEstadoTransaccion(requestData.Username, requestData.Password, requestData.Idsite, requestData.Nrooperacion);
                        respuesta = GetRespuestaFromXML(querytrans);

                        break;
                    case Operation.Annulment:
                        IanularTransaccion proxyA = XmlRpcProxyGen.Create<IanularTransaccion>();
                        proxyA.Proxy = getWebProxy();
                        XmlRpcClientProtocol cpA = (XmlRpcClientProtocol)proxyA;
                        cpA.Url = serviceUrl;
                        if (certificateOn)
                        {
                            cpA.ClientCertificates.Add(new X509Certificate(certificatePath));
                        }
                        cpA.KeepAlive = false;
                        respuesta = GetRespuestaFromXML(proxyA.anularTransaccion(requestData.Username, requestData.Password, requestData.Idsite, requestData.Nrooperacion));

                        break;
                    case Operation.TotalRefund:
                        IdevolverTransaccionAcreditada proxyTR = XmlRpcProxyGen.Create<IdevolverTransaccionAcreditada>();
                        proxyTR.Proxy = getWebProxy();
                        XmlRpcClientProtocol cpTR = (XmlRpcClientProtocol)proxyTR;
                        cpTR.Url = serviceUrl;
                        if (certificateOn)
                        {
                            cpTR.ClientCertificates.Add(new X509Certificate(certificatePath));
                        }
                        cpTR.KeepAlive = false;
                        respuesta = GetRespuestaFromXML(proxyTR.devolverTransaccionAcreditada(requestData.Username, requestData.Password, requestData.Idsite, requestData.Nrooperacion));

                        break;
                    case Operation.PartialRefund:
                        IdevolverTransaccion proxyPR = XmlRpcProxyGen.Create<IdevolverTransaccion>();
                        proxyPR.Proxy = getWebProxy();
                        XmlRpcClientProtocol cpPR = (XmlRpcClientProtocol)proxyPR;
                        cpPR.Url = serviceUrl;
                        if (certificateOn)
                        {
                            cpPR.ClientCertificates.Add(new X509Certificate(certificatePath));
                        }
                        cpPR.KeepAlive = false;
                        respuesta = GetRespuestaFromXML(proxyPR.devolverTransaccion(requestData.Username, requestData.Password, requestData.Idsite, requestData.Nrooperacion, requestData.Importe));

                        break;
                }
            }
            catch (Exception excommunicate)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogTool.InsertLogException(LogTypeModel.Error, excommunicate).ToString());
            }
            return respuesta;
        }

        public T GetResponse<T>(string controllerName, string actionName, bool isPostRequest = false, object dataToPost = null)
        {
            try
            {
                string jsonData = isPostRequest && dataToPost != null ? JsonConvert.SerializeObject(dataToPost) : null;
                string apiUrlPath = $"{controllerName}/{actionName}";

                string jsonResponse = ConnectToLDAPI(isPostRequest ? Method.POST : Method.GET, apiUrlPath, jsonData);

                if (jsonResponse != null && jsonResponse != "false")
                {
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }
            }
            catch (JsonException jsonEx)
            {
                LogTool.InsertLogException(LogTypeModel.Error, jsonEx, $"{nameof(GetResponse)} - JSON Deserialization Error");
            }
            catch (Exception ex)
            {
                LogTool.InsertLogException(LogTypeModel.Error, ex, nameof(GetResponse));
            }
            return default(T);
        }

        private SPSWebServiceResponse GetRespuestaFromXML(string xmlToParse)
        {
            //ToDo: Save Response in Communications.
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlToParse);
            var nodes = xmlDoc.SelectNodes("/Respuesta");

            SPSWebServiceResponse resp = new SPSWebServiceResponse();

            foreach (XmlNode childrenNode in nodes)
            {
                resp.idResultado = (childrenNode.SelectSingleNode("IdResultado")?.FirstChild)?.Value;
                resp.idEstado = null;
                resp.idOperacion = null;
                resp.idMotivo = null;
                if (resp.idResultado == "0")
                {
                    resp.idEstado = childrenNode.SelectSingleNode("IdEstado").FirstChild.Value;
                    resp.idOperacion = (childrenNode.SelectSingleNode("idOperacion") != null) ? childrenNode.SelectSingleNode("idOperacion").FirstChild.Value : "";
                    resp.idMotivo = (childrenNode.SelectSingleNode("idMotivo") != null) ? childrenNode.SelectSingleNode("idMotivo").FirstChild.Value : "";
                }
            }
            resp.originalRequest = xmlToParse;
            return resp;
        }

        internal string ConnectToLDAPI(Method httpMethod, string urlRequest, string dataPosted = null, bool isLoggingError = false)
        {
            string baseApiUrl = ConfigurationManager.AppSettings["PGDL_URL"];
            var restClient = new RestClient(baseApiUrl);
            var restRequest = new RestRequest(urlRequest, httpMethod);

            string authCredentials = $"{ConfigurationManager.AppSettings["PGDL_User"]}:{ConfigurationManager.AppSettings["PGDL_Password"]}";
            string base64AuthHeader = Base64Encode(authCredentials);

            restRequest.AddHeader("Authorization", "Basic " + base64AuthHeader);

            // Headers adicionales Manejo de nulos para HttpContext.Current y sus propiedades.
            string absoluteUrl = "UnknownHost";
            if (HttpContext.Current?.Request?.Url != null)
            {
                absoluteUrl = HttpContext.Current?.Request?.Url?.GetLeftPart(UriPartial.Authority) ?? "UnknownHost";
                restRequest.AddHeader("AbsoluteURL", absoluteUrl);
            }
            restRequest.AddHeader("UserWorker", "PGPluginSPS");
            string currentUserId = "Anonymous";
            if (HttpContext.Current?.User?.Identity is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated) // Chequeo de nulos y autenticación
            {
                currentUserId = claimsIdentity.Claims.FirstOrDefault(c => c.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase))?.Value ?? "Anonymous";
            }
            restRequest.AddHeader("UserId", currentUserId);

            restRequest.RequestFormat = DataFormat.Json;

            if (httpMethod == Method.POST && dataPosted != null)
            {
                restRequest.AddParameter("application/json; charset=utf-8", dataPosted, ParameterType.RequestBody);
            }

            IRestResponse apiResponse = restClient.Execute(restRequest);

            if (apiResponse.ResponseStatus == ResponseStatus.Completed && apiResponse.StatusCode == HttpStatusCode.OK)
            {
                return apiResponse.Content;
            }
            else
            {
                // Error en la comunicación o respuesta
                string errorMessage = apiResponse.ResponseStatus != ResponseStatus.Completed ? "Error en comunicación API" : "Error en respuesta API";
                string errorDetails = string.Format("|| Status: {0} | Code: {1} | Method: {2} | BASE: {5} | URL: {4} | DataPosted: {3} | ErrorMsg: {6}",
                    apiResponse.ResponseStatus, apiResponse.StatusCode, httpMethod, dataPosted,
                    urlRequest, baseApiUrl, apiResponse.ErrorMessage ?? apiResponse.StatusDescription);

                if (isLoggingError)
                {
                    //this.SaveNLog(logErrorMessage + errorDetailsForLog); falta loguear error
                    return "false";
                }
                else
                {
                    LogTool.InsertLogCommon(LogTypeModel.Error, "APIClient(ConnectToLDAPI)");
                }
                return null;
            }
        }

        private string Base64Encode(string plainText)
        {
            if (plainText == null) return null;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }

    public class TrustAllCertificatePolicy : ICertificatePolicy
    {
        public TrustAllCertificatePolicy() { }
        public bool CheckValidationResult(ServicePoint sp,
           X509Certificate cert,
           WebRequest req,
           int problem)
        {
            return true;
        }
    }

    [XmlRpcUrl("https://sps433.decidir.net/sps-ar/SacXmlRpcServer")]
    public interface IgetEstadoTransaccion : IXmlRpcProxy
    {
        [XmlRpcMethod]
        string getEstadoTransaccion(string username, string Password, string Idsite, string Nrooperacion);
    }

    [XmlRpcUrl("https://sps433.decidir.net/sps-ar/SacXmlRpcServer")]
    public interface IanularTransaccion : IXmlRpcProxy
    {
        [XmlRpcMethod]
        string anularTransaccion(string username, string Password, string Idsite, string Nrooperacion);

    }

    [XmlRpcUrl("https://sps433.decidir.net/sps-ar/SacXmlRpcServer")]
    public interface IdevolverTransaccionAcreditada : IXmlRpcProxy
    {
        [XmlRpcMethod]
        string devolverTransaccionAcreditada(string username, string Password, string Idsite, string Nrooperacion);

    }

    //ToDo: Devoluciones Parciales (to 3.5)
    [XmlRpcUrl("https://sps433.decidir.net/sps-ar/SacXmlRpcServer")]
    public interface IdevolverTransaccion : IXmlRpcProxy
    {
        [XmlRpcMethod]
        string devolverTransaccion(string username, string Password, string Idsite, string Nrooperacion, string importe);

    }


}