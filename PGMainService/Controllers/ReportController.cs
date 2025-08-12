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
using System.Text;
using System.Web;
using System.Web.Http;

namespace PGMainService.Controllers
{

    public class ReportController : ApiController
    {
        private Utils _utilities = new Utils();
        private PGDataServiceClient _datacontext = new PGDataServiceClient();

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Reports - Service Online.")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [Authorize(Roles = "apiReport,apiAdminServices")]
        [Route("Report/{ReportType}/")]
        public HttpResponseMessage Post(string ReportType, CriteriaModelReportToSearch criteria)
        {
            int jobRunId = 0;

            if (!this.ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);

            try
            {
                if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                {
                    if (!HttpContext.Current.User.IsInRole("apiReport"))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
            }

            AuxSearchCriteria searchCriteria;
            try
            {
                searchCriteria = this.GetSearchCriteria(criteria);
                ReportType = ReportType.ToLower();
                if (searchCriteria.DataValidationResponse != null)
                    return searchCriteria.DataValidationResponse;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Parameters are wrong.");
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                string reportContent = "";
                ReportType = ReportType.ToLower();
                var commerceItemIdsToMark = new List<long>();
                string filenameToDownload;

                switch (ReportType)
                {
                    case "centralizerRefunds":
                        var listadoCentralizadorRefunds = this._datacontext.GetReport814Refunds(searchCriteria.TransactionId, searchCriteria.CommerceItem, searchCriteria.MerchantId, searchCriteria.SearchFrom, searchCriteria.SearchTo);
                        reportContent = "TrxId;ExternalId;ElectronicPaymentCode;MerchantId;TrxCurrencyOriginalAmount;TrxOriginalAmount;TrxDateHour;ProductId;CardNumber4Dig;AuthCode;ClaimerDocType;ClaimerDocNumber;ClaimerLastName;ClaimerFirstName;ClaimerEmail;ClaimNumber;ClaimDateHour;CiCodeAnnulled;AnnulmentDateHour;AnnulmentAuthCode";
                        foreach (var lineToReport in listadoCentralizadorRefunds)
                        {
                            reportContent += this.ModelRefundsToLine(lineToReport, false);
                            commerceItemIdsToMark.Add(lineToReport.CommerceItemId);
                        }
                        filenameToDownload = $"{this._datacontext.GetAppConfig("CentralizerRefundFileNameHeader")}{DateTime.Now:yyyyMMdd}.txt";
                        break;

                    case "renditionRefunds":

                        var listadoRenditionRefunds = this._datacontext.GetReportRenditionRefunds(searchCriteria.TransactionId, searchCriteria.CommerceItem, searchCriteria.MerchantId, searchCriteria.SearchFrom, searchCriteria.SearchTo);
                        string filenameServiceSuffix = "";
                        reportContent = "TrxId;";

                        if (string.IsNullOrWhiteSpace(searchCriteria.MerchantId))
                        {
                            reportContent += "ExternalId;";
                            // Esta línea puede causar NullReferenceException si MerchantId es realmente nulo/vacío.
                            ServiceModel serviceInfo = this._datacontext.GetServiceByMerchantId(searchCriteria.MerchantId);
                            if (serviceInfo != null) filenameServiceSuffix = "_" + serviceInfo.Name;
                        }

                        if (listadoRenditionRefunds.Any())
                            reportContent += "ElectronicPaymentCode;MerchantId;TrxCurrencyOriginalAmount;TrxOriginalAmount;TrxDateHour;ProductId;CardNumber4Dig;AuthCode;ClaimerDocType;ClaimerDocNumber;ClaimerLastName;ClaimerFirstName;ClaimerEmail;ClaimNumber;ClaimDateHour;CiCodeAnnulled;AnnulmentDateHour;AnnulmentAuthCode";

                        foreach (var lineToReport in listadoRenditionRefunds)
                        {
                            reportContent += this.ModelRefundsToLine(lineToReport, !string.IsNullOrWhiteSpace(searchCriteria.MerchantId));
                            commerceItemIdsToMark.Add(lineToReport.CommerceItemId);
                        }
                        filenameToDownload = $"{this._datacontext.GetAppConfig("RenditionRefundFileNameHeader")}{DateTime.Now:yyyyMMdd}{filenameServiceSuffix}.csv";
                        break;

                    case "centralizer":
                        var listadoCentralizador = this._datacontext.GetReport814(searchCriteria.TransactionId, searchCriteria.CommerceItem, searchCriteria.MerchantId, searchCriteria.SearchFrom, searchCriteria.SearchTo);
                        foreach (var lineToReport in listadoCentralizador)
                        {
                            reportContent += this.Model814ToLine(lineToReport);
                            commerceItemIdsToMark.Add(lineToReport.CommerceItemId);
                        }
                        filenameToDownload = $"{this._datacontext.GetAppConfig("814FileNameHeader")}{DateTime.Now:yyyyMMdd}.txt";
                        break;

                    case "conciliation":
                        var listadoConciliacion = this._datacontext.GetReportConciliation(searchCriteria.TransactionId, searchCriteria.CommerceItem, searchCriteria.MerchantId, searchCriteria.SearchFrom, searchCriteria.SearchTo);
                        foreach (var lineToReport in listadoConciliacion)
                        {
                            reportContent += this.ModelConciliationToLine(lineToReport);
                            commerceItemIdsToMark.Add(lineToReport.CommerceItemIdPK);
                        }
                        filenameToDownload = $"{this._datacontext.GetAppConfig("ConciliationFileNameHeader")}{DateTime.Now:yyyyMMdd}.txt";
                        break;

                    case "rendition":
                        if (string.IsNullOrWhiteSpace(searchCriteria.MerchantId))
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "El merchant es obligatorio para el reporte de Rendition.");

                        var listadoRendicion = this._datacontext.GetReportRendition(searchCriteria.TransactionId, searchCriteria.CommerceItem, searchCriteria.MerchantId, searchCriteria.SearchFrom, searchCriteria.SearchTo);

                        if (listadoRendicion.Any())
                            reportContent = "ElectronicPaymentCode;TrxID;TrxSource;BatchNbr;CustomerId;CustomerMail;Producto;Country;Currency;Amount;CardHolderName;CardMask;MerchantId;MerchOrderId;MerchTrxRef;NumPayments;Operation;PosDateTime;Code;Description;Price";

                        foreach (var lineToReport in listadoRendicion)
                        {
                            reportContent += this.ModelRenditionToLine(lineToReport);
                            commerceItemIdsToMark.Add(lineToReport.CommerceItemId);
                        }

                        var serviceInfoForFilename = this._datacontext.GetServiceByMerchantId(searchCriteria.MerchantId);
                        if (serviceInfoForFilename == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Servicio del merchant no encontrado para generar el nombre del archivo de rendición.");
                        }
                        filenameToDownload = $"{this._datacontext.GetAppConfig("RenditionFileNameHeader")}{DateTime.Now:yyyyMMdd}_{serviceInfoForFilename.Name}.csv";
                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El tipo de reporte especificado no existe. Las opciones son: Conciliation,Centralizer,Rendition,RenditionRefunds,CentralizerRefunds");
                }

                if (jobRunId != 0)
                    this._datacontext.InsertReportJobRunLog(commerceItemIdsToMark.ToArray(), jobRunId, ReportType);

                response.Content = new StringContent(reportContent, Encoding.UTF8, "text/plain");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filenameToDownload
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return response;
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex, nameof(Post));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error en el proceso.");
            }
        }

        [Authorize(Roles = "apiRePrint,apiAdminServices")]
        [Route("Report/Reprint/")]
        public HttpResponseMessage Post(CriteriaRePrintModel criteria)
        {
            if (!this.ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);

            try
            {
                // Lógica de autorización de roles.
                if (!HttpContext.Current.User.IsInRole("apiAdminServices"))
                {
                    if (!HttpContext.Current.User.IsInRole("apiRePrint"))
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
            }

            try
            {
                var searchDate = DateTime.ParseExact(criteria.Date, "yyyyMMdd", CultureInfo.InvariantCulture);

                var ticketList = this._datacontext.GetTicketInformationToRePrint(searchDate, criteria.ExternalId, criteria.CreditCard4LastDigits, criteria.AuthorizationCode).ToList();

                return ticketList.Any()
                  ? Request.CreateResponse(HttpStatusCode.OK, ticketList)
                  : Request.CreateResponse(HttpStatusCode.NoContent, "No existen datos para ese criterio");
            }
            catch (Exception ex)
            {
                this._utilities.InsertLogException(LogType.Error, ex, "PostReprint");

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error obteniendo datos");
            }
        }
        private string ModelRenditionToLine(ReporteRenditionModel lineToWrite)
        {
            return $"{lineToWrite.ElectronicPaymentCode};{lineToWrite.TrxID};{lineToWrite.TrxSource};{lineToWrite.BatchNbr};{lineToWrite.CustomerId};{lineToWrite.CustomerMail};{lineToWrite.Producto};{lineToWrite.Country};{lineToWrite.Currency};{lineToWrite.Amount};{lineToWrite.CardHolderName?.ToUpper()};{lineToWrite.CardMask?.ToUpper()};{lineToWrite.MerchantId};{lineToWrite.MerchOrderId};{lineToWrite.MerchTrxRef};{lineToWrite.NumPayments};{lineToWrite.Operation};{lineToWrite.PosDateTime};{lineToWrite.Code};{lineToWrite.Description};{lineToWrite.Price}\n";
        }

        private string ModelRefundsToLine(ReporteCentralizadorOrRefundionRefundModel lineToWrite, bool isClientReport)
        {

            string prefix = lineToWrite.TransactionId.ToString() + ";";
            if (!isClientReport)
            {

                if (!isClientReport && lineToWrite.ExternalId != null) // Si no es para cliente Y ExternalId existe
                {
                    prefix = $"{lineToWrite.TransactionId};{lineToWrite.ExternalId};";
                }
                // Si es para cliente, prefix solo tiene TransactionId.
            }

            return $"{prefix}{lineToWrite.ElectronicPaymentCode};{lineToWrite.MerchantId};{lineToWrite.TrxOriginalAmount};{lineToWrite.TrxDateTime};{lineToWrite.ProductId};{lineToWrite.CardNumber4Dig};{lineToWrite.TrxAuthCode};{lineToWrite.ClaimerDocType};{lineToWrite.ClaimerDocNumber};{lineToWrite.ClaimerLastName};{lineToWrite.ClaimerFirstName};{lineToWrite.ClaimerEmail};{lineToWrite.ClaimNumber};{lineToWrite.ClaimDateTime};{lineToWrite.CICodeAnnulled};{lineToWrite.AnnulmentAmount};{lineToWrite.AnnulmentDateTime};{lineToWrite.AnnulmentAuthCode}\n";
        }

        private string Model814ToLine(ReporteCentralizadorModel lineToWrite)
        {
            return $"{lineToWrite.Datos}{lineToWrite.Spacer1}{lineToWrite.CodigoBCRA}{lineToWrite.CodigoR}{this.GetFormattedNumber(lineToWrite.CodigoTerminal.ToString(), '0', 5)}{lineToWrite.Spacer2}{this.GetFormattedNumber(lineToWrite.CodigoSucursal.ToString(), '0', 4)}{lineToWrite.NroSecuenciaOn}{this.GetFormattedNumber(lineToWrite.NroSecuenciaTrans.ToString(), '0', 8)}{lineToWrite.CodigoOperacion}{lineToWrite.Desde}{lineToWrite.Hasta}{lineToWrite.CodigoEnte}{this.GetFormattedString(lineToWrite.CodigoServicio.ToString(), ' ', 19)}{this.GetIntegerFromDecimal(lineToWrite.Importe, '0', 11)}{lineToWrite.Interes}{lineToWrite.Recargo}{lineToWrite.Moneda}{lineToWrite.CodigoCajero}{lineToWrite.FondoEducativo}{lineToWrite.Spacer3}{lineToWrite.CodigoSeguridad}{lineToWrite.FechaVto1}{lineToWrite.FechaVto2}{lineToWrite.BancoCheque}{lineToWrite.BancoSucursal}{lineToWrite.BancoCodPostal}{lineToWrite.NroCheque}{lineToWrite.NroCuenta}{lineToWrite.Plazo}{this.GetFormattedString(lineToWrite.CodigoBarra, ' ', 60)}{lineToWrite.FechaPago:yyMMdd}{lineToWrite.Spacer4}{lineToWrite.AnioCuota}{lineToWrite.FoProvi}{lineToWrite.FormaPago}{lineToWrite.Jurisdiccion}{lineToWrite.Spacer5}{lineToWrite.Autorizacion}{lineToWrite.NroAnulacion}\r\n";
        }

        private string ModelConciliationToLine(ReporteConciliacionModel lineToWrite)
        {
            string tarjetaFinRaw = lineToWrite.TarjetaFin?.Trim() ?? "";
            string tarjetaFinFormatted = tarjetaFinRaw.Length >= 4 ? tarjetaFinRaw.Substring(tarjetaFinRaw.Length - 4, 4) : tarjetaFinRaw.PadLeft(4, '0');

            string tarjetaInicioRaw = lineToWrite.TarjetaInicio?.Trim() ?? "";
            string tarjetaInicioFormatted = tarjetaInicioRaw.Length >= 6 ? tarjetaInicioRaw.Substring(0, 6) : tarjetaInicioRaw.PadLeft(6, '0');

            return $"{this.GetFormattedNumber(lineToWrite.TransactionId.ToString(), '0', 19)}{this.GetFormattedNumber(lineToWrite.CommerceItemId.ToString(), '0', 19)}{this.GetFormattedNumber(lineToWrite.NroCommercio.ToString(), '0', 19)}{this.GetFormattedNumber(lineToWrite.NroEnte, '0', 6)}{lineToWrite.FechaPago:yyyyMMdd}{lineToWrite.HoraPago:HHmmss}{this.GetFormattedNumber(lineToWrite.TipoTarjeta?.ToString(), '0', 4)}{this.GetFormattedNumber(tarjetaInicioFormatted, '0', 6)}{this.GetFormattedNumber(tarjetaFinFormatted, '0', 4)}{this.GetFormattedString(lineToWrite.CodigoAutorizacion, ' ', 6)}{this.GetFormattedNumber(lineToWrite.NroTicket, '0', 4)}{this.GetIntegerFromDecimal(lineToWrite.Importe, '0', 18)}{this.GetFormattedNumber(lineToWrite.NroLote, '0', 4)}{this.GetFormattedString(lineToWrite.MerchantId, ' ', 64)}{this.GetFormattedString(lineToWrite.CodigoBarra, ' ', 68)}\n";
        }

        private string GetFormattedNumber(string data, char fillChar, int repetitions)
        {
            return string.IsNullOrEmpty(data) ? new string(fillChar, repetitions) : data.PadLeft(repetitions, fillChar);
        }

        private string GetFormattedString(string data, char fillChar, int repetitions)
        {
            return string.IsNullOrEmpty(data) ? new string(fillChar, repetitions) : data.PadRight(repetitions, fillChar);
        }

        private string GetIntegerFromDecimal(decimal value, char fillChar, int repetitions)
        {
            int integerPart = (int)value;
            int decimalPart = (int)((value - integerPart) * 100M); // Usar 100M
                                                                   // Asegurar que decimalPart siempre tenga dos dígitos para el string, ej. "05"
            string combinedValue = integerPart.ToString() + decimalPart.ToString("D2");
            return combinedValue.PadLeft(repetitions, fillChar);
        }
        private AuxSearchCriteria GetSearchCriteria(CriteriaModelReportToSearch criteria)
        {
            var criteriaResult = new AuxSearchCriteria { DataValidationResponse = null };

            bool isStartDefined = criteria != null && !string.IsNullOrWhiteSpace(criteria.SearchFrom);
            bool isEndDefined = criteria != null && !string.IsNullOrWhiteSpace(criteria.SearchTo);

            if (isStartDefined)
            {
                criteriaResult.SearchFrom = DateTime.ParseExact(criteria.SearchFrom, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else
            {
                double startDiffDays = double.Parse(this._datacontext.GetAppConfig("ReportStartDateDefaultDateFromToday"));
                criteriaResult.SearchFrom = DateTime.Now.AddDays(startDiffDays);
            }

            if (isEndDefined)
            {
                criteriaResult.SearchTo = DateTime.ParseExact(criteria.SearchTo, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else
            {
                double endDiffDays = double.Parse(this._datacontext.GetAppConfig("ReportDaysQuantityDefaultDateFromToday")) - 1.0;
                criteriaResult.SearchTo = criteriaResult.SearchFrom.AddDays(endDiffDays);
            }

            criteriaResult.SearchFrom = new DateTime(criteriaResult.SearchFrom.Year, criteriaResult.SearchFrom.Month, criteriaResult.SearchFrom.Day, 0, 0, 0);
            criteriaResult.SearchTo = new DateTime(criteriaResult.SearchTo.Year, criteriaResult.SearchTo.Month, criteriaResult.SearchTo.Day, 23, 59, 59);

            criteriaResult.MerchantId = (criteria == null || string.IsNullOrWhiteSpace(criteria.MerchantId)) ? null : criteria.MerchantId;

            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.CommerceItemCode))
                {
                    if (!string.IsNullOrWhiteSpace(criteria.TransactionId))
                    {
                        criteriaResult.FilterByCommerceItem = true;
                        criteriaResult.CommerceItem = criteria.CommerceItemCode;
                    }
                    else
                        criteriaResult.DataValidationResponse = Request.CreateResponse(HttpStatusCode.BadRequest, "Si especifica un Commerce Item, debe especificar obligatoriamente un TransactionId");
                }
                else
                    criteriaResult.CommerceItem = null;

                if (!string.IsNullOrWhiteSpace(criteria.TransactionId))
                {
                    criteriaResult.FilterByTransactionId = true;
                    criteriaResult.TransactionId = criteria.TransactionId;
                }
                else
                    criteriaResult.TransactionId = null;
            }
            else
            {
                criteriaResult.CommerceItem = null;
                criteriaResult.TransactionId = null;
            }
            return criteriaResult;
        }
    }
}