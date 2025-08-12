using PGPluginSPS.Models;
using PGPluginSPS.PGDataAccessService;
using PGPluginSPS.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGPluginSPS.Controllers
{
    public class CallbackController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "CallbackSPS - Service Online.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spsCallbackResponse"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]CallbackResponse spsCallbackResponse)
        {
            try
            {
                using (PGDataServiceClient _dataContext = new PGDataServiceClient())
                {
                    _dataContext.InsertLog(LogTypeModel.Info, "PGPluginSPS/Controllers/CallbackController/Post/Step1", spsCallbackResponse.ToString(), string.Empty);

                    if (string.IsNullOrEmpty(spsCallbackResponse.NOperacion))
                    {
                        _dataContext.InsertLog
                            (LogTypeModel.Error,
                            "PGPluginSPS/Controllers/CallbackController/Post/Step2",
                            "No se recibió el número de operación",
                            string.Empty);

                        return Request.CreateResponse(HttpStatusCode.Conflict, "No se recibió el número de operación");
                    }

                    if (!ExistOperationNumber(spsCallbackResponse.NOperacion))
                    {
                        try
                        {


                            TransactionModel transactionIdToUpdate = _dataContext.GetTransactionByTransactionId(spsCallbackResponse.NOperacion);

                            if (transactionIdToUpdate == null)
                            {
                                _dataContext.InsertLog
        (LogTypeModel.Error,
        "PGPluginSPS/Controllers/CallbackController/Post/Step2-Error",
        "La transaccion que envio SPS no existe en PG: " + spsCallbackResponse.NOperacion,
        string.Empty);

                            }
                            long transactionId = (long)transactionIdToUpdate.Id;

                            this.SaveCallback(spsCallbackResponse, transactionId);

                            _dataContext.InsertLog
                                (LogTypeModel.Info,
                                "PGPluginSPS/Controllers/CallbackController/Post/Step3",
                                "Callback Received",
                                string.Empty);

                            string transactionResult = spsCallbackResponse.Resultado;
                            int validatorId = transactionIdToUpdate.ValidatorId;

                            _dataContext.UpdateTransactionStatus(transactionId, transactionResult, validatorId);

                            //END ALTU 3.4

                            //Modify for v4 - TRI
                            if (transactionIdToUpdate != null)
                            {
                                TransactionResultInfoModel toSaveTRISPSResponse = new TransactionResultInfoModel();
                                decimal monto = spsCallbackResponse.Monto ?? 0;
                                monto = monto * 100;
                                string mascaraTarjeta = spsCallbackResponse.NroTarjetaVisible;
                                toSaveTRISPSResponse.Amount = (long)monto;
                                toSaveTRISPSResponse.AuthorizationCode = spsCallbackResponse.CodAutorizacion;
                                toSaveTRISPSResponse.CardHolder = spsCallbackResponse.Titular;
                                toSaveTRISPSResponse.CardMask = mascaraTarjeta;
                                toSaveTRISPSResponse.Country = spsCallbackResponse.PaisEntrega;
                                toSaveTRISPSResponse.CreatedBy = "PluginSPS";
                                toSaveTRISPSResponse.CreatedOn = DateTime.Now;
                                toSaveTRISPSResponse.Currency = spsCallbackResponse.Moneda;
                                toSaveTRISPSResponse.CustomerDocNumber = (spsCallbackResponse.NroDoc == null) ? "" : spsCallbackResponse.NroDoc.ToString();
                                toSaveTRISPSResponse.CustomerDocType = spsCallbackResponse.TipoDocDescri;
                                toSaveTRISPSResponse.CustomerEmail = spsCallbackResponse.EmailComprador;
                                toSaveTRISPSResponse.Payments = spsCallbackResponse.Cuotas;
                                toSaveTRISPSResponse.StateExtendedMessage = spsCallbackResponse.MotivoAdicional;
                                toSaveTRISPSResponse.StateMessage = spsCallbackResponse.Motivo;
                                toSaveTRISPSResponse.TransactionIdPK = transactionIdToUpdate.Id;
                                toSaveTRISPSResponse.ResponseCode = spsCallbackResponse.Resultado;
                                toSaveTRISPSResponse.TicketNumber = spsCallbackResponse.NroTicket;
                                toSaveTRISPSResponse.CardNbrLfd = mascaraTarjeta.Substring(Math.Max(0, mascaraTarjeta.Length - 4)); ;

                                int lastIdTRI = _dataContext.InsertTransactionResultInfo(toSaveTRISPSResponse);

                                _dataContext.InsertLog(LogTypeModel.Info, "PGPluginSPS/Controllers/CallbackController/Post/Step4", "TRI Saved: " + lastIdTRI.ToString(), string.Empty);

                                string SPSResponse = Tools.ObjectToString(spsCallbackResponse, "|");
                                PaymentValidatorCommModel SPSResponseToValidator = new PaymentValidatorCommModel();
                                SPSResponseToValidator.RequestDate = DateTime.Now;
                                SPSResponseToValidator.RequestMessage = SPSResponse;
                                SPSResponseToValidator.TransactionId = transactionIdToUpdate.Id;
                                _dataContext.UpdatePaymentValidatorCommByIdTransaction(SPSResponseToValidator);
                            }
                            else
                            {
                                _dataContext.InsertLog(LogTypeModel.Warning, "PGPluginSPS/Controllers/CallbackController/Post/Step5", "TRI NOT Saved - No Transaction for TransactionId: " + spsCallbackResponse.NOperacion, string.Empty);
                            }

                            return Request.CreateResponse(HttpStatusCode.OK, "Callback received");
                        }
                        catch (Exception exception)
                        {
                            _dataContext.InsertLog
                                (LogTypeModel.Error,
                                "PGPluginSPS/Controllers/CallbackController/Post/Step4",
                                exception.Message,
                                exception.ToString());

                            var error = new HttpError(exception.ToString());
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
                        }
                    }

                    string conflicMessage = string.Format("El numero de operacion {0} ya existe.", spsCallbackResponse.NOperacion);
                    _dataContext.InsertLog
                        (LogTypeModel.Error,
                        "PGPluginSPS/Controllers/CallbackController/Post/Step4-Error",
                        conflicMessage,
                        string.Empty);

                    return Request.CreateResponse
                        (HttpStatusCode.Conflict,
                        conflicMessage);
                }
            }
            catch (Exception exception)
            {
                var error = new HttpError(exception.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError, error);
            }
        }

        #region Private Methods

        private void SaveCallback(CallbackResponse spsCallbackResponse, long transactionId)
        {
            var spsServiceResponse = this.MapSpsCallbackResponsesToPaymentResponse(spsCallbackResponse);

            using (PGDataServiceClient context = new PGDataServiceClient())
            {
                PaymentValidatorCommModel newResponseFromSPS = new PaymentValidatorCommModel();
                newResponseFromSPS.TransactionId = transactionId;
                newResponseFromSPS.RequestDate = spsCallbackResponse.FechaHora;
                newResponseFromSPS.RequestMessage = null;
                newResponseFromSPS.ResponseDate = DateTime.Now;
                newResponseFromSPS.ResponseMessage = MapSpsCallbackResponsesToPaymentResponse(spsCallbackResponse);
                context.InsertPaymentValidatorComm(newResponseFromSPS);
            }
        }

        private bool ExistOperationNumber(string operationNumber)
        {
            bool isInTRI = false;
            try
            {
                using (PGDataServiceClient context = new PGDataServiceClient())
                {
                    isInTRI = context.IsTransactionInTRIByTransactionId(operationNumber);
                }
            }
            catch (Exception ex)
            {
                //Todo Log Error
            }

            return isInTRI;
        }

        private string MapSpsCallbackResponsesToPaymentResponse(CallbackResponse spsCallbackResponse)
        {
            string mappedResponse = "";
            mappedResponse += " | Reason " + spsCallbackResponse.Motivo;
            mappedResponse += " | Currency " + spsCallbackResponse.Moneda;
            mappedResponse += " | DeliveryAddress " + spsCallbackResponse.Direccionentrega;
            mappedResponse += " | AddressValidation " + spsCallbackResponse.Validaciondomicilio;
            mappedResponse += " | OrderCode " + spsCallbackResponse.CodigoPedido;
            mappedResponse += " | DeliveryName " + spsCallbackResponse.NombreEntrega;
            mappedResponse += " | Date " + spsCallbackResponse.FechaHora;
            mappedResponse += " | PurchaserTelephone " + spsCallbackResponse.TelefonoComprador;
            mappedResponse += " | DeliveryDistrict " + spsCallbackResponse.BarrioEntrega;
            mappedResponse += " | AuthorizationCode " + spsCallbackResponse.CodAutorizacion;
            mappedResponse += " | DeliveryCountry " + spsCallbackResponse.PaisEntrega;
            mappedResponse += " | Quotes " + spsCallbackResponse.Cuotas;
            mappedResponse += " | IsDateOfBirthValidated " + spsCallbackResponse.ValidaFechaNac;
            mappedResponse += " | IsDocNumValidated " + spsCallbackResponse.ValidaNroDoc;
            mappedResponse += " | Holder " + spsCallbackResponse.Titular;
            mappedResponse += " | Order " + spsCallbackResponse.Pedido;
            mappedResponse += " | ZipNumDelivery " + spsCallbackResponse.ZipEntrega;
            mappedResponse += " | Amount " + spsCallbackResponse.Monto;
            mappedResponse += " | Card " + spsCallbackResponse.Tarjeta;
            mappedResponse += " | DeliveryDate " + spsCallbackResponse.FechaEntrega;
            mappedResponse += " | PurchaserMail " + spsCallbackResponse.EmailComprador;
            mappedResponse += " | IsGateNumValidated " + spsCallbackResponse.ValidaNroPuerta;
            mappedResponse += " | DeliveryCity " + spsCallbackResponse.CiudadEntrega;
            mappedResponse += " | IsDocTypeValidated " + spsCallbackResponse.ValidaTipoDoc;
            mappedResponse += " | OperationNumber " + spsCallbackResponse.NOperacion;
            mappedResponse += " | DeliveryState " + spsCallbackResponse.EstadoEntrega;
            mappedResponse += " | Result " + spsCallbackResponse.Resultado;
            mappedResponse += " | DeliveryMessage " + spsCallbackResponse.MensajeEntrega;
            mappedResponse += " | SiteParameter " + spsCallbackResponse.ParamSitio;
            mappedResponse += " | DocType " + spsCallbackResponse.TipoDoc;
            mappedResponse += " | DocTypeDescription " + spsCallbackResponse.TipoDocDescri;
            mappedResponse += " | DocNumber " + spsCallbackResponse.NroDoc;
            mappedResponse += " | ChargebackDate " + spsCallbackResponse.FechaContraCargo;
            mappedResponse += " | chargebackReason " + spsCallbackResponse.MotivoContraCargo;
            mappedResponse += " | chargebackSite " + spsCallbackResponse.SiteContraCargo;
            mappedResponse += " | AuthenticationResultVbv " + spsCallbackResponse.ResultadoAutenticacionVBV;
            mappedResponse += " | VisibleCardNum " + spsCallbackResponse.NroTarjetaVisible;
            mappedResponse += " | AditionalReason " + spsCallbackResponse.MotivoAdicional;
            mappedResponse += " | TicketNum " + spsCallbackResponse.NroTicket;
            mappedResponse += " | IdMotivo " + spsCallbackResponse.IdMotivo;
            mappedResponse += " | CreatedOn = " + DateTime.Now.ToString();

            return mappedResponse;


        }

        #endregion Private Methods
    }
}