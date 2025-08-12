using PGMainService.Manager;
using PGMainService.Models;
using PGMainService.PGDataAccess;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using LogModel = PGMainService.Models.LogModel;

namespace PGMainService.Controllers
{

    [AllowAnonymous]
    public class TransactionTerminalController : ApiController
    {
        private Utils _utilities = new Utils();

        /// <summary>
        /// Devuelve el estado de salud del servicio de Callback. También cierra respuesta.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Service Online. ACK-EOT.")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        /// <summary>
        /// Obtiene operationCode para devolver estado de la transacción (solo para SPS).
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get(string operationCode)
        {
            return this.GetResponseToFinishTransaction(operationCode, 3);
        }

        /// <summary>
        /// POST Method to Callback for NPS.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Post(TransactionTerminalModel transactionTerminal)
        {
            return this.GetResponseToFinishTransaction(transactionTerminal.psp_TransactionId, 1);
        }

        // Método privado GetResponseToFinishTransaction
        private HttpResponseMessage GetResponseToFinishTransaction(string transactionId, int validatorId)
        {

            var serviceResponse = new HttpResponseMessage();

            try
            {
                using (new PGDataServiceClient())
                {
                    var initialTransactionResult = GetTransactionResult(transactionId, "es", validatorId);

                    if (initialTransactionResult == null)
                        return Request.CreateResponse(HttpStatusCode.NotFound);

                    // Condición (GenericCode != "OK") 
                    if (initialTransactionResult.TransactionCompletedInfo.TransactionCompletedStatus.GenericCode != "OK")
                    {
                        this._utilities.PostString(transactionId, initialTransactionResult.TransactionCompletedInfo.Validator, WebConfigurationManager.AppSettings["PluginMethod_transactionInfo"]);
                        var finalTransactionResult = GetTransactionResult(transactionId, "es", validatorId);

                        // Es crucial chequear si finalTransactionResult o su TransactionCompletedInfo son null antes de usarlos.
                        if (finalTransactionResult == null || finalTransactionResult.TransactionCompletedInfo == null)
                        {
                            this._utilities.InsertLog(new LogModel()
                            {
                                Type = LogType.Error,
                                module = nameof(GetResponseToFinishTransaction),
                                message = $" Falla al obtener el resultado final de la transaction para {transactionId} despues del post plugin."
                            });
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error al obtener el resultado final de la transaction .");
                        }

                        var callbackConfig = GetCallBackToClient(finalTransactionResult.TransactionCompletedInfo.Id);

                        long ticketApiResponse = this._utilities.GetResponse<long>("notification", "SendQuickTicketOfPayment/" + finalTransactionResult.TransactionIdPk);
                        if (ticketApiResponse > 0)
                            this._utilities.InsertLog(new LogModel()
                            {
                                Type = LogType.Error,
                                module = nameof(GetResponseToFinishTransaction),
                                transaction = finalTransactionResult.TransactionIdPk,
                                message = "SendQuickTicketOfPayment returns " + ticketApiResponse.ToString()
                            });

                        if (callbackConfig == null)
                        {
                            this._utilities.InsertLog(new LogModel()
                            {
                                Type = LogType.Warning,
                                module = nameof(GetResponseToFinishTransaction),
                                transaction = finalTransactionResult.TransactionIdPk,
                                message = "Callback configuration not found for transaction. Returning empty OK."
                            });
                            // Si no hay config de callback, retorna 'finishTransaction' (vacía, OK 200).
                            return serviceResponse;
                        }

                        if (callbackConfig.IsCallbackPosted)
                        {
                            var postDataForCallback = GenerateCallbackToPost(finalTransactionResult.TransactionCompletedInfo);
                            serviceResponse.Content = new StringContent(this._utilities.PostCallback(postDataForCallback));
                            serviceResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                        }
                        else
                        {
                            // Código HTTP: usa MovedPermanently (301), Se usa 302 (Moved).
                            var redirectResponse = Request.CreateResponse(HttpStatusCode.Moved);
                            redirectResponse.Headers.Location = new Uri(callbackConfig.ReturnUrl);
                            return redirectResponse;
                        }
                    }
                    // Si GenericCode ya era "OK", se retorna 'serviceResponse' (vacía, OK 200).
                }
            }
            catch (FormatException ex)
            {
                this._utilities.InsertLogException(LogType.Warning, ex, nameof(GetResponseToFinishTransaction), message: "Error de formato en TransactionID.");
            }
            catch (Exception ex)
            {
                string innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "N/A";
                this._utilities.InsertLogException(LogType.Error, (FormatException)ex, nameof(GetResponseToFinishTransaction), message: $"Error al finalizar la transaction. Inner: {innerExceptionMessage}");
                throw;
            }
            return serviceResponse;
        }

        // Método Estático GetCallBackToClient
        private static CallbackModel GetCallBackToClient(long transactionPk)
        {
            using (var context = new PGDataServiceClient())
            {
                CallbackModel callback = context.GetCallback(transactionPk);
                return callback;
            }
        }

        // Método Estático GetTransactionResult
        private static TransactionCompletedInfoWithPk GetTransactionResult(
          string transactionId,
          string language,
          int validatorId)
        {
            using (var context = new PGDataServiceClient())
            {
                var resultContainer = new TransactionCompletedInfoWithPk();
                var transactionModel = context.GetTransactionByValidatorTransactionId(transactionId, validatorId);

                if (transactionModel == null)
                    return null;

                resultContainer.TransactionIdPk = transactionModel.Id;
                resultContainer.TransactionCompletedInfo = context.GetTransactionResult(transactionModel.Id, language);

                if (resultContainer.TransactionCompletedInfo == null)
                {
                    return null;
                }
                return resultContainer;
            }
        }

        // Método Estático GenerateCallbackToPost
        private static CallbackToPostModel GenerateCallbackToPost(
          TransactionCompletedInfo transactionResultData)
        {
            var callback = new CallbackToPostModel()
            {
                ElectronicPaymentCode = transactionResultData.ElectronicPaymentCode,
                ResponseGenericCode = transactionResultData.TransactionCompletedStatus.GenericCode,
                ResponseGenericMessage = transactionResultData.TransactionCompletedStatus.GenericMessage,
                ResponseCode = transactionResultData.TransactionCompletedStatus.ResponseCode,
                ResponseMessage = transactionResultData.TransactionCompletedStatus.ResponseMessage,
                ResponseExtended = transactionResultData.TransactionCompletedStatus.ResponseMessage,
                ReturnUrl = transactionResultData.CallbackUrl,
                TransactionId = transactionResultData.TransactionId
            };
            return callback;
        }
    }
}
