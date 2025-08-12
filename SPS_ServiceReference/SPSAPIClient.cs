using Newtonsoft.Json;
using RestSharp;
using SPS_ServiceReference.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;

namespace SPS_ServiceReference
{
    public class SPSAPIClient
    {
        /*
        public SPSPaymentResponseModel GetResponse(SPSClientConfigurationModel config, string transactionId) {
            try
            {
                return JsonConvert.DeserializeObject<SPSPaymentResponseModel>(ConnectToSPSAPILog(Method.GET, config, transactionId).returnValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        public SPSPaymentResponseModelWithLog GetResponseWithLog(SPSClientConfigurationModel config, string transactionId)
        {
            SPSCommunicationAPI communication = new SPSCommunicationAPI();
            SPSPaymentResponseModelWithLog returnValue = new SPSPaymentResponseModelWithLog();
            try
            {
                communication = ConnectToSPSAPILog(Method.GET, config, transactionId);
                returnValue.LogRequest = communication.LogRequest;
                returnValue.LogResponse = communication.LogResponse;
                if (communication.resultCanBeParsed)
                {
                    returnValue.respuesta = JsonConvert.DeserializeObject<SPSPaymentResponseModel>(communication.returnValue);
                }
            }
            catch (Exception ex)
            {
                returnValue.LogResponse = ex.ToString();
            }
            return returnValue;
        }

        public string GetLinkHashedFromTransaction(SPSClientConfigurationModel config, SPSPaymentModel paymentToPost)
        {
            return config.urlbase + config.urlrequest + "/" + GetHashFromTransaction(config, paymentToPost) + "?apikey=" + paymentToPost.public_apikey;
        }

        public string GetHashFromTransaction(SPSClientConfigurationModel config, SPSPaymentModel paymentToPost)
        {
            try
            {
                return JsonConvert.DeserializeObject<SPSPaymentHashModelResponse>(ConnectToSPSAPILog(Method.POST, config, JsonConvert.SerializeObject(paymentToPost)).returnValue).hash;
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
                throw ex;
            }
        }

        private SPSCommunicationAPI ConnectToSPSAPILog(Method method, SPSClientConfigurationModel config, string dataposted = null)
        {
            SPSCommunicationAPI returnResponse = new SPSCommunicationAPI();
            returnResponse.resultCanBeParsed = false;
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(config.urlbase);
                if (config.ProxyConfiguration.ProxyActive)
                {
                    client.Proxy = GetWebProxy(config.ProxyConfiguration);
                }
                var request = new RestRequest(config.urlrequest, method);
                //string bup = Base64Encode(config.user + ":" + config.pass);
                //request.AddHeader("Authorization", "Basic " + bup);
                request.AddHeader("apikey", config.apikeyPrivate);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.RequestFormat = DataFormat.Json;

                switch (method)
                {
                    case Method.GET:


                        request.AddParameter("siteOperationId", dataposted);
                        request.AddParameter("expand", "card_data");
                        break;

                    case Method.POST:
                        request.AddParameter("application/json; charset=utf-8", dataposted, ParameterType.RequestBody);
                        break;
                }

                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                returnResponse.LogRequest = GetRequestLog(request, client.BuildUri(request));
                IRestResponse response = client.Execute(request);
                returnResponse.LogResponse = GetResponseLog(response);
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    //Socket OK 
                    if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Respuesta de la API OK
                        returnResponse.returnValue = response.Content;
                        returnResponse.resultCanBeParsed = true;
                        return returnResponse;
                        // return response.Content;
                    }
                    else
                    {
                        //Error con contenido de la API
                        throw new Exception("Error comunicacion con SPS-API - NO STATUS CODE OK: " + response.StatusCode + " - " + response.Content + " !! " + dataposted);
                    }

                }
                else
                {
                    //Error en la comunicacion con la API
                    throw new Exception("Error comunicacion con SPS-API: NO RESPONSE COMPLETED " + response.StatusCode + " - " + response.Content + " !! " + dataposted);
                }
            }
            catch (HttpRequestException ex) when (ex.InnerException is AuthenticationException)
            {
                returnResponse.LogResponse = "Error de TLS: " + ex.InnerException.Message;
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                returnResponse.LogResponse = "Error de TLS: " + ex.Message;
                throw ex;
            }
            catch (Exception ex)
            {
                returnResponse.LogResponse = ex.Message;
                throw ex;
            }
            return returnResponse;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static WebProxy GetWebProxy(ProxySettings proxySettings)
        {
            WebProxy proxy;
            try
            {
                proxy = new WebProxy(proxySettings.Server, Int32.Parse(proxySettings.Port))
                {
                    Credentials = new NetworkCredential(proxySettings.Username, proxySettings.Password, proxySettings.Domain)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
            return proxy;
        }

        private static string GetRequestLog(IRestRequest request, Uri _uri)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                method = request.Method.ToString(),
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
