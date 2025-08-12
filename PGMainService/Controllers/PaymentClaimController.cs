using PGMainService.Manager;
using PGMainService.Models;
using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace PGMainService.Controllers
{
    public class PaymentClaimController : ApiController
    {
        private readonly Utils _utilities = new Utils();
        private readonly List<string> validationResults = new List<string>();
        private readonly PGDataServiceClient _dataContext = new PGDataServiceClient();
        private const string statusCodeOpen = "PGCLAIMOPEN";
        private const string statusCodeAnulated = "PGCLAIMOPERATIONAPPROVED";
        private const string statusCodeApprovedToSend = "PGCLAIMAPPROVEDTOSEND";
        private const string statusCodeRejectedToSend = "PGCLAIMREJECTEDTOSEND";
        private const string statusCodeCancelled = "PGCLAIMCANCELLED";
        private const string statusCodeClaimSentTimeOut = "PGCLAIMSENTTIMEOUT";
        private const string statusCodeClaimError = "PGCLAIMERROR";
        private const string statusCodeClaimSent = "PGCLAIMSENT";
        private const string statusCodeClaimAlreadyProcess = "PGCLAIMALREADYPROCESSED";
        private const string statusCodeClaimRejected = "PGCLAIMOPERATIONREJECTED";
        private const string statusCodeClaimUndefined = "PGCLAIMUNDEFINED";
        private const string StatusCodeClaimLocked = "PGCLAIMLOCKED";
        private const string statusCodePaymentRefunded = "PGPAYMENTREFUNDED";
        private const string statusCodePGPaymentPaid = "PGPAYMENTPAID";
        private const string channelName = "WEB";
        private const string moduleTypeClaims = "claim";
        private const string moduleTypePayment = "payment";
        private const string statusCodePGPaymentPaidwithPartialRefund = "PGPAYMENTPAIDWITHPARTIALREFUND";
        private const string statusCodePGPaymentRefundedByAllPartials = "PGPAYMENTREFUNDEDBYALLPARTIALS";
        private const string statusCodeSpsInvalidUser = "STATUSCODEINVALIDUSER";
        private string languageCode = "";

        [AppConfig]
        [AllowAnonymous]
        [Route("PaymentClaim/Status")]
        public HttpResponseMessage GetStatus()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Payment Claim - Service Online")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [AppConfig]
        [Authorize(Roles = "apiClaimsGet")]
        [Route("PaymentClaim/{paymentClaimNumber}")]
        [ResponseType(typeof(PaymentClaimModelSearchResult))]
        public HttpResponseMessage Get(long paymentClaimNumber)
        {
            var header = this._utilities.GetDataFromHTTPRequest(this.Request.Headers);

            if (!header.HasAuthentication || !header.IsAuthorized)

                return Request.CreateResponse(HttpStatusCode.Forbidden, $"El usuario no tiene la autorización para consultar un reclamo {paymentClaimNumber}");

            if (!HttpContext.Current.User.IsInRole("apiClaimsGet"))
                return Request.CreateResponse(HttpStatusCode.Forbidden, $"El usuario no tiene la autorización para consultar un reclamo {paymentClaimNumber}");

            bool canUserWork = this._utilities.GetResponse<bool>("claim", "CanUserWorkWithPaymentClaimByPaymentClaimNumber", true, new PaymentClaimInputDataLayer()
            {
                user = header.User,
                claimnumber = paymentClaimNumber
            });

            if (canUserWork)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, $"El usuario no tiene la autorización para consultar un reclamo {paymentClaimNumber}");
            }

            try
            {
                var paymentClaimId = this._dataContext.GetPaymentClaimIdByNumber(paymentClaimNumber);
                if (!paymentClaimId.HasValue)
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"No se encontró el reclamo para el Numero: {paymentClaimNumber}");

                var paymentClaim = this._dataContext.GetPaymentClaimById(paymentClaimId.Value);

                var claimToReturn = new PaymentClaimModelSearchResult();

                var transaction = this._dataContext.GetTransactionById(paymentClaim.TransactionIdPK);

                var claimer = this._dataContext.GetClaimerById(paymentClaim.ClaimerId.GetValueOrDefault());

                var claimerToReturn = new PaymentClaimModelSearchResult_Claimer()
                {
                    ClaimerMail = claimer.Email,
                    DocNumber = claimer.DocNumber,
                    DocType = claimer.DocShortName,
                    FirstName = claimer.Name,
                    LastName = claimer.LastName,
                    Mobile = claimer.Cellphone,
                    Phone = claimer.Phone
                };
                claimToReturn.Claimer = claimerToReturn;
                claimToReturn.CreatedBy = paymentClaim.CreatedBy;

                claimToReturn.CreatedOn = paymentClaim.CreatedOn;

                claimToReturn.CurrencyCode = transaction.CurrencyCode;
                claimToReturn.MerchantId = transaction.MerchantId;
                claimToReturn.Observation = paymentClaim.Observation;
                claimToReturn.Product = transaction.Product;
                claimToReturn.RefundAmount = paymentClaim.Amount.ToString();
                claimToReturn.StatusCode = paymentClaim.ActualStateCode;

                // Si this.languageCode no se setea antes, será "" o el valor de una llamada previa.
                string currentLanguageCode = string.IsNullOrEmpty(this.languageCode) ? "es" : this.languageCode;
                claimToReturn.StatusMessage = this._dataContext.GetStatusMessageByStatusCode(paymentClaim.ActualStateCode, currentLanguageCode).ResponseStatusMessage;

                claimToReturn.TransactionId = transaction.TransactionId;
                claimToReturn.UpdatedBy = paymentClaim.UpdatedBy;
                claimToReturn.UpdatedOn = paymentClaim.UpdatedOn;
                claimToReturn.PaymentClaimNumber = paymentClaim.PaymentClaimNumber.ToString();

                var annulmentResultInfo = this._dataContext.GetAnnulmentResultInfoByPaymentClaimId(paymentClaim.PaymentClaimId);
                if (annulmentResultInfo != null)
                {
                    claimToReturn.AdditionalInfo = new ClaimAdditionalInfo()
                    {
                        authorizationCode = annulmentResultInfo.AuthorizationCode,
                        operationNumber = annulmentResultInfo.OperationNumber,
                        originalDateTime = annulmentResultInfo.OriginalDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    };
                }
                return Request.CreateResponse(HttpStatusCode.OK, claimToReturn);
            }
            catch (Exception ex)
            {
                _utilities.InsertLogException(LogType.Error, ex, nameof(Get));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Hubo un error inesperado al obtener el reclamo");
            }
        }

        [AppConfig]
        [Authorize(Roles = "apiClaimsCreate")]
        [Route("PaymentClaim/Create")]
        [ResponseType(typeof(PaymentClaimRequestResult))]
        public HttpResponseMessage Post([FromBody] PaymentClaimInput paymentClaim)
        {
            if (!this.ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);

            var requestResult = new PaymentClaimRequestResult();
            try
            {
                this.languageCode = this.Request.Headers.AcceptLanguage.FirstOrDefault()?.ToString().Split('-')[0];
                if (string.IsNullOrEmpty(this.languageCode)) this.languageCode = "es";
            }
            catch (Exception)
            {
                this.languageCode = "es";
            }

            if (!HttpContext.Current.User.IsInRole("apiClaimsCreate"))
            {
                requestResult.ResponseMessage = "El usuario no tiene la autorización para crear un reclamo";
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, requestResult);
            }

            var currentUser = HttpContext.Current.User.Identity.Name;

            var response = this.CreatePaymentClaim(paymentClaim, currentUser);
            return response;
        }
        private PaymentClaimModel MapPaymentClaimInputToPaymentClaimModel(PaymentClaimInputModel paymentClaim)
        {
            return new PaymentClaimModel()
            {
                TransactionIdPK = new long?(paymentClaim.IdTransaction),
                CommerceItems = paymentClaim.CommerceItems.ToArray(),
                CurrencyId = new int?(paymentClaim.CurrencyId),
                Amount = new Decimal?(paymentClaim.TotalAmountToAnnulment),
                Observation = paymentClaim.Observation,
                IsLocked = false,
                CreatedBy = HttpContext.Current.User.Identity.Name,
                CreatedOn = DateTime.Now
            };
        }
        private void UpdateCommerceItemByClaim(long paymentClaimId, int stateId)
        {
            foreach (CommerceItemModel commerceItemModel in ((IEnumerable<CommerceItemModel>)this._dataContext.GetCommerceItemByPaymentClaimId(paymentClaimId)).ToList<CommerceItemModel>())
                this._dataContext.UpdateCommerceItemState(commerceItemModel.CommerceItemId, stateId);
        }
        private void SendTicketToClientByStatus(string statusCode, PaymentClaimModel paymentClaimModel)
        {
            try
            {
                this._utilities.InsertLogCommon(LogType.Info, $"Method: void SendTicketToClientByStatus(string statusCode, PaymentClaimModel paymentClaimModel) - statusCode:{statusCode}, paymentClaim: {paymentClaimModel}");
                PaymentClaimTicketModel claimTicketModel = new PaymentClaimTicketModel();
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex);
                throw;
            }
        }

        private PaymentClaimInputModel SanitizePaymentClaim(PaymentClaimInput claimToSanitize)
        {
            ClaimerModel claimerModel = new ClaimerModel()
            {
                Cellphone = claimToSanitize.ClaimerCellphone,
                LastName = claimToSanitize.ClaimerLastName,
                Email = claimToSanitize.ClaimerMail,
                Name = claimToSanitize.ClaimerName,
                Phone = claimToSanitize.ClaimerPhone,
                DocNumber = claimToSanitize.DocumentNumber,
                DocTypeId = this._dataContext.GetDocTypeByShortName(claimToSanitize.DocumentType).DocTypeId
            };
            PaymentClaimInputModel paymentClaimInputModel = new PaymentClaimInputModel()
            {
                MerchantId = claimToSanitize.MerchantId,
                Observation = claimToSanitize.Observation,
                TransactionId = claimToSanitize.TransactionId,
                ClaimerModel = claimerModel
            };
            try
            {
                this._utilities.InsertLogCommon(LogType.Info, $"Method: PaymentClaim SanitizePaymentClaim(PaymentClaim claimToSanitize) - claimToSanitize: {claimToSanitize}");
                string transactionId = claimToSanitize.TransactionId;
                TransactionModel transactionByTransactionId = this._dataContext.GetTransactionByTransactionId(transactionId);
                string codeOfTransaction = this._dataContext.GetModuleCodeOfTransaction(transactionId);
                if (codeOfTransaction == null)
                {
                    this._utilities.InsertLogCommon(LogType.Warning, $"La transacción con nro. {transactionId} no posee un estado, no se puede crear una reclamo sobre la misma.");
                    this.validationResults.Add($"La transacción con nro. {transactionId} no posee un estado, no se puede crear una reclamo sobre la misma.");
                    return paymentClaimInputModel;
                }
                if (transactionByTransactionId == null)
                {
                    this._utilities.InsertLogCommon(LogType.Warning, $"No existe una transacción para el valor TransactionId: {transactionId}");
                    this.validationResults.Add($"No existe una transacción para el valor TransactionId: {transactionId}");
                    return paymentClaimInputModel;
                }
                if (!(codeOfTransaction == "PGPAYMENTPAID") && !(codeOfTransaction == "PGPAYMENTPAIDWITHPARTIALREFUND"))
                {
                    StatusResponseMessageModel messageByOriginalCode = this._dataContext.GetStatusMessageByOriginalCode("payment", codeOfTransaction, this.languageCode);
                    string str = messageByOriginalCode.ResponseStatusMessage != null ? messageByOriginalCode.ResponseStatusMessage : "null";
                    string logMessage = transactionByTransactionId == null ? $"No existe una transacción para el valor TransactionId: {transactionId}" : $"{messageByOriginalCode.ResponseStatusMessage}. No se puede crear el reclamo.";
                    this._utilities.InsertLogCommon(LogType.Warning, logMessage);
                    this.validationResults.Add(logMessage);
                    return paymentClaimInputModel;
                }
                paymentClaimInputModel.IdTransaction = transactionByTransactionId.Id;
                paymentClaimInputModel.MerchantId = claimToSanitize.MerchantId;
                if (transactionByTransactionId.MerchantId.ToLower() != paymentClaimInputModel.MerchantId.ToLower())
                {
                    string logMessage = $"El valor MerchantId: {claimToSanitize.MerchantId} no es válido";
                    this._utilities.InsertLogCommon(LogType.Warning, logMessage);
                    this.validationResults.Add(logMessage);
                    return paymentClaimInputModel;
                }
                foreach (PaymentClaimModel paymentClaimModel in this._dataContext.GetPaymentClaimsByTransactionId(transactionId))
                {
                    string codeOfPaymentClaim = this._dataContext.GetModuleCodeOfPaymentClaim(paymentClaimModel.PaymentClaimId);
                    if (codeOfPaymentClaim == "PGCLAIMOPEN" || codeOfPaymentClaim == "PGCLAIMAPPROVEDTOSEND")
                    {
                        string logMessage = $"Ya existe un reclamo abierto o pendiente de aprobación sobre alguno de los CI enviados para la transacción número: {transactionId}";
                        this._utilities.InsertLogCommon(LogType.Warning, logMessage);
                        this.validationResults.Add(logMessage);
                        return paymentClaimInputModel;
                    }
                }
                string documentType = claimToSanitize.DocumentType;
                if (this._dataContext.GetDocTypeByShortName(documentType) == null)
                {
                    string logMessage = $"El valor documentType: {documentType} no es válido";
                    this._utilities.InsertLogCommon(LogType.Warning, logMessage);
                    this.validationResults.Add(logMessage);
                    return paymentClaimInputModel;
                }
                List<string> stringList1 = new List<string>();
                if (transactionByTransactionId.CommerceItems != null)
                {
                    List<string> stringList2;
                    bool flag;
                    if (claimToSanitize.ListOfCommerceItemCodeToRefund != null)
                    {
                        stringList2 = claimToSanitize.ListOfCommerceItemCodeToRefund;
                        List<string> list = ((IEnumerable<CommerceItemModel>)transactionByTransactionId.CommerceItems).Select<CommerceItemModel, string>((Func<CommerceItemModel, string>)(transCI => transCI.Code)).ToList<string>();
                        IEnumerable<string> source = stringList2.Except<string>((IEnumerable<string>)list);
                        if (source.Any<string>())
                        {
                            this._utilities.InsertLogCommon(LogType.Warning, "Se enviaron commerce items que no existen en la transacción.");
                            foreach (string str in source)
                                this.validationResults.Add($"El commerce item con código: {str} no existe en la transacción: {transactionByTransactionId.TransactionId}");
                            return paymentClaimInputModel;
                        }
                        if (stringList2.GroupBy<string, string>((Func<string, string>)(x => x)).Where<IGrouping<string, string>>((Func<IGrouping<string, string>, bool>)(group => group.Count<string>() > 1)).Select<IGrouping<string, string>, string>((Func<IGrouping<string, string>, string>)(group => group.Key)).Any<string>())
                        {
                            this._utilities.InsertLogCommon(LogType.Warning, "Se enviaron commerce items duplicados.");
                            this.validationResults.Add("Existen códigos de items duplicados en la petición de reclamo.");
                            return paymentClaimInputModel;
                        }
                        if (stringList2.Join((IEnumerable<CommerceItemModel>)transactionByTransactionId.CommerceItems, (Func<string, string>)(req => req), (Func<CommerceItemModel, string>)(trans => trans.Code), (req, trans) => new
                        {
                            req = req,
                            trans = trans
                        }).Where(_param1 => _param1.trans.State != 0).Select(_param1 => _param1.req).ToList<string>().Any<string>())
                        {
                            this._utilities.InsertLogCommon(LogType.Warning, "Se quisieron anular commerce items en proceso de anulación o ya anulados.");
                            this.validationResults.Add("Existen códigos de items anulados o pendientes de anulación en la petición.");
                            return paymentClaimInputModel;
                        }
                        flag = true;
                    }
                    else
                    {
                        stringList2 = ((IEnumerable<CommerceItemModel>)transactionByTransactionId.CommerceItems).Where<CommerceItemModel>((Func<CommerceItemModel, bool>)(ci => ci.State == 0)).Select<CommerceItemModel, string>((Func<CommerceItemModel, string>)(ci => ci.Code)).ToList<string>();
                        if (stringList2.Any<string>())
                        {
                            flag = true;
                        }
                        else
                        {
                            this._utilities.InsertLogCommon(LogType.Warning, "No existe ningún commerce items en la transacción que sea plausible de ser anulado.");
                            this.validationResults.Add("No existe ningún commerce items en la transacción que sea plausible de ser anulado.");
                            return paymentClaimInputModel;
                        }
                    }
                    if (flag)
                    {
                        Decimal num = 0M;
                        paymentClaimInputModel.CommerceItems = new List<CommerceItemModel>();
                        foreach (string str in stringList2)
                        {
                            string CIToSanitaze = str;
                            CommerceItemModel commerceItemModel1 = new CommerceItemModel();
                            CommerceItemModel commerceItemModel2 = ((IEnumerable<CommerceItemModel>)transactionByTransactionId.CommerceItems).Where<CommerceItemModel>((Func<CommerceItemModel, bool>)(ciInTrans => ciInTrans.Code == CIToSanitaze)).FirstOrDefault<CommerceItemModel>();
                            paymentClaimInputModel.CommerceItems.Add(commerceItemModel2);
                            num += commerceItemModel2.Amount;
                        }
                        paymentClaimInputModel.TotalAmountToAnnulment = num;
                    }
                    paymentClaimInputModel.CurrencyId = transactionByTransactionId.CurrencyId;
                }
                else
                {
                    this._utilities.InsertLogCommon(LogType.Warning, $"La transacción: {transactionByTransactionId.TransactionId} no tiene Commerce Items asociados");
                    this.validationResults.Add("La transacción no tiene Commerce Items asociados");
                    return paymentClaimInputModel;
                }
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex);
                this.validationResults.Add("Error interno del sistema, por favor contacte al administrador");
                return paymentClaimInputModel;
            }
            return paymentClaimInputModel;
        }
        private HttpResponseMessage CreatePaymentClaim(PaymentClaimInput paymentClaim, string currentUser)
        {
            var requestResult = new PaymentClaimRequestResult { ResponseMessage = "Operación no completada" };
            PaymentClaimInputModel sanitizedPaymentClaim;

            try
            {
                _utilities.InsertLogCommon(LogType.Info, $"CreatePaymentClaim - paymentClaim: {paymentClaim}");
                sanitizedPaymentClaim = this.SanitizePaymentClaim(paymentClaim);
            }
            catch (Exception ex)
            {
                _utilities.InsertLogException(LogType.Error, ex, nameof(CreatePaymentClaim));
                requestResult.ResponseMessage = $"{ex.Message}: {ex.StackTrace}";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, requestResult);
            }

            if (this.validationResults.Any())
            {
                string validationMessage = string.Join("; ", this.validationResults);
                _utilities.InsertLogCommon(LogType.Warning, validationMessage, null, nameof(CreatePaymentClaim));

                requestResult.ResponseMessage = validationMessage;
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, requestResult);
            }

            try
            {
                var paymentClaimToInsert = this.MapPaymentClaimInputToPaymentClaimModel(sanitizedPaymentClaim);
                var insertedPaymentClaimId = this._dataContext.InsertPaymentClaim(paymentClaimToInsert, sanitizedPaymentClaim.ClaimerModel, statusCodeOpen, currentUser);

                var statusMessageResult = this._dataContext.GetStatusMessageByOriginalCode(moduleTypeClaims, statusCodeOpen, this.languageCode);

                if (!insertedPaymentClaimId.HasValue || insertedPaymentClaimId.Value == 0)
                {
                    requestResult.ResponseMessage = "Error al insertar el reclamo en la base de datos.";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, requestResult);
                }

                this.UpdateCommerceItemByClaim(insertedPaymentClaimId.Value, 1); // Estado 1 = En curso

                // Obtener el número de reclamo real después de la inserción
                var createdClaim = this._dataContext.GetPaymentClaimById(insertedPaymentClaimId.Value);
                if (createdClaim == null)
                {
                    requestResult.ResponseMessage = "Reclamo insertado pero no se pudo recuperar para obtener el número.";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, requestResult);
                }

                this.SendTicketToClientByStatus(statusMessageResult.ResponseStatusCode, createdClaim);

                requestResult.ResponseMessage = "Operación completada con éxito";
                requestResult.ResponseStatusCode = statusMessageResult.ResponseStatusCode;
                requestResult.ResponseStatusMessage = statusMessageResult.ResponseStatusMessage;
                requestResult.ResponseGenericMessage = statusMessageResult.ResponseGenericMessage;
                requestResult.ClaimNumber = createdClaim.PaymentClaimNumber;

                return Request.CreateResponse(HttpStatusCode.Created, requestResult);
            }
            catch (Exception ex)
            {
                _utilities.InsertLogException(LogType.Error, ex, nameof(CreatePaymentClaim));
                requestResult.ResponseMessage = $"{ex.Message}: {ex.StackTrace}";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, requestResult);
            }
        }
    }
}