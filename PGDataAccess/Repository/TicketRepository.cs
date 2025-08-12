using PGDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGDataAccess.Repository
{
    public class TicketRepository
    {
        public static List<TicketsModel> GetTicket_Payment(long transactionNumber, string langCode) {
            try
            {
                using (var context = new EF.PGDataEntities())
                {
                    var ticketmodel = (from tt in context.TransactionTicket
                                       join tai in context.TransactionAdditionalInfo
                                       on tt.TransactionIdPK equals tai.TransactionIdPK
                                       join stemp in context.StatusTemplate
                                       on tt.StatusTemplateId equals stemp.StatusTemplateId
                                       join sc in context.StatusCode
                                       on stemp.StatusCodeId equals sc.StatusCodeId
                                       join mes in context.StatusMessage
                                       on sc.StatusCodeId equals mes.StatusCodeId
                                       join lang in context.Language
                                       on mes.IdLanguage equals lang.Id
                                       where tai.TransactionNumber == transactionNumber && lang.ISO6391 == langCode
                                       select new TicketsModel()
                                       {
                                           TransactionNumber = transactionNumber,
                                           TicketNumber = tt.TicketNumber,
                                           NotificationSent = tt.EmailSent,
                                           StatusCode = sc.Code,
                                           StatusMessage = mes.Message

                                       }).ToList();
                    return ticketmodel;
                }
            }
            catch (Exception ex) {
                LogRepository.InsertLogException(LogTypeModel.Error, ex, TransactionNumber: transactionNumber.ToString(), logtransactionType: LogTransactionType.TransactionNumber);
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static List<TicketModel> GetTicket_PaymentClaim(long claimNumber, string lang)
        {
            try
            {
                using (var context = new EF.PGDataEntities())
                {
                    var ticketmodels = from tl in context.TicketLog
                                    join pcs in context.PaymentClaimStatus
                                    on tl.TicketNumber equals pcs.TicketNumber
                                    join pc in context.PaymentClaim
                                    on pcs.PaymentClaimId equals pc.PaymentClaimId
                                    join sc in context.StatusCode
                                    on pcs.StatusCodeId equals sc.StatusCodeId
                                    join sm in context.StatusMessage
                                    on pcs.StatusCodeId equals sm.StatusCodeId
                                    join lng in context.Language
                                    on sm.IdLanguage equals lng.Id
                                    where pc.PaymentClaimNumber == claimNumber && lng.ISO6391 == lang
                                    select new TicketModel()
                                    {
                                        Content = tl.HtmlTicket,
                                        StatusCode = sc.Code,
                                        StatusMessage = sm.Message,
                                        TicketNumber = tl.TicketNumber.ToString()
                                    };
                    return ticketmodels.ToList();
                }
            }
            catch (Exception ex)
            {
                string innerExMessage = ex.InnerException?.Message ?? "N/A";
                LogRepository.InsertLogCommon(LogTypeModel.Error,
                    "TicketRepository.GetTicket_PaymentClaim",
                    $"Error fetching payment claim ticket. ClaimNumber: {claimNumber}, Lang: {lang}. Ex: {ex.Message}",
                    $"InnerEx: {innerExMessage}. StackTrace: {ex.StackTrace}");

                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }
    }
}