using PGPluginSPS.Models;
using PGPluginSPS.PGDataAccess;
using PGPluginSPS.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace PGPluginSPS.Controllers
{
    public class PaymentClaimController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "PaymentClaimSPS - Service Online.");
        }

        /// <summary>
        /// Send a claim to be processed on the SPS Validator
        /// </summary>
        /// <param name="orderToClaim">Order To Claim</param>
        /// <returns></returns>
        public HttpResponseMessage Post(AnnulationOrderModel orderToClaim)
        {
            try
            {
                using (var _dataContext = new PGDataServiceClient())
                {
                    var client = new WebServiceInterface();
                    var requirement = new SPSWebServiceRequest();
                    var transaction = _dataContext.GetTransactionByTransactionId(orderToClaim.TransactionId);

                    //Fix to v4
                    var validatorservconfig = new ValidatorServiceConfigModel();
                    validatorservconfig = _dataContext.GetValidatorConfigByServiceId(transaction.ServiceId, 3);
                    requirement.Username = validatorservconfig.ValidatorUser;
                    requirement.Password = validatorservconfig.ValidatorPass;
                    requirement.Idsite = transaction.UniqueCode;
                    requirement.Nrooperacion = orderToClaim.TransactionId;
                    requirement.Importe = orderToClaim.Import + "." + orderToClaim.Cents;
                    SPSWebServiceResponse response = new SPSWebServiceResponse();

                    //Get Status before proceed.

                    //Save the request.
                    int savedrequest = SaveRequest(orderToClaim.RequestOrder, WebServiceInterface.Operation.Query, requirement);

                    response = client.Communicate(WebServiceInterface.Operation.Query, requirement);
                    WebServiceInterface.Operation newOperationToPerform = new WebServiceInterface.Operation();
                    if (response.idResultado != "0")
                    {
                        //There is an error in the communication.
                        //Save the response because has some return.
                        SaveResponse(savedrequest, response.originalRequest);
                        return GetHttpResponseFromIdResultado(response.idResultado);
                    }
                    else
                    {
                        //Save the response
                        SaveResponse(savedrequest, response.originalRequest);

                        switch (response.idEstado)
                        {
                            case "7":
                            case "8":
                            case "9":
                            case "10":
                                //Is already "anulada" o "devuelta"
                                AnnulmentResultInfoModel ARIToSave = new AnnulmentResultInfoModel();
                                ARIToSave.TransactionId = transaction.Id;
                                ARIToSave.AuthorizationCode = "N/A";
                                ARIToSave.OperationNumber = "N/A";
                                ARIToSave.OriginalDateTime = DateTime.Now;
                                ARIToSave.ValidatorId = 3;
                                ARIToSave.PaymentClaimId = orderToClaim.PaymentClaimId;
                                _dataContext.InsertAnnulmentResultInfo(ARIToSave);
                                return Request.CreateResponse(HttpStatusCode.OK, "PGCLAIMALREADYPROCESSED");

                            case "4":
                                //Is already "autorizada" (no cerró el proceso de lotes, se debe anular)

                                //Added Partial Annulment for 4.1
                                if (orderToClaim.OperationType == WebServiceInterface.Operation.PartialRefund)
                                {
                                    newOperationToPerform = WebServiceInterface.Operation.PartialRefund;
                                }
                                else
                                {
                                    newOperationToPerform = WebServiceInterface.Operation.Annulment; //Original for 3.7
                                }
                                break;

                            case "6":
                                //Is already "acreditada" (cerró el proceso de lotes, se debe devolver)

                                //Added Partial Annulment for 4.1
                                if (orderToClaim.OperationType == WebServiceInterface.Operation.PartialRefund)
                                {
                                    newOperationToPerform = WebServiceInterface.Operation.PartialRefund;
                                }
                                else
                                {
                                    newOperationToPerform = WebServiceInterface.Operation.TotalRefund;//Original for 3.7
                                }
                                break;

                            default:
                                //ERROR INDEFINIDO

                                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                        }
                    }

                    SPSWebServiceResponse responseProcess = new SPSWebServiceResponse();
                    int savedProcessRequest = SaveRequest(orderToClaim.RequestOrder, newOperationToPerform, requirement);
                    responseProcess = client.Communicate(newOperationToPerform, requirement);
                    SaveResponse(savedProcessRequest, responseProcess.originalRequest);

                    if (responseProcess.idResultado != "0")
                    {
                        return GetHttpResponseFromIdResultado(responseProcess.idResultado);
                    }
                    else
                    {
                        //TESTEAR CASO 8 o 10?

                        AnnulmentResultInfoModel ARIToSave = new AnnulmentResultInfoModel();
                        ARIToSave.TransactionId = transaction.Id;
                        ARIToSave.AuthorizationCode = "N/A";
                        ARIToSave.OperationNumber = "N/A";
                        ARIToSave.OriginalDateTime = DateTime.Now;
                        ARIToSave.ValidatorId = 3;
                        ARIToSave.PaymentClaimId = orderToClaim.PaymentClaimId;
                        _dataContext.InsertAnnulmentResultInfo(ARIToSave);

                        return Request.CreateResponse(HttpStatusCode.OK, "PGCLAIMOPERATIONAPPROVED");
                    }
                }
            }
            catch (Exception exSrException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, LogTool.InsertLogException(LogTypeModel.Error, exSrException));
            }
        }

        private HttpResponseMessage GetHttpResponseFromIdResultado(string idResultado)
        {
            switch (idResultado)
            {
                case "0":
                    return Request.CreateResponse(HttpStatusCode.OK);

                case "1":
                    return Request.CreateResponse(HttpStatusCode.NotFound, "PGCLAIMOPERATIONREJECTED");

                case "2":
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "STATUSCODEINVALIDUSER");

                case "3":
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "PGCLAIMOPERATIONREJECTED");

                case "4":
                    return Request.CreateResponse(HttpStatusCode.RequestTimeout, "PGCLAIMUNDEFINED");

                case "5":
                    return Request.CreateResponse(HttpStatusCode.NotModified, "PGCLAIMOPERATIONREJECTED");

                case "6":
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "PGCLAIMUNDEFINED");

                case "7":
                    return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "PGCLAIMOPERATIONREJECTED");

                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Error.");
            }
        }

        private int SaveRequest(int requestId, WebServiceInterface.Operation operationType, SPSWebServiceRequest request)
        {
            try
            {
                using (var _dataContext = new PGDataServiceClient())
                {
                    var requestToSave = new AnnulmentValidatorCommModel
                    {
                        AnnulmentRequestId = requestId,
                        RequestDate = DateTime.UtcNow,
                        RequestMessage =
                            "Validator:SPS|Operation:" + operationType.ToString() +
                            "|IdSite:" + request.Idsite + "|Nrooperacion:" +
                            request.Nrooperacion + "|Importe:" + request.Importe
                    };
                    var communicationId = _dataContext.InsertAnnulmentValidatorComm(requestToSave);

                    return (int)communicationId;
                }
            }
            catch (Exception exSrException)
            {
                LogTool.InsertLogException(LogTypeModel.Error, exSrException);
                return 0;
            }
        }

        private void SaveResponse(int annulmentValidatorCommId, string response)
        {
            try
            {
                using (var _dataContext = new PGDataServiceClient())
                {
                    var requestToUpdate = _dataContext.GetAnnulmentValidatorCommById(annulmentValidatorCommId);
                    if (requestToUpdate != null)
                    {
                        requestToUpdate.ResponseDate = DateTime.UtcNow;
                        requestToUpdate.ResponseMessage = response;

                        _dataContext.UpdateAnnulmentValidatorComm(requestToUpdate);
                    }
                }
            }
            catch (Exception exSrException)
            {
                LogTool.InsertLogException(LogTypeModel.Error, exSrException);
            }
        }
    } 
}