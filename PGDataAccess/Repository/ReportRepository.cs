using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataAccess.Models;
using System.Data;
using System.Data.Entity;
using PGDataAccess.Tools;
namespace PGDataAccess.Repository
{

    public class ReportRepository
    {
        #region Reimpresión de comprobantes

        public static List<RePrintModel> GetTicketInformationToRePrint(DateTime PurchaseDate, int ExternalId, string CreditCard4LastDigits, string AuthorizationCode)
        {
            List<RePrintModel> RePrintToReturn = new List<RePrintModel>();

            using (var context = new EF.PGDataEntities())
            {
                RePrintToReturn = (from trans in context.Transactions
                                   join tai in context.TransactionAdditionalInfo
                                        on trans.Id equals tai.TransactionIdPK
                                   join tri in context.TransactionResultInfo
                                        on trans.Id equals tri.TransactionIdPK
                                   join serv in context.Services
                                        on tai.ServiceId equals serv.ServiceId
                                   join ci in context.CommerceItems
                                        on trans.Id equals ci.TransactionIdPK
                                   join ts in context.TransactionStatus
                                        on trans.Id equals ts.TransactionsId
                                   join sc in context.StatusCode
                                        on ts.StatusCodeId equals sc.StatusCodeId
                                   join servConf in context.ServicesConfig
                                        on serv.ServiceId equals servConf.ServiceId

                                   where tri.CardNbrLfd == CreditCard4LastDigits
                                        && (DbFunctions.TruncateTime(trans.CreatedOn) >= PurchaseDate.Date && DbFunctions.TruncateTime(trans.CreatedOn) <= PurchaseDate.Date)
                                        && sc.Code == "2"
                                        && servConf.ExternalId == ExternalId
                                        && tri.AuthorizationCode == AuthorizationCode

                                   select new RePrintModel()
                                   {
                                       //CHANGE 4.1 TransactionNumber PBI 2528
                                       //Old
                                       //TransactionId = trans.TransactionId,
                                       //New
                                       TransactionNumber = tai.TransactionNumber,
                                       ElectronicPaymentCode = trans.ElectronicPaymentCode,
                                       CurrencyCode = trans.CurrencyCode,
                                       CreditCardDescription = trans.Product,
                                       PaymentDate = trans.CreatedOn,
                                       Fees = tri.Payments.ToString(),
                                       CreditCardNumber = tri.CardMask,
                                       SecurityCode = tri.AuthorizationCode,
                                       TicketNumber = tri.TicketNumber,
                                       CardNbrLfd = tri.CardNbrLfd,
                                       Amount = ci.Amount.ToString(),
                                       BarCode = ci.Code,
                                       ExternalId = servConf.ExternalId,
                                       EntityName = serv.Description,
                                       TerminalId = servConf.TerminalId
                                   }).ToList();

                foreach (RePrintModel toUpdate in RePrintToReturn)
                {
                    toUpdate.TransactionId = CustomTool.ConvertTNtoTransaction(toUpdate.TransactionNumber);
                }

            }

            return RePrintToReturn;
        }

        #endregion

        #region Reportes de Rendición, Centralizador, Conciliación

        #region 814 - Centralizer

        public static List<ReporteCentralizadorOrRefundionRefundModel> Get814Refunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            List<ReporteCentralizadorOrRefundionRefundModel> ItemsWithCI = new List<ReporteCentralizadorOrRefundionRefundModel>();
            long transactionNumber = 0;

            if (transactionId != null)
                transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
            using (var context = new EF.PGDataEntities())
            {
                var servicesToReport = new List<int>();

                if (string.IsNullOrWhiteSpace(merchantId))
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        where  servconf.RptToCentralizer
                                        select servconf.ServiceId).ToList();
                }
                else if (transactionId != null)
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        join tai in context.TransactionAdditionalInfo
                                        on serv.ServiceId equals tai.ServiceId
                                        where servconf.RptToCentralizer && tai.TransactionNumber == transactionNumber
                                        select servconf.ServiceId).ToList();
                }
                else
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        where (servconf.RptToCentralizer) && (serv.MerchantId == merchantId)
                                        select servconf.ServiceId).ToList();

                }
                if (!servicesToReport.Any())
                {
                    return null;
                }

                //ADD ITEMS WITH COMMERCE ITEMS
                ItemsWithCI = (from t in context.Transactions
                               join tri in context.TransactionResultInfo
                               on t.Id equals tri.TransactionIdPK
                               join tai in context.TransactionAdditionalInfo
                               on t.Id equals tai.TransactionIdPK
                               join services in context.Services
                               on tai.ServiceId equals services.ServiceId
                               join servconfig in context.ServicesConfig
                               on services.ServiceId equals servconfig.ServiceId
                               join ci in context.CommerceItems
                               on t.Id equals ci.TransactionIdPK
                               //join clients in context.Clients
                               //on tai.ClientId equals clients.ClientId
                               join ts in context.TransactionStatus
                               on t.Id equals ts.TransactionsId
                               join servicesConfigToReport in servicesToReport
                               on services.ServiceId equals servicesConfigToReport
                               join pc in context.PaymentClaim
                               on t.Id equals pc.TransactionId
                               join claimci in context.CommerceItemClaim
                               on ci.CommerceItemsId equals claimci.CommerceItemId
                               join ari in context.AnnulmentResultInfo
                               on pc.PaymentClaimId equals ari.PaymentClaimId

                               where (ts.StatusCodeId == 36 || ts.StatusCodeId == 49 || ts.StatusCodeId == 50)
                                  && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                  && (CommerceItemCode == null ? true : ci.Code == CommerceItemCode)
                                  && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)

                               select new ReporteCentralizadorOrRefundionRefundModel
                               {
                                   CommerceItemId = ci.CommerceItemsId,
                                   TransactionId = tai.TransactionNumber,
                                   ExternalId = servconfig.ExternalId,
                                   ElectronicPaymentCode = t.ElectronicPaymentCode,
                                   MerchantId = tai.MerchantId,
                                   TrxOriginalAmount = t.Amount,
                                   TrxDateTime = t.CreatedOn,
                                   ProductId = tai.ProductId,
                                   CardNumber4Dig = tri.CardNbrLfd,
                                   TrxAuthCode = tri.AuthorizationCode,
                                   ClaimerDocType = pc.Claimer.DocType.ShortName,
                                   ClaimerDocNumber = pc.Claimer.DocNumber,
                                   ClaimerLastName = pc.Claimer.LastName,
                                   ClaimerFirstName = pc.Claimer.Name,
                                   ClaimerEmail = pc.Claimer.Email,
                                   ClaimNumber = pc.PaymentClaimNumber,
                                   ClaimDateTime = pc.CreatedOn,
                                   CICodeAnnulled = ci.Code,
                                   AnnulmentAmount = pc.Amount,
                                   AnnulmentDateTime = ari.CreatedOn,
                                   AnnulmentAuthCode = ari.AuthorizationCode
                               }).ToList();

            }

            return ItemsWithCI.Any() ? ItemsWithCI : null;
        }

        public static List<ReporteCentralizadorModel> Get814(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            List<ReporteCentralizadorModel> DatosARetornar = new List<ReporteCentralizadorModel>();
            List<ReporteCentralizadorModel> ItemsWithCI = new List<ReporteCentralizadorModel>();
            List<ReporteCentralizadorModel> ItemsWOCI = new List<ReporteCentralizadorModel>();

            long transactionNumber = 0;

            if (transactionId != null)
                transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);

            //GET ALL SERVICES TO REPORT TO CENTRALIZER
            using (var context = new EF.PGDataEntities())
            {
                LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", transactionNumber));
                context.Database.Log = s => LogRepository.InsertLogCommon(LogTypeModel.Info, s);
                List<int> servicesToReport = new List<int>();

                if (string.IsNullOrWhiteSpace(merchantId))
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        where servconf.RptToCentralizer
                                        select servconf.ServiceId).ToList();
                }
                else if (transactionId != null)
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        join tai in context.TransactionAdditionalInfo
                                        on serv.ServiceId equals tai.ServiceId
                                        where servconf.RptToCentralizer && tai.TransactionNumber == transactionNumber
                                        select servconf.ServiceId).ToList();
                }
                else
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        where (servconf.RptToCentralizer) && (serv.MerchantId == merchantId)
                                        select servconf.ServiceId).ToList();

                }
                if (!servicesToReport.Any())
                {
                    return null;
                }

                //ADD ITEMS WITH COMMERCE ITEMS
                ItemsWithCI = (from t in context.Transactions
                               join tri in context.TransactionResultInfo
                               on t.Id equals tri.TransactionIdPK
                               join tai in context.TransactionAdditionalInfo
                               on t.Id equals tai.TransactionIdPK
                               join services in context.Services
                               on tai.ServiceId equals services.ServiceId
                               join servconfig in context.ServicesConfig
                               on services.ServiceId equals servconfig.ServiceId
                               join ci in context.CommerceItems
                               on t.Id equals ci.TransactionIdPK
                               join clients in context.Clients
                               on tai.ClientId equals clients.ClientId
                               join ts in context.TransactionStatus
                               on t.Id equals ts.TransactionsId
                               join servicesConfigToReport in servicesToReport
                               on services.ServiceId equals servicesConfigToReport

                               where (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                                  && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                  && (CommerceItemCode == null ? true : ci.Code == CommerceItemCode)
                                  && (servconfig.RenditionType == 1)
                                  && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)



                               select new ReporteCentralizadorModel
                               {
                                   CommerceItemId = ci.CommerceItemsId,
                                   Datos = "Datos",
                                   Spacer1 = new String(' ', 3),
                                   CodigoBCRA = "0814",
                                   CodigoR = "R",
                                   CodigoTerminal = servconfig.TerminalId,
                                   Spacer2 = new String(' ', 10),
                                   CodigoSucursal = servconfig.BranchId,
                                   NroSecuenciaOn = new String('0', 8),
                                   NroSecuenciaTrans = ci.CommerceItemsId,
                                   CodigoOperacion = "A3",
                                   Desde = new String('0', 2),
                                   Hasta = new String('0', 2),
                                   CodigoEnte = servconfig.ExternalId,
                                   CodigoServicio = ci.TransactionIdPK,
                                   Importe = ci.Amount,
                                   Interes = new String('0', 11),
                                   Recargo = new String('0', 11),
                                   Moneda = new String('0', 1),
                                   CodigoCajero = new String('0', 4),
                                   FondoEducativo = new String('0', 3),
                                   
                                   Spacer3 = new String(' ', 2),
                                   CodigoSeguridad = new String('0', 3),
                                   FechaVto1 = new String('0', 6),
                                   FechaVto2 = new String('0', 6),
                                   BancoCheque = new String('0', 3),
                                   BancoSucursal = new String('0', 3),
                                   BancoCodPostal = new String('0', 4),
                                   NroCheque = new String('0', 8),
                                   NroCuenta = new String('0', 8),
                                   Plazo = new String('0', 3),
                                   CodigoBarra = ci.Code,
                                   FechaPago = t.CreatedOn,
                                   Spacer4 = new String(' ', 1),
                                   AnioCuota = new String('0', 7),
                                   FoProvi = new String('0', 9),
                                   FormaPago = new String('0', 2),//check
                                   Jurisdiccion = new String('0', 4),
                                   Spacer5 = new String(' ', 3),
                                   Autorizacion = new String('0', 15),
                                   NroAnulacion = new String('0', 8)
                               }).ToList();

                //ADD ITEMS WITHOUT COMMERCE ITEMS
                if (CommerceItemCode == null)
                {
                    ItemsWOCI = (from t in context.Transactions
                                 join tri in context.TransactionResultInfo
                                 on t.Id equals tri.TransactionIdPK
                                 join tai in context.TransactionAdditionalInfo
                                 on t.Id equals tai.TransactionIdPK
                                 join services in context.Services
                                 on tai.ServiceId equals services.ServiceId
                                 join servconfig in context.ServicesConfig
                                 on services.ServiceId equals servconfig.ServiceId
                                 join clients in context.Clients
                                 on tai.ClientId equals clients.ClientId
                                 join ts in context.TransactionStatus
                                 on t.Id equals ts.TransactionsId
                                 join servicesConfigToReport in servicesToReport
                                on services.ServiceId equals servicesConfigToReport

                                 where (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                                     && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                     && (servconfig.RenditionType == 0)
                                     && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)

                                 select new ReporteCentralizadorModel
                                 {
                                     CommerceItemId = 0,
                                     Datos = "Datos",
                                     Spacer1 = new String(' ', 3),
                                     CodigoBCRA = "0814",
                                     CodigoR = "R",
                                     CodigoTerminal = servconfig.TerminalId,
                                     Spacer2 = new String(' ', 10),
                                     CodigoSucursal = servconfig.BranchId,
                                     NroSecuenciaOn = new String('0', 8),
                                     NroSecuenciaTrans = (int)t.Id, //REPLACED FOR RENDITION TYPE TOTAL
                                     CodigoOperacion = "A3",
                                     Desde = new String('0', 2),
                                     Hasta = new String('0', 2),
                                     CodigoEnte = servconfig.ExternalId,
                                     CodigoServicio = t.Id, //REPLACED FOR RENDITION TYPE TOTAL
                                     Importe = t.Amount, //REPLACED FOR RENDITION TYPE TOTAL
                                     Interes = new String('0', 11),
                                     Recargo = new String('0', 11),
                                     Moneda = new String('0', 1),
                                     CodigoCajero = new String('0', 4),
                                     FondoEducativo = new String('0', 3),
                                     Spacer3 = new String(' ', 2),
                                     CodigoSeguridad = new String('0', 3),
                                     FechaVto1 = new String('0', 6),
                                     FechaVto2 = new String('0', 6),
                                     BancoCheque = new String('0', 3),
                                     BancoSucursal = new String('0', 3),
                                     BancoCodPostal = new String('0', 4),
                                     NroCheque = new String('0', 8),
                                     NroCuenta = new String('0', 8),
                                     Plazo = new String('0', 3),
                                     CodigoBarra = t.ElectronicPaymentCode, //REPLACED FOR RENDITION TYPE TOTAL
                                     FechaPago = t.CreatedOn,
                                     Spacer4 = new String(' ', 1),
                                     AnioCuota = new String('0', 7),
                                     FoProvi = new String('0', 9),
                                     FormaPago = new String('0', 2),
                                     Jurisdiccion = new String('0', 4),
                                     Spacer5 = new String(' ', 3),
                                     Autorizacion = new String('0', 15),
                                     NroAnulacion = new String('0', 8)
                                 }).ToList();
                }
            }

            if (ItemsWithCI.Any())
            {
                DatosARetornar.AddRange(ItemsWithCI);
            }
            if (ItemsWOCI.Any())
            {
                DatosARetornar.AddRange(ItemsWOCI);
            }

            return DatosARetornar;
        }

        #endregion

        #region Conciliation


        public static List<ReporteConciliacionModel> GetConciliation(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            List<ReporteConciliacionModel> DatosARetornar = new List<ReporteConciliacionModel>();
            List<ReporteConciliacionModel> ItemsWithCI = new List<ReporteConciliacionModel>();
            List<ReporteConciliacionModel> ItemsWOCI = new List<ReporteConciliacionModel>();

            long transactionNumber = 0;

            if (transactionId != null)
                transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);

            string NroDeLote = "";

            //GET ALL SERVICES TO REPORT TO CONCILIATION
            using (var context = new EF.PGDataEntities())
            {
                List<int> servicesToReport = new List<int>();

                NroDeLote = (
                    from appconf in context.AppConfig
                    where appconf.Setting == "NumeroDeLote" && appconf.IsActive
                    select appconf.Value).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(merchantId))
                {
                    servicesToReport = (
                        from servconf in context.ServicesConfig
                        where servconf.RptToConciliation
                        select servconf.ServiceId
                        ).ToList();
                }
                else if (transactionId != null)
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        join tai in context.TransactionAdditionalInfo
                                        on serv.ServiceId equals tai.ServiceId
                                        where servconf.RptToConciliation && tai.TransactionNumber == transactionNumber
                                        select servconf.ServiceId).ToList();
                }
                else
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        where (servconf.RptToConciliation) && (serv.MerchantId == merchantId)
                                        select servconf.ServiceId).ToList();
                }
                if (!servicesToReport.Any())
                {
                    return null;
                }

                //ADD ITEMS WITH COMMERCE ITEMS
                ItemsWithCI = (from t in context.Transactions
                                 join tri in context.TransactionResultInfo
                                 on t.Id equals tri.TransactionIdPK
                                 join tai in context.TransactionAdditionalInfo
                                 on t.Id equals tai.TransactionIdPK
                                 join services in context.Services
                                 on tai.ServiceId equals services.ServiceId
                                 join servconfig in context.ServicesConfig
                                 on services.ServiceId equals servconfig.ServiceId
                                 join ci in context.CommerceItems
                                 on t.Id equals ci.TransactionIdPK
                                 join ts in context.TransactionStatus
                                 on t.Id equals ts.TransactionsId
                                 join config in context.Configurations
                                 on new { u1 = tai.ServiceId, u2 = tai.ProductId, u3 = tai.ChannelId, u4 = tai.ValidatorId } equals new { u1 = config.ServiceId, u2 = config.ProductId, u3 = config.ChannelId, u4 = config.ValidatorId }
                                 join prodCentralizer in context.ProductCentralizer
                                 on tai.ProductId equals prodCentralizer.ProductId
                                 join servicesConfigToReport in servicesToReport
                                 on services.ServiceId equals servicesConfigToReport
                                 where (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                                    && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                    && (CommerceItemCode == null ? true : ci.Code == CommerceItemCode)
                                    && servconfig.RenditionType == 1
                                    && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)
                               select new ReporteConciliacionModel
                               {
                                          CommerceItemIdPK = ci.CommerceItemsId,
                                          TransactionId = ci.CommerceItemsId.ToString(),
                                          CommerceItemId = ci.TransactionIdPK,
                                          NroCommercio = config.CommerceNumber,
                                          NroEnte = servconfig.ExternalId.ToString(),
                                          FechaPago = t.CreatedOn,
                                          HoraPago = t.CreatedOn,
                                          TipoTarjeta = prodCentralizer.CentralizerCode,
                                          TarjetaInicio = tri.CardMask,
                                          TarjetaFin = tri.CardNbrLfd,
                                          CodigoAutorizacion = tri.AuthorizationCode,
                                          NroTicket = tri.TicketNumber,
                                          Importe = ci.Amount,
                                          NroLote = NroDeLote,
                                          MerchantId = config.UniqueCode,
                                          CodigoBarra = ci.Code
                               }).ToList();

                //ADD ITEMS WITHOUT COMMERCE ITEMS

                ItemsWOCI = (from t in context.Transactions
                             join tri in context.TransactionResultInfo
                             on t.Id equals tri.TransactionIdPK
                             join tai in context.TransactionAdditionalInfo
                             on t.Id equals tai.TransactionIdPK
                             join services in context.Services
                             on tai.ServiceId equals services.ServiceId
                             join servconfig in context.ServicesConfig
                             on services.ServiceId equals servconfig.ServiceId
                             join ts in context.TransactionStatus
                             on t.Id equals ts.TransactionsId
                             join config in context.Configurations
                             on new { u1 = tai.ServiceId, u2 = tai.ProductId, u3 = tai.ChannelId, u4 = tai.ValidatorId } equals new { u1 = config.ServiceId, u2 = config.ProductId, u3 = config.ChannelId, u4 = config.ValidatorId }
                             join prodCentralizer in context.ProductCentralizer
                             on tai.ProductId equals prodCentralizer.ProductId
                             join servicesConfigToReport in servicesToReport
                             on services.ServiceId equals servicesConfigToReport
                             where (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                             && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                             && servconfig.RenditionType == 1
                             && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)
                             select new ReporteConciliacionModel
                             {
                                 CommerceItemIdPK = 0,
                                 //CHANGE 4.1 TransactionNumber PBI 2528
                                 //Old
                                 //TransactionId = t.TransactionId,
                                 //New
                                 TransactionNumber = tai.TransactionNumber,
                                 CommerceItemId = t.Id,
                                 NroCommercio = config.CommerceNumber,
                                 NroEnte = servconfig.ExternalId.ToString(),
                                 FechaPago = t.CreatedOn,
                                 HoraPago = t.CreatedOn,
                                 TipoTarjeta = prodCentralizer.CentralizerCode,
                                 TarjetaInicio = tri.CardMask,
                                 TarjetaFin = tri.CardNbrLfd,
                                 CodigoAutorizacion = tri.AuthorizationCode,
                                 NroTicket = tri.TicketNumber,
                                 Importe = t.Amount,
                                 NroLote = NroDeLote,
                                 MerchantId = config.UniqueCode,
                                 CodigoBarra = t.ElectronicPaymentCode
                             }).ToList();
            }

            if (ItemsWithCI.Any())
            {
                DatosARetornar.AddRange(ItemsWithCI);
            }
            if (ItemsWOCI.Any())
            {
                DatosARetornar.AddRange(ItemsWOCI);
            }

            if (DatosARetornar.Any())
            {
                foreach (ReporteConciliacionModel toUpdate in DatosARetornar)
                {
                    toUpdate.TransactionId = CustomTool.ConvertTNtoTransaction(toUpdate.TransactionNumber);
                }
            }
            return DatosARetornar;
        }

        #endregion

        #region Rendition

        public static List<ReporteRenditionModel> GetRendition(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            List<ReporteRenditionModel> DatosARetornar = new List<ReporteRenditionModel>();
            List<ReporteRenditionModel> ItemsWithCI = new List<ReporteRenditionModel>();
            List<ReporteRenditionModel> ItemsWOCI = new List<ReporteRenditionModel>();

            long transactionNumber = 0;

            if (transactionId != null)
                transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);

            string NroDeLote = "";
            string NPSCountryCode = "";
            string NPSCurrencyCode = "";
            string NPSOperation = "";

            //GET ALL SERVICES TO REPORT TO CONCILIATION
            using (var context = new EF.PGDataEntities())
            {
                NroDeLote = AppConfigRepository.GetAppConfiguration("NumeroDeLote");
                NPSCountryCode = AppConfigRepository.GetAppConfiguration("NpsCountryCode");
                NPSCurrencyCode = AppConfigRepository.GetAppConfiguration("NpsCurrencyCode");
                NPSOperation = AppConfigRepository.GetAppConfiguration("NPSOperation");

                List<int> servicesToReport = new List<int>();

                if (string.IsNullOrWhiteSpace(merchantId))
                {
                    servicesToReport = (
                        from servconf in context.ServicesConfig
                        where servconf.RptToRendition
                        select servconf.ServiceId
                        ).ToList();
                }
                else if (transactionId != null)
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        join tai in context.TransactionAdditionalInfo
                                        on serv.ServiceId equals tai.ServiceId
                                        where servconf.RptToRendition && tai.TransactionNumber == transactionNumber
                                        select servconf.ServiceId).ToList();
                }
                else
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        where (servconf.RptToRendition) && (serv.MerchantId == merchantId)
                                        select servconf.ServiceId).ToList();
                }
                if (!servicesToReport.Any())
                {
                    return null;
                }

                //ADD ITEMS WITH COMMERCE ITEMS
                ItemsWithCI =(from t in context.Transactions
                                 join tri in context.TransactionResultInfo
                                 on t.Id equals tri.TransactionIdPK
                                 join tai in context.TransactionAdditionalInfo
                                 on t.Id equals tai.TransactionIdPK
                                 join chan in context.Channels
                                 on tai.ChannelId equals chan.ChannelId
                                 join services in context.Services
                                 on tai.ServiceId equals services.ServiceId
                                 join servconfig in context.ServicesConfig
                                 on services.ServiceId equals servconfig.ServiceId
                                 join ci in context.CommerceItems
                                 on t.Id equals ci.TransactionIdPK
                                 join ts in context.TransactionStatus
                                 on t.Id equals ts.TransactionsId
                                 join products in context.Products
                                on tai.ProductId equals products.ProductId
                                 join servicesConfigToReport in servicesToReport
                                on services.ServiceId equals servicesConfigToReport
                                 where
                                (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                                && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                && (CommerceItemCode == null ? true : ci.Code == CommerceItemCode)
                                && servconfig.RenditionType == 1
                                && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)
                                 select new ReporteRenditionModel
                                 {
                                     CommerceItemId = ci.CommerceItemsId,
                                     ElectronicPaymentCode = t.ElectronicPaymentCode,
                                     //CHANGE 4.1 TransactionNumber PBI 2528
                                     //Old
                                     //TrxID = t.TransactionId,
                                     //New
                                     TransactionNumber = tai.TransactionNumber,
                                     TrxSource = chan.Name,
                                     BatchNbr = "", //TO CHECK
                                     CustomerId = "", //TO CHECK
                                     CustomerMail = tri.CustomerEmail,
                                     Producto = products.Description,
                                     Country = NPSCountryCode,
                                     Currency = NPSCurrencyCode,
                                     Amount = t.Amount.ToString(),
                                     CardHolderName = tri.CardHolder,
                                     CardMask = tri.CardMask,
                                     MerchantId = services.MerchantId,
                                     MerchOrderId = t.InternalNbr, //TO CHECK
                                     MerchTrxRef = t.InternalNbr, //TO CHECK
                                     NumPayments = tri.Payments.ToString(),
                                     Operation = NPSOperation,
                                     PosDateTime = tri.CreatedOn.ToString(),
                                     Code = ci.Code,
                                     Description = ci.Description,
                                     Price = ci.Amount.ToString(),
                                 }).ToList();

                //ADD ITEMS WITHOUT COMMERCE ITEMS
                ItemsWOCI =
                            (from t in context.Transactions
                             join tri in context.TransactionResultInfo
                             on t.Id equals tri.TransactionIdPK
                             join tai in context.TransactionAdditionalInfo
                             on t.Id equals tai.TransactionIdPK
                             join chan in context.Channels
                             on tai.ChannelId equals chan.ChannelId
                             join services in context.Services
                             on tai.ServiceId equals services.ServiceId
                             join servconfig in context.ServicesConfig
                             on services.ServiceId equals servconfig.ServiceId
                             join ts in context.TransactionStatus
                             on t.Id equals ts.TransactionsId
                             join products in context.Products
                            on tai.ProductId equals products.ProductId
                             join servicesConfigToReport in servicesToReport
                            on services.ServiceId equals servicesConfigToReport
                             where
                             (ts.StatusCodeId == 5 || ts.StatusCodeId == 49)
                             && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                             && servconfig.RenditionType == 0
                             && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)
                             select new ReporteRenditionModel
                             {
                                 CommerceItemId = 0,
                                 ElectronicPaymentCode = t.ElectronicPaymentCode,
                                 //CHANGE 4.1 TransactionNumber PBI 2528
                                 //Old
                                 //TrxID = t.TransactionId,
                                 //New
                                 TransactionNumber = tai.TransactionNumber,
                                 TrxSource = chan.Name,
                                 BatchNbr = "", //TO CHECK
                                 CustomerId = "", //TO CHECK
                                 CustomerMail = tri.CustomerEmail,
                                 Producto = products.Description,
                                 Country = NPSCountryCode,
                                 Currency = NPSCurrencyCode,
                                 Amount = t.Amount.ToString(),
                                 CardHolderName = tri.CardHolder,
                                 CardMask = tri.CardMask,
                                 MerchantId = services.MerchantId,
                                 MerchOrderId = t.InternalNbr, //TO CHECK
                                 MerchTrxRef = t.InternalNbr, //TO CHECK
                                 NumPayments = tri.Payments.ToString(),
                                 Operation = NPSOperation,
                                 PosDateTime = tri.CreatedOn.ToString(),
                                 Code = tai.BarCode,
                                 Description = "", //TO CHECK
                                 Price = t.Amount.ToString(),
                             }).ToList();
            }

            if (ItemsWithCI.Any())
            {
                DatosARetornar.AddRange(ItemsWithCI);
            }
            if (ItemsWOCI.Any())
            {
                DatosARetornar.AddRange(ItemsWOCI);
            }

            if (DatosARetornar.Any())
            {
                foreach (ReporteRenditionModel toUpdate in DatosARetornar)
                {
                    toUpdate.TrxID = CustomTool.ConvertTNtoTransaction(toUpdate.TransactionNumber);
                }
            }
            return DatosARetornar;
        }

        public static List<ReporteCentralizadorOrRefundionRefundModel> GetRenditionRefunds(string transactionId, string CommerceItemCode, string merchantId, DateTime RangoDesde, DateTime RangoHasta)
        {
            List<ReporteCentralizadorOrRefundionRefundModel> ItemsWithCI = new List<ReporteCentralizadorOrRefundionRefundModel>();
            long transactionNumber = 0;

            if (transactionId != null)
                transactionNumber = CustomTool.ConvertTransactionToTN(transactionId);
            using (var context = new EF.PGDataEntities())
            {
                List<int> servicesToReport = new List<int>();

                if (string.IsNullOrWhiteSpace(merchantId))
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        where servconf.RptToRendition
                                        select servconf.ServiceId).ToList();
                }
                else if (transactionId != null)
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        join tai in context.TransactionAdditionalInfo
                                        on serv.ServiceId equals tai.ServiceId
                                        where servconf.RptToRendition && tai.TransactionNumber == transactionNumber
                                        select servconf.ServiceId).ToList();
                }
                else
                {
                    servicesToReport = (from servconf in context.ServicesConfig
                                        join serv in context.Services
                                        on servconf.ServiceId equals serv.ServiceId
                                        where (servconf.RptToRendition) && (serv.MerchantId == merchantId)
                                        select servconf.ServiceId).ToList();

                }
                if (!servicesToReport.Any())
                {
                    return null;
                }

                //ADD ITEMS WITH COMMERCE ITEMS
                ItemsWithCI = (from t in context.Transactions
                               join tri in context.TransactionResultInfo
                               on t.Id equals tri.TransactionIdPK
                               join tai in context.TransactionAdditionalInfo
                               on t.Id equals tai.TransactionIdPK
                               join services in context.Services
                               on tai.ServiceId equals services.ServiceId
                               join servconfig in context.ServicesConfig
                               on services.ServiceId equals servconfig.ServiceId
                               join ci in context.CommerceItems
                               on t.Id equals ci.TransactionIdPK
                               //join clients in context.Clients
                               //on tai.ClientId equals clients.ClientId
                               join ts in context.TransactionStatus
                               on t.Id equals ts.TransactionsId
                               join servicesConfigToReport in servicesToReport
                               on services.ServiceId equals servicesConfigToReport
                               join pc in context.PaymentClaim
                               on t.Id equals pc.TransactionId
                               join claimci in context.CommerceItemClaim
                               on ci.CommerceItemsId equals claimci.CommerceItemId
                               join ari in context.AnnulmentResultInfo
                               on pc.PaymentClaimId equals ari.PaymentClaimId

                               where (ts.StatusCodeId == 36 || ts.StatusCodeId == 49 || ts.StatusCodeId == 50)
                                  && (transactionId == null ? true : tai.TransactionNumber == transactionNumber)
                                  && (CommerceItemCode == null ? true : ci.Code == CommerceItemCode)
                                  && (DbFunctions.TruncateTime(t.CreatedOn) >= RangoDesde.Date && DbFunctions.TruncateTime(t.CreatedOn) <= RangoHasta.Date)

                               select new ReporteCentralizadorOrRefundionRefundModel
                               {
                                   CommerceItemId = ci.CommerceItemsId,
                                   TransactionId = tai.TransactionNumber,
                                   ExternalId = servconfig.ExternalId,
                                   ElectronicPaymentCode = t.ElectronicPaymentCode,
                                   MerchantId = tai.MerchantId,
                                   TrxOriginalAmount = t.Amount,
                                   TrxDateTime = t.CreatedOn,
                                   ProductId = tai.ProductId,
                                   CardNumber4Dig = tri.CardNbrLfd,
                                   TrxAuthCode = tri.AuthorizationCode,
                                   ClaimerDocType = pc.Claimer.DocType.ShortName,
                                   ClaimerDocNumber = pc.Claimer.DocNumber,
                                   ClaimerLastName = pc.Claimer.LastName,
                                   ClaimerFirstName = pc.Claimer.Name,
                                   ClaimerEmail = pc.Claimer.Email,
                                   ClaimNumber = pc.PaymentClaimNumber,
                                   ClaimDateTime = pc.CreatedOn,
                                   CICodeAnnulled = ci.Code,
                                   AnnulmentAmount = pc.Amount,
                                   AnnulmentDateTime = ari.CreatedOn,
                                   AnnulmentAuthCode = ari.AuthorizationCode
                               }).ToList();

            }
            if (ItemsWithCI.Any())
            {
                return ItemsWithCI;
            }
            else
            {
                return null;
            }
        }


        #endregion

        #endregion
    }
}