using Newtonsoft.Json;
using NLog; 
using PGMainService.Models;
using PGMainService.PGDataAccess; 
using RestSharp; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection; 
using System.Runtime.CompilerServices; 
using System.Security.Claims; 
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 
using System.Web; 
using System.Web.Configuration;
using LogModel = PGMainService.Models.LogModel;

namespace PGMainService.Manager
{
    internal class Utils
    {
        public HeaderRequestModel GetDataFromHTTPRequest(HttpRequestHeaders header)
        {
            var returnHeaderRequest = new HeaderRequestModel
            {
                Language = "es",
                User = null, 
                HasAuthentication = false,
                IsAuthorized = false
            };

            try
            {
                // Lógica para obtener lenguaje.
                var firstLanguageHeader = header.AcceptLanguage.FirstOrDefault(); 
                if (firstLanguageHeader != null)
                    returnHeaderRequest.Language = firstLanguageHeader.ToString().Split('-')[0];
            }
            catch (Exception ex)
            {
                this.InsertLogException(LogType.Error, ex, nameof(GetDataFromHTTPRequest));
            }

            try
            {
                // Lógica para decodificar Basic Auth.
                if (header.TryGetValues("Authorization", out IEnumerable<string> authorizationValues) &&
                    authorizationValues.Any() &&
                    !string.IsNullOrEmpty(authorizationValues.First())) 
                {
                    string basicToken = authorizationValues.First();
                    string base64Credentials = new Regex("Basic ", RegexOptions.IgnoreCase).Replace(basicToken, "", 1);
                    byte[] credentialBytes = Convert.FromBase64String(base64Credentials);
                    string decodedCredentials = Encoding.UTF8.GetString(credentialBytes);

                    int separatorIndex = decodedCredentials.IndexOf(':');
                    if (separatorIndex >= 0)
                    {
                        string username = decodedCredentials.Substring(0, separatorIndex);
                        string password = decodedCredentials.Substring(separatorIndex + 1);
                        returnHeaderRequest.User = username;
                        returnHeaderRequest.HasAuthentication = true;
                        if (this.IsAuthorizedToPay(username, password))
                            returnHeaderRequest.IsAuthorized = true;
                    }
                    else
                    {
                        this.InsertLogCommon(LogType.Warning, "Authorization header malformed: ':' separator missing.");
                    }
                }
            }
            catch (Exception ex)
            {
                this.InsertLogException(LogType.Error, ex, nameof(GetDataFromHTTPRequest));
            }
            return returnHeaderRequest;
        }
        public HttpResponseMessage GetHTTPResponse(
          string pgCode, 
          string lang = "es",
          string responseType = "html", 
          string additionalInfo = "",
          string newUrl = "",
          long reference = 0) 
        {
            string actualContentType; 
            switch (responseType.ToLowerInvariant()) 
            {
                case "json":
                    actualContentType = "application/json";
                    break;
                case "html":
                default:
                    actualContentType = "text/html";
                    break;
            }

            var httpResponseMessage = new HttpResponseMessage(); 

            try
            {
                var responseModelData = this.GetResponse<HTTPResponseModel>("status", "GetHTTPResponseByStatusCodeOrPGCode", true, new HTTPResponseQueryModel()
                {
                    lang = lang,
                    pgcode = pgCode
                });

                if (responseModelData == null)
                { // Chequeo de nulidad
                    httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                    httpResponseMessage.Content = new StringContent("Failed to retrieve HTTP response model.", Encoding.UTF8, "text/plain");
                    this.InsertLogCommon(LogType.Error, $"GetHTTPResponse: GetResponse<HTTPResponseModel> returned null for PGCode: {pgCode}, Lang: {lang}");
                    return httpResponseMessage;
                }


                if (actualContentType == "application/json")
                {
                    var jsonOutput = new JSONResponse 
                    {
                        message = responseModelData.Message,
                        details = additionalInfo,
                        code = responseModelData.PG_Code
                    };
                    if (reference > 0L) // Incluye 'reference' si se pasó
                    {
                        jsonOutput.reference = "NPG-" + reference.ToString();
                    }
                    httpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(jsonOutput), Encoding.UTF8, actualContentType);
                }
 
                else // Para otros tipos (html, xml)
                {
                    // Desarrollo concatena: responseModelData.Message + additionalInfo
                    httpResponseMessage.Content = new StringContent(responseModelData.Message + additionalInfo, Encoding.UTF8, actualContentType);
                }

                httpResponseMessage.StatusCode = (HttpStatusCode)System.Enum.ToObject(typeof(HttpStatusCode), responseModelData.HTTPStatusCode);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Found && !string.IsNullOrEmpty(newUrl))
                {
                    httpResponseMessage.Headers.Location = new Uri(newUrl);
                }
            }
            catch (Exception ex)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
                this.InsertLogException(LogType.Error, ex, nameof(GetHTTPResponse));
            }
            return httpResponseMessage;
        }

        public string GetValidationResponse(string statusCode, string lang = "es", string additionalInfo = "") 
        {
            string responseMessage = "";
            try
            {
                responseMessage = this.GetResponse<string>("status", "GetValidationResponseByStatusCode", true, new HTTPResponseQueryModel()
                {
                    pgcode = statusCode,
                    lang = lang
                });
            }
            catch (Exception ex)
            {
                this.InsertLogException(LogType.Error, ex, nameof(GetValidationResponse)); 
            }
            return responseMessage;
        }

        internal void InsertLogException(LogType warning, FormatException ex, string v, string message)
        {
            throw new NotImplementedException();
        }

        // IsAuthorizedToPay
        private bool IsAuthorizedToPay(string username, string password)
        {
            try
            { 
                using (var dataContext = new PGDataServiceClient()) 
                {
                    var authenticatedUser = this.CheckCredential(username, password);
                    if (authenticatedUser != null)
                    {
                        List<RolModel> roles = dataContext.GetRolesByUsername(authenticatedUser.username).ToList();
                        List<string> rolshortname = new List<string>();
                        foreach (RolModel rol in roles)
                        {
                            rolshortname.Add(rol.shortName);
                        }
                        string[] rolarray = rolshortname.ToArray();
                        // TODO: falta desarrollo de roles
                        return true;
                    }
                    else 
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // CheckCredential
        private UserModel CheckCredential(string username, string password)
        {
            using (var dataContext = new PGDataServiceClient()) 
            {
                int userId = dataContext.LoginUser(username, password);
                return userId != 0 ? dataContext.GetUser(userId) : null;
            }
        }

        // GetTransactionResultFromJson.
        public CallbackToPostModel GetTransactionResultFromJson(string operationNumber, string urlWebService)
        {
            var model = new CallbackToPostModel(); 
                                                   
            using (var client = new HttpClient()) 
            {
                var task = client.GetAsync($"{urlWebService}?operationNumber={operationNumber}") 
                  .ContinueWith((taskwithresponse) =>
                  {
                      var response = taskwithresponse.Result;
                  var jsonStringTask = response.Content.ReadAsStringAsync();
                      jsonStringTask.Wait();
                      model = JsonConvert.DeserializeObject<CallbackToPostModel>(jsonStringTask.Result);
                  });
                task.Wait();
            }
            return model;
        }

        // PostCallbackToURL
        public string PostCallbackToURL(CallbackToPostModel callbackToPost, string urlWebService)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlWebService);
                client.DefaultRequestHeaders.Accept.Clear(); 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var postData = this.ToDictionary(callbackToPost); 
                HttpContent content = new FormUrlEncodedContent(postData);

                HttpResponseMessage response = client.PostAsync("", content).Result;

                return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "";
            }
        }

        internal void InsertLogCommon(LogType info, string v, string CallerMemberName, string CallerFilePath, int CallerLineNumber)
        {
            throw new NotImplementedException();
        }

        // PostCallback
        public string PostCallback(CallbackToPostModel callbackToPost)
        {
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", callbackToPost.ReturnUrl);

            var postData = this.ToDictionary(callbackToPost);

            foreach (KeyValuePair<string, string> kvp in postData)
            {
                if (kvp.Key != "ReturnUrl") 
                    sb.AppendFormat("<input type='hidden' name='{0}' value='{1}'>", kvp.Key, kvp.Value);
            }
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        // ToDictionary 
        internal Dictionary<string, string> ToDictionary(object obj) 
        {

            return obj.GetType().GetProperties()
                     .ToDictionary(prop => prop.Name,
                                   prop => prop.GetGetMethod()?.Invoke(obj, null)?.ToString() ?? "");
        }


        internal long InsertLogException(
          LogType logType,
          Exception logException,
          [CallerMemberName] string callerMemberName = "", 
          [CallerFilePath] string callerFilePath = "",     
          [CallerLineNumber] int callerLineNumber = 0,     
          string transactionNumber = "")                   
        {
            return this.InsertLog(this.MapToLogModel(logType, logException.Message, logException, callerMemberName, callerFilePath, callerLineNumber, transactionNumber));
        }

        internal long InsertLogCommon(
          LogType logType,
          string logMessage,
          Exception logException = null,
          [CallerMemberName] string callerMemberName = "", 
          [CallerFilePath] string callerFilePath = "",     
          [CallerLineNumber] int callerLineNumber = 0,    
          string transactionNumber = "")                   
        {
            return this.InsertLog(this.MapToLogModel(logType, logMessage, logException, callerMemberName, callerFilePath, callerLineNumber, transactionNumber));
        }

        internal LogModel MapToLogModel(
          LogType logType,
          string logMessage,
          Exception logException,
          string callerMemberName = "", 
          string callerFilePath = "",   
          int callerLineNumber = 0,  
          string transactionNumber = "") 
        {

            return new LogModel()
            {
                exception = logException,
                Type = logType,
                message = logMessage,
                module = $"CallerMemberName: {callerMemberName} - CallerFilePath {callerFilePath} - CallerLineNumber {callerLineNumber} - TN: {transactionNumber} "
            };
        }

        // PostPayment 
        internal PaymentResponseInternal PostPayment(PaymentInputModel transactionToPost, string validatorName) // (recibe ID)

        {
            string jsonResponseFromPlugin = this.PostObjectToWebServiceAsJSON(transactionToPost,
                validatorName, WebConfigurationManager.AppSettings["PluginMethod_paymentRequest"]);

            return jsonResponseFromPlugin == null ? null : JsonConvert.DeserializeObject<PaymentResponseInternal>(jsonResponseFromPlugin);
        }

        internal string PostString(object objectToPost, string validatorName, string method)
        {
            return this.PostObjectToWebServiceAsJSON(objectToPost, validatorName, method);
        }
        internal string PostObjectToWebServiceAsJSON(object objectToPost, string validatorName, string method)
        {
            using (var client = new HttpClient())
            {
                string pluginFullUrl = $"{WebConfigurationManager.AppSettings[$"Plugin_{validatorName}_URL"]}/{method}";
                if (string.IsNullOrEmpty(WebConfigurationManager.AppSettings[$"Plugin_{validatorName}_URL"]))
                { 
                    this.InsertLogCommon(LogType.Error, $"URL del plugin no encontrada para el validador: {validatorName}");
                    return null;
                }
                client.BaseAddress = new Uri(pluginFullUrl);
                client.DefaultRequestHeaders.Accept.Clear(); 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("", objectToPost).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    string errorContent = response.Content.ReadAsStringAsync().Result;
                    this.InsertLogCommon(LogType.Error, $"Error en comunicación con el Plugin {validatorName}. Status: {response.StatusCode}. Contenido: {errorContent}", null, nameof(PostObjectToWebServiceAsJSON));
                    return null; 
                }
            }
        }

        internal decimal ConvertStringToDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0M; // Chequeo de nulo/vacío.


            input = input.Replace(".", ","); 
            string numericStringOnly = Regex.Replace(input, @"[^\d,]", ""); // Quitar todo excepto dígitos y coma

            if (!numericStringOnly.Contains(","))
            {
                // Añadir "00" para centavos
                numericStringOnly += ",00";
            }

            string[] parts = numericStringOnly.Split(',');
            string integerPart = parts[0];
            string decimalPart = parts.Length > 1 ? parts[1] : "00";

            // Asegurar que la parte decimal tenga exactamente dos dígitos
            decimalPart = decimalPart.PadRight(2, '0').Substring(0, 2);

            string fullNumberString = integerPart + decimalPart;

            if (long.TryParse(fullNumberString, out long centsValue))
            {
                return decimal.Divide(centsValue, 100M); // Usar 100M
            }
            else
            {
                // Error en la conversión final, loguear y devolver 0 o lanzar excepción.
                this.InsertLogCommon(LogType.Error, $"Error al convertir la cadena de monto a long: '{fullNumberString}' (original: '{input}')", null, nameof(ConvertStringToDecimal));
                return 0M;
            }
        }

        internal string GetFirstValue(decimal? value) 
        {
            if (!value.HasValue) return "0"; 
     
            string[] parts = value.Value.ToString(CultureInfo.InvariantCulture).Split('.');
            return parts[0];
        }

        internal string GetSecondValue(decimal? value) 
        {
            if (!value.HasValue) return "00"; 
            string[] parts = value.Value.ToString(CultureInfo.InvariantCulture).Split('.');

            return parts.Length > 1 ? parts[1].PadRight(2, '0').Substring(0, 2) : "00";
        }

        public string GetRealResponse(string validatorURLToCheckTransaction, string transactionId)
        {
            string responseContent = null;
            try
            {
                string fullUrl = validatorURLToCheckTransaction + transactionId;
                using (var client = new HttpClient()) 
                {
                    client.BaseAddress = new Uri(fullUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("").Result; 
                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = response.Content.ReadAsAsync<string>().Result;
                    }
                }
            }
            catch (HttpRequestException ex) 
            {
                responseContent = null; 
            }
            return responseContent;
        }

        internal long InsertLog(LogModel logToInsert, bool fromAPIClient = false) 
        {
            try
            {
                string apiResponseJson = this.ConnectToLDAPI(Method.POST, "/log/add", JsonConvert.SerializeObject(logToInsert), fromAPIClient);

                if (string.IsNullOrEmpty(apiResponseJson) || apiResponseJson.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    // Si la API devuelve null, "false" o vacío, indica un error no excepcional en la API de log.
                    // Loguear a NLog como último recurso y no intentar más.
                    this.SaveNLog($"InsertLog: Fallo al insertar log via API para {JsonConvert.SerializeObject(logToInsert)}. Respuesta API: {apiResponseJson}");
                    return 0; // O un valor que indique fallo.
                }

                long logId = JsonConvert.DeserializeObject<long>(apiResponseJson);

                // Si la deserialización falla o devuelve 0, se considera error.
                if (logId > 0L)
                {
                    return logId;
                }
                else
                {
                    // Loguear a NLog si el ID es inválido pero la llamada API no falló catastróficamente.
                    this.SaveNLog($"InsertLog: API de log devolvió un ID inválido ({logId}) para {JsonConvert.SerializeObject(logToInsert)}");
                    return 0; // O un valor que indique fallo.
                }
            }
            catch (JsonException jsonEx) // Error al deserializar la respuesta de la API
            {
                this.SaveNLog($"InsertLog: Error de deserialización JSON de la respuesta de la API de log. Exception: {jsonEx.Message} para {JsonConvert.SerializeObject(logToInsert)}");
                return 0; // Indicar fallo.
            }
            catch (Exception ex) // Cualquier otra excepción (ej. fallo en ConnectToLDAPI)
            {
                this.SaveNLog($"InsertLog: Excepción general. Message: {ex.Message} para {JsonConvert.SerializeObject(logToInsert)}");
                // El throw original es "Error Irrecuperable: {ex.Message} ||| {ex.InnerException?.Message}"
                // Considerar si relanzar o solo loguear a NLog y devolver 0.
                // Relanzar puede ser apropiado si es un fallo crítico del sistema de log.
                throw new Exception($"Error Irrecuperable al intentar loguear: {ex.Message}", ex);
            }
        }

        private string ConnectToLDAPI(Method method, string urlRequest, string dataPosted = null, bool isLoggingError = false) 
        {
            // Lee de AppSettings, usa RestSharp.
            string pgdlBaseUrl = ConfigurationManager.AppSettings["PGDL_URL"]; 
            if (string.IsNullOrEmpty(pgdlBaseUrl))
            {
                this.SaveNLog("ConnectToLDAPI: PGDL_URL no está configurada en AppSettings.");
                return isLoggingError ? "false" : null;
            }

            var restClient = new RestClient(pgdlBaseUrl); 
            var restRequest = new RestRequest(urlRequest, method); 

            // Autenticación Basic
            string pgdlUser = ConfigurationManager.AppSettings["PGDL_User"];
            string pgdlPassword = ConfigurationManager.AppSettings["PGDL_Password"];
            if (string.IsNullOrEmpty(pgdlUser) || string.IsNullOrEmpty(pgdlPassword))
            {
                this.SaveNLog("ConnectToLDAPI: PGDL_User o PGDL_Password no están configurados.");
                return isLoggingError ? "false" : null;
            }
            string credentials = $"{pgdlUser}:{pgdlPassword}";
            string base64Auth = this.Base64Encode(credentials); 
            restRequest.AddHeader("Authorization", "Basic " + base64Auth);

            // Headers adicionales
            // Manejo de nulos para HttpContext.Current y sus propiedades.
            string absoluteUrl = "UnknownHost";
            try
            {
                absoluteUrl = HttpContext.Current?.Request?.Url?.GetLeftPart(UriPartial.Authority) ?? "UnknownHost";
            }
            catch { }
            restRequest.AddHeader("AbsoluteURL", absoluteUrl);
            restRequest.AddHeader("UserWorker", "PGMainServices"); // Hardcoded

            // Obtener UserId de Claims
            string currentUserId = "Anonymous"; 
            try
            {
                var claimsIdentity = ClaimsPrincipal.Current?.Identities?.FirstOrDefault(id => id.IsAuthenticated); // Buscar identidad autenticada
                currentUserId = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase))?.Value ?? "Anonymous";
            }
            catch { }
            restRequest.AddHeader("UserId", currentUserId);

            restRequest.RequestFormat = DataFormat.Json;

            if (method == Method.POST && dataPosted != null)
            {
                restRequest.AddParameter("application/json; charset=utf-8", dataPosted, ParameterType.RequestBody);
            }

            IRestResponse apiResponse = restClient.Execute(restRequest); 

            string logErrorMessage = ""; 
            bool hasCommunicationError = false; 

            if (apiResponse.ResponseStatus == ResponseStatus.Completed)
            {
                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    return apiResponse.Content; // Éxito
                }
                else
                {
                    hasCommunicationError = true;
                    logErrorMessage = $"Error en respuesta de API PGDL ({apiResponse.StatusCode})";
                }
            }
            else // Error de comunicación (Timeout, NameResolutionFailure, etc.)
            {
                hasCommunicationError = true;
                logErrorMessage = $"Error en comunicación con API PGDL ({apiResponse.ResponseStatus}): {apiResponse.ErrorMessage}";
            }

            if (hasCommunicationError)
            {
                string errorDetailsForLog = $"|| Status: {apiResponse.ResponseStatus} | Code: {apiResponse.StatusCode} | Method: {method} | BASE: {pgdlBaseUrl} | URL: {urlRequest} | DataPosted: {dataPosted} | Error: {apiResponse.ErrorMessage}";
                if (isLoggingError) // Si el error ocurrió mientras se intentaba loguear (evitar bucle)
                {
                    this.SaveNLog(logErrorMessage + errorDetailsForLog); // Loguear a NLog
                    return "false"; 
                }
                else // Error normal, intentar loguearlo usando el sistema estándar (que llama a ConnectToLDAPI)
                {
                    this.InsertLog(new LogModel()
                    {
                        Type = LogType.Error,
                        module = "APIClient_PGDL_Error", 
                        message = logErrorMessage,
                        exception = new Exception(errorDetailsForLog)
                    }, true); // 'true' indica que es un log desde el propio cliente API
                }
            }
            return null; // Indica fallo si no se retornó contenido OK antes
        }

        private string Base64Encode(string plainText)
        {
            if (plainText == null) return null; 
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
 
        private void SaveNLog(string textToLog) 
        {
            try
            {
                Logger nlogLogger = LogManager.GetCurrentClassLogger(); 
                nlogLogger.Fatal(textToLog); 
            }
            catch (Exception nlogException) 
            {
                System.Diagnostics.Debug.WriteLine($"FALLO CRÍTICO DE NLOG: {nlogException.Message}. Mensaje original: {textToLog}");
            }
        }

        //llama al DataLayer
        public T GetResponse<T>(string serviceGroup, string actionName, bool isPostRequest = false, object dataToPost = null) 
        {
            string apiResponseContent = null; 
            try
            {
                string apiUrl = $"{serviceGroup}/{actionName}"; 
                if (isPostRequest)
                {
                    apiResponseContent = this.ConnectToLDAPI(Method.POST, apiUrl, JsonConvert.SerializeObject(dataToPost));
                }
                else
                {
                    apiResponseContent = this.ConnectToLDAPI(Method.GET, apiUrl);
                }

                if (apiResponseContent != null && !apiResponseContent.Equals("false", StringComparison.OrdinalIgnoreCase)) 
                {
                    return JsonConvert.DeserializeObject<T>(apiResponseContent);
                }
            }
            catch (Exception ex)
            {
                this.InsertLogException(LogType.Error, ex, nameof(GetResponse));
            }
            return default(T); // Retorna default si hubo error o respuesta null/"false"
        }

        // Clase interna JSONResponse
        internal class JSONResponse 
        {
            public string message { get; set; }
            public string details { get; set; }
            public string code { get; set; }
            public string reference { get; set; }
        }

    }
}