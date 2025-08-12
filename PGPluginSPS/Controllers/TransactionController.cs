using PGPluginSPS.Models;
using PGPluginSPS.PGDataAccess;
using PGPluginSPS.Utils;
using SPS_ServiceReference;
using SPS_ServiceReference.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace PGPluginSPS.Controllers
{
    public class TransactionController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "TransactionsSPS - Service Online.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="operationNumber"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string operationNumber)
        {
            //TODO REFACTOR
            return null;
        }

        /// <summary>
        /// Metodo para actualizar los datos de una transaccion al cerrar la operacion
        /// </summary>
        /// <param name="sps_TransactionId"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]string psp_TransactionId )
        {
            GetResultFromValidator(psp_TransactionId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// This method will check the transaction with any condition.
        /// Avoid any information in the Payment Gateway side, to ensure return 
        /// the real status of the transaction from the validator
        /// </summary>
        /// <param name="TransactionId">NROOPERACION</param>
        /// <returns></returns>
        [Route("Transaction/CloseOperation/{TransactionId}")]
        public HttpResponseMessage GetByTransactionId(string TransactionId)
        {
            string codigoInterno = GetResultFromValidator(TransactionId);
            return Request.CreateResponse(HttpStatusCode.OK, codigoInterno);
        }


        #region Private Methods

        private string GetResultFromValidator(string sps_TransactionId)
        {
            string returnresult = "";
            try
            {
                using (var _dataContext = new PGDataServiceClient())
                {
                    var transactionIdToUpdate = _dataContext.GetTransactionByTransactionId(sps_TransactionId);
                    if (transactionIdToUpdate != null)
                    {
                        //Si existe la transaccion y no esta en la TRI
                        if (!ExistOperationNumberResultInTRI(sps_TransactionId))
                        {
                            CallbackResponse TransactionDataFromSPS = new CallbackResponse();
                            //Si pertenece a la nueva API
                            string serviciosAPI = _dataContext.GetAppConfig("SPSAPIMethods");
                            string[] servicessplitted = serviciosAPI.Split(',');
                            bool isAPI = false;
                            if (servicessplitted.Contains(transactionIdToUpdate.ServiceId.ToString()))
                            {
                                isAPI = true;
                                ValidatorServiceConfigModel config = _dataContext.GetValidatorConfigByServiceId(transactionIdToUpdate.ServiceId, transactionIdToUpdate.ValidatorId);
                                TransactionDataFromSPS = GetTransactionFromAPI(config.HashKey, sps_TransactionId);
                            }
                            else {
                                TransactionDataFromSPS = GetTransaction(sps_TransactionId, transactionIdToUpdate.UniqueCode);
                                isAPI = false;
                            }


                            long transactionId = (long)transactionIdToUpdate.Id;
                            if (TransactionDataFromSPS != null)
                            {
                                //Save in Validator Comm
                                SaveCallback(TransactionDataFromSPS, transactionId);
                                //Obtener el codigo de estado
                                string transactionResult = TransactionDataFromSPS.Resultado;
                                //Actualizar el estado
                                if (isAPI)
                                {
                                    _dataContext.UpdateTransactionStatus(transactionId, transactionResult, "api", transactionIdToUpdate.ValidatorId);
                                }
                                else {
                                    _dataContext.UpdateTransactionStatus(transactionId, transactionResult, "webservice", transactionIdToUpdate.ValidatorId);
                                }

                                //Salvar en TRI
                                //Modify for v4 - TRI
                                if (transactionIdToUpdate != null && transactionResult != "1") //Added Fix para que ingrese en la TRI solo si no es ingresada
                                {
                                    TransactionResultInfoModel toSaveTRISPSResponse = new TransactionResultInfoModel();
                                    decimal monto = TransactionDataFromSPS.Monto ?? 0;
                                    monto = monto * 100;
                                    string mascaraTarjeta = TransactionDataFromSPS.NroTarjetaVisible;
                                    toSaveTRISPSResponse.Amount = (long)monto;
                                    toSaveTRISPSResponse.AuthorizationCode = TransactionDataFromSPS.CodAutorizacion;
                                    toSaveTRISPSResponse.CardHolder = TransactionDataFromSPS.Titular;
                                    toSaveTRISPSResponse.CardMask = mascaraTarjeta;
                                    toSaveTRISPSResponse.Country = TransactionDataFromSPS.PaisEntrega;
                                    toSaveTRISPSResponse.CreatedBy = "PluginSPS";
                                    toSaveTRISPSResponse.CreatedOn = DateTime.Now;
                                    toSaveTRISPSResponse.Currency = TransactionDataFromSPS.Moneda;
                                    toSaveTRISPSResponse.CustomerDocNumber = (TransactionDataFromSPS.NroDoc == null) ? "" : TransactionDataFromSPS.NroDoc.ToString();
                                    toSaveTRISPSResponse.CustomerDocType = TransactionDataFromSPS.TipoDocDescri;
                                    toSaveTRISPSResponse.CustomerEmail = TransactionDataFromSPS.EmailComprador;
                                    toSaveTRISPSResponse.Payments = TransactionDataFromSPS.Cuotas;
                                    toSaveTRISPSResponse.StateExtendedMessage = TransactionDataFromSPS.MotivoAdicional;
                                    toSaveTRISPSResponse.StateMessage = TransactionDataFromSPS.Motivo;
                                    toSaveTRISPSResponse.TransactionIdPK = transactionIdToUpdate.Id;
                                    toSaveTRISPSResponse.ResponseCode = TransactionDataFromSPS.Resultado;
                                    toSaveTRISPSResponse.TicketNumber = TransactionDataFromSPS.NroTicket;
                                    toSaveTRISPSResponse.CardNbrLfd = mascaraTarjeta.Substring(Math.Max(0, mascaraTarjeta.Length - 4)); ;

                                    int lastIdTRI = _dataContext.InsertTransactionResultInfo(toSaveTRISPSResponse);

                                    LogTool.InsertLogCommon(LogTypeModel.Info, "TRI Saved: " + lastIdTRI.ToString());
                                }
                                else
                                {
                                    if (transactionResult == "1")
                                    {
                                        return "PGPAYMENTSENT";
                                    }
                                }
                                //string PGCode = _dataContext.MapOriginalCodeFromValidatorToPGCode(transactionResult, "webservice", "payment", transactionIdToUpdate.ValidatorId);

                                //return PGCode;
                            }
                            else
                            {
                                //CATCH ERROR FOR NO RESPONSE SERVER /NA
                                //Actualizar el estado
                                _dataContext.UpdateTransactionStatus(transactionId, "PGPAYMENTUNDEFINED", "payment", null);
                            }
                        }//End If Check in TRI
                        else
                        {
                            string conflicMessage = string.Format("El numero de operacion {0} ya existe.", sps_TransactionId);
                            LogTool.InsertLogCommon(LogTypeModel.Error, conflicMessage);                       
                        }
                    } //End If Transaction doesnt exists
                    else
                    {
                        LogTool.InsertLogCommon(LogTypeModel.Error, "La transaccion que envio SPS no existe en PG: " + sps_TransactionId);
                    }
                }

            }
            catch (Exception ex)
            {
                LogTool.InsertLogException(LogTypeModel.Error, ex);
            }
            return returnresult;
        
        }

        private void SaveCallback(CallbackResponse spsCallbackResponse, long transactionId)
        {
            var spsServiceResponse = MapSpsCallbackResponsesToPaymentResponse(spsCallbackResponse);

            using (PGDataServiceClient _context = new PGDataServiceClient())
            {
                PaymentValidatorCommModel newResponseFromSPS = new PaymentValidatorCommModel();
                newResponseFromSPS.TransactionId = transactionId;
                newResponseFromSPS.ResponseDate = DateTime.Now;
                newResponseFromSPS.ResponseMessage = spsServiceResponse;

                _context.UpdatePaymentValidatorCommByIdTransaction(newResponseFromSPS);
            }
        }

        private bool ExistOperationNumberResultInTRI(string operationNumber)
        {
            bool isInTRI = false;
            try
            {
                using (PGDataServiceClient context = new PGDataServiceClient())
                {
                    isInTRI = context.IsTransactionInTRIByTransactionId(operationNumber);
                }
            }
            catch (Exception)
            {
                //Todo Log Error
            }

            return isInTRI;
        }

        private CallbackResponse GetTransaction(string transactionId, string uniquecode)
        {
            try
            {
                using (PGDataServiceClient _dataContext = new PGDataServiceClient())
                {
                    var client = new WebServiceInterface();
                    var requirement = new SPSWebServiceRequest();
                    requirement.Idsite = uniquecode;
                    requirement.Nrooperacion = transactionId;
                    CallbackResponse completeInfoOfTransaction = new CallbackResponse();
                    completeInfoOfTransaction = client.GetCompleteData(requirement); //CALL
                    return completeInfoOfTransaction;
                }
            }
            catch (Exception ex)
            {
                //TODO LOG
                return null;
            }

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

        private HttpResponseMessage GetHttpResponseFromIdResultado(string idResultado)
        {
            switch (idResultado)
            {
                case "0":
                    return Request.CreateResponse(HttpStatusCode.OK);

                case "1":
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                case "2":
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid User.");

                case "3":
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                case "4":
                    return Request.CreateResponse(HttpStatusCode.RequestTimeout);

                case "5":
                    return Request.CreateResponse(HttpStatusCode.NotModified);

                case "6":
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Unknown Error.");

                case "7":
                    return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);

                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Error.");
            }
        }

        public CallbackResponse GetTransactionFromAPI(string apikey, string transactionId) {
            try
            {
                SPSAPIClient cliente = new SPSAPIClient();
                SPSClientConfigurationModel config = new SPSClientConfigurationModel();

                config.apikeyPrivate = apikey;
                config.urlbase = ConfigurationManager.AppSettings["SPS_API_BASE"];
                config.urlrequest = ConfigurationManager.AppSettings["SPS_API_TRANSACTION"];
                config.ProxyConfiguration = new SPS_ServiceReference.Models.ProxySettings
                {
                    ProxyActive = ConfigurationManager.AppSettings["proxyOn"].Equals("on"),
                    Domain = ConfigurationManager.AppSettings["proxyDomain"],
                    Password = ConfigurationManager.AppSettings["proxyPassword"],
                    Port = ConfigurationManager.AppSettings["proxyPort"],
                    Server = ConfigurationManager.AppSettings["proxyServer"],
                    Username = ConfigurationManager.AppSettings["proxyUsername"]
                };
                SPSPaymentResponseModelWithLog respuestaconLog = cliente.GetResponseWithLog(config, transactionId);
                SaveRequestTransactionFromAPI(respuestaconLog.LogRequest, respuestaconLog.LogResponse, transactionId);

                SPSPaymentResponseModel respuesta = respuestaconLog.respuesta;
                var firstResult = respuesta.results[0];

                var returned = new CallbackResponse() 
                {
                    NroTarjetaVisible = firstResult.card_data?.card_number, 
                    CodAutorizacion = firstResult.status_details?.card_authorization_code,
                    Titular = firstResult.card_data?.card_holder?.name,
                    PaisEntrega = "", 
                    Moneda = "",      
                    NroDoc = 0,      
                    TipoDocDescri = "",
                    EmailComprador = firstResult.customer?.email,
                    Cuotas = firstResult.installments, 
                    MotivoAdicional = "", 
                    Motivo = "",          
                    Resultado = firstResult.status,
                    NroTicket = firstResult.status_details?.ticket
                };
                return returned;
            }
            catch (Exception ex) {
                LogTool.InsertLogException(LogTypeModel.Error, ex);
            }
            return null;
        }

        private void SaveRequestTransactionFromAPI(string spsRequest, string spsResponse, string transactionId)
        {
            try
            {
                using (PGDataServiceClient context = new PGDataServiceClient())
                {
                    PaymentValidatorCommModel newResponseFromSPS = new PaymentValidatorCommModel();

                    newResponseFromSPS.RequestDate = DateTime.Now;
                    newResponseFromSPS.RequestMessage = spsRequest;
                    newResponseFromSPS.ResponseDate = DateTime.Now;
                    newResponseFromSPS.ResponseMessage = spsResponse;

                    try
                    {
                        int result = Int32.Parse(transactionId);
                        newResponseFromSPS.TransactionId = result;
                    }
                    catch (FormatException)
                    {
                        newResponseFromSPS.TransactionId = 0;
                    }

                    LogTool.InsertLogCommon(LogTypeModel.Info, spsRequest + spsResponse);
                    context.InsertPaymentValidatorComm(newResponseFromSPS);
                }
            }
            catch (Exception ex) {
                LogTool.InsertLogException(LogTypeModel.Error, ex);
            }
        }

        #endregion Private Methods
    }
}