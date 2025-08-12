using PGDataLayer.EF;
using PGDataLayer.Models;
using PGDataLayer.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PGDataLayer.Repositories
{
    public static class NotificationRepository
    {
        internal static object GetHTTPResponseByStatusCodeOrPGCode(SendNotificationModel input)
        {
            throw new NotImplementedException();
        }

        private static string GetSenderAddress(int serviceId)
        {
            using (var context = new PaymentGatewayEntities())
            {
                var senderURL = context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == serviceId).SenderURL;
                if (string.IsNullOrWhiteSpace(senderURL))
                    senderURL = context.AppConfig.FirstOrDefault(app => app.Setting == "TicketSenderEmailAddress")
                        .Value;
                return senderURL;
            }
        }

        public static long SendQuickTicketOfPayment(long transactionIdPk)
        {
            var progressTracer = new StringBuilder();
            try
            {
                using (var _context = new PaymentGatewayEntities())
                {
                    progressTracer.Append("1/");
                    var transactionDetailsForTicket = (from tai in _context.TransactionAdditionalInfo
                                                       join cur in _context.Currency on tai.CurrencyId equals cur.CurrencyId
                                                       join p in _context.Products on tai.ProductId equals p.ProductId
                                                       join s in _context.Services on tai.ServiceId equals s.ServiceId
                                                       join ts in _context.TransactionStatus on tai.TransactionIdPK equals ts.TransactionsId
                                                       where tai.TransactionIdPK == transactionIdPk &&
                                                             cur.IsActive && p.IsActive && s.IsActive &&
                                                             ts.IsActual && ts.IsActive == true && ts.StatusCodeId == 5
                                                       select new DataForTicketModel // Proyección al modelo
                                                       {
                                                           TransactionIdPK = tai.TransactionIdPK,
                                                           StatusCodeId = ts.StatusCodeId,
                                                           ServiceId = s.ServiceId,
                                                           CreatedOn = tai.CreatedOn,
                                                           ServiceDescription = s.Description, 
                                                           TransactionNumber = tai.TransactionNumber,
                                                           ProductName = p.Description,           
                                                           CurrentAmount = tai.CurrentAmount,       
                                                           CurrencyISO = cur.IsoCode, 
                                                           CustomerMail = tai.CustomerMail   
                                                       }).FirstOrDefault();
                    progressTracer.Append("2/");
                    if (transactionDetailsForTicket == null)
                        return -2; // No se encontraron datos de transacción válidos o con estado 5

                    progressTracer.Append("3/");
                    var statusTemplate = _context.StatusTemplate.FirstOrDefault(st => st.StatusCodeId == transactionDetailsForTicket.StatusCodeId && st.IsActive);

                    progressTracer.Append("4/");
                    if (statusTemplate == null)
                        return -4; // No se encontró estado

                    progressTracer.Append("5/");
                    var notificationConfig = _context.NotificationConfig.FirstOrDefault(nc => nc.ServiceId == transactionDetailsForTicket.ServiceId && 
                                                                                              nc.IsActive && nc.StatusTemplateId == statusTemplate.StatusTemplateId);

                    progressTracer.Append("6/");
                    if (notificationConfig == null) 
                        return -5;

                    progressTracer.Append("7/");
                    var notificationTemplate = _context.NotificationTemplate.FirstOrDefault(nt => nt.IsActive && nt.NotificationTemplateId == statusTemplate.NotificationTemplateId);

                    progressTracer.Append("8/");
                    if (notificationTemplate == null)
                        return -6;

                    progressTracer.Append("9/");
                    var appConfigs = AppConfigRepository.GetAllConfigs();

                    progressTracer.Append("10/");
                    var newTransactionTicket = new TransactionTicket
                    {
                        TransactionIdPK = transactionDetailsForTicket.TransactionIdPK,
                        CreatedBy = "system", 
                        SentBySync = false, 
                        StatusTemplateId = statusTemplate.StatusTemplateId
                    };

                    progressTracer.Append("11/");
                    newTransactionTicket = InsertTransactionTicket(newTransactionTicket);

                    progressTracer.Append("12/");
                    var ticketDataDictionary = new Dictionary<string, string>();
                    ticketDataDictionary.Add("TicketDate", transactionDetailsForTicket.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));

                    progressTracer.Append("13/");
                    ticketDataDictionary.Add("TicketTime", transactionDetailsForTicket.CreatedOn.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                    progressTracer.Append("14/");
                    ticketDataDictionary.Add("TicketNumber", newTransactionTicket.TicketNumber.ToString());
                    ticketDataDictionary.Add("ServiceDescription", transactionDetailsForTicket.ServiceDescription);
                    ticketDataDictionary.Add("TransactionId", transactionDetailsForTicket.TransactionNumber.ToString());
                    ticketDataDictionary.Add("ProductName", transactionDetailsForTicket.ProductName);

                    string numberGroupSeparator = appConfigs.FirstOrDefault(c => c.Setting == "NumberGroupSeparator")?.Value ?? ",";
                    int numberDecimalDigits = Convert.ToInt32(appConfigs.FirstOrDefault(c => c.Setting == "NumberDecimalDigits")?.Value ?? "2");
                    string numberDecimalSeparator = appConfigs.FirstOrDefault(c => c.Setting == "NumberDecimalSeparator")?.Value ?? ".";

                    ticketDataDictionary.Add("Amount", FormatDecimal(transactionDetailsForTicket.CurrentAmount, numberGroupSeparator, numberDecimalDigits, numberDecimalSeparator));
                    ticketDataDictionary.Add("CurrencyIsoCode", transactionDetailsForTicket.CurrencyISO);

                    progressTracer.Append("15/");
                    string cardNumberToDisplay = " ";
                    string authorizationCodeToDisplay = " ";
                    var transactionResultInfo = _context.TransactionResultInfo.FirstOrDefault(tri => tri.TransactionIdPK == transactionDetailsForTicket.TransactionIdPK);

                    progressTracer.Append("16/");
                    if (transactionResultInfo != null)
                    {
                        cardNumberToDisplay = string.IsNullOrWhiteSpace(transactionResultInfo.CardNbrLfd) ? " " : transactionResultInfo.CardNbrLfd;
                        authorizationCodeToDisplay = string.IsNullOrWhiteSpace(transactionResultInfo.AuthorizationCode) ? " " : transactionResultInfo.AuthorizationCode;
                    }

                    progressTracer.Append("17/");
                    ticketDataDictionary.Add("CardNumber", cardNumberToDisplay);
                    ticketDataDictionary.Add("AuthorizationCode", authorizationCodeToDisplay);

                    progressTracer.Append("18/");
                    string filledTicketContent = FillTicket(notificationTemplate.TemplateTicket, ticketDataDictionary, transactionDetailsForTicket.CreatedOn);

                    progressTracer.Append("19/");
                    string emailDisplayName = appConfigs.FirstOrDefault(c => c.Setting == "TicketSenderEmailDisplayName")?.Value;
                    string emailAddress = appConfigs.FirstOrDefault(c => c.Setting == "TicketSenderEmailAddress")?.Value;

                    var recipientEmails = new List<string>();
                    if (!string.IsNullOrWhiteSpace(transactionDetailsForTicket.CustomerMail))
                    {
                        recipientEmails.Add(transactionDetailsForTicket.CustomerMail);
                    }

                    progressTracer.Append("20/");
                    // Solo envía si hay destinatarios y configuración de correo.
                    bool emailSentSuccessfully = false;
                    if (recipientEmails.Any() && !string.IsNullOrEmpty(emailAddress))
                    {
                        emailSentSuccessfully = SendTicketToClient(filledTicketContent, notificationTemplate.TemplateSubject, recipientEmails, emailDisplayName, emailAddress);
                    }

                    progressTracer.Append("21/");
                    // UpdateTransactionTicket toma el ID del TransactionTicket, no el TicketNumber.
                    long finalTicketNumber = UpdateTransactionTicket(emailSentSuccessfully, newTransactionTicket.TransactionTicketId);

                    progressTracer.Append("22/");
                    return finalTicketNumber; // Retorna el TicketNumber del ticket actualizado.
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error " + progressTracer?.ToString(), 
                    module = "PGDataLayer/NotificationRepository/SendQuickTicketOfPayment",
                    Type = LogType.Exception
                });
                return -1; 
            }
        }

        public static long SendNotification(SendNotificationModelComplex input)
        {
            var logclave = "";
            foreach (var entry in input.ticketModel)
                logclave = logclave + entry.Key + ": " + entry.Value + "|";


           LogRepository.InsertLog(new LogModel
           {
                exception = null,
                message = input.statusCode + " " + input.moduleDescription + " " + input.serviceId + " " + logclave + " " + input.finalUserMails,
                module = "PGDataLayer/NotificationRepository/SendNotification",
                Type = LogType.Debug
           });

           try
           {
            using (var context = new PaymentGatewayEntities())
            {
                var service = context.Services.Where(s => s.ServiceId == input.serviceId).FirstOrDefault();
                var statusCodeId = StatusRepository.GetStatusCodeByCode(input.statusCode).StatusCodeId;
                var statusTemplate = context.StatusTemplate.FirstOrDefault(st => st.StatusCodeId == statusCodeId);
                var module = context.Module.FirstOrDefault(m => m.Type == input.moduleDescription);

                //Quien hizo este if?!
                if (statusTemplate == null || module == null) {
                    LogRepository.InsertLog(new LogModel
                    {
                        exception = null,
                        message = "No existe template o modulo para este codigo",
                        module = "PGDataLayer/NotificationRepository/SendNotification",
                        Type = LogType.Error
                    });
                    return -1;
                }

                var notificationConfig =
                    context.NotificationConfig.FirstOrDefault(nt => nt.ServiceId == service.ServiceId &&
                                                                    nt.StatusTemplateId == statusTemplate
                                                                        .StatusTemplateId);
             
                var transactionTicket = new TransactionTicket();
                transactionTicket.TransactionIdPK = input.transactionIdPk;
                //transactionTicket.CreatedBy = "system";
                //transactionTicket.CreatedOn = DateTime.Now;
                //transactionTicket.SentBySync = input.sentBySync;
                transactionTicket.StatusTemplateId = statusTemplate.StatusTemplateId;
                transactionTicket.TicketNumber = InsertTransactionTicket(transactionTicket).TicketNumber;

                //Inserto el nro de ticket en el diccionario de datos del ticket
                input.ticketModel.Add("TicketNumber", transactionTicket.TicketNumber.ToString());
                var scid = StatusRepository.GetStatusCodeByCode(input.statusCode).StatusCodeId;
                var statustemplate = context.StatusTemplate.FirstOrDefault(st => st.StatusCodeId == scid);

                var notificationTemplate = context.NotificationTemplate.Where(c => c.NotificationTemplateId == statustemplate.NotificationTemplateId).FirstOrDefault();

                var ticketTemplate = notificationTemplate.TemplateTicket;
                ticketTemplate = FillTicket(ticketTemplate, input.ticketModel, input.paymentDate);

                var auxNotificationBuilder = string.Concat(notificationTemplate.TemplateBody, "</br>",
                    notificationConfig.AdditionalHeader, ticketTemplate, notificationConfig.AdditionalFooter);

                var notification = FillTicket(auxNotificationBuilder, input.ticketModel, input.paymentDate);

                /*
                //Guardo la notificacion
                var notificacionLog = new NotificationLogModel();
                notificacionLog.HtmlNotification = notification;
                notificacionLog.TicketLogId = transactionTicket.TransactionTicketId;
                notificacionLog.ModuleId = module.Id;
                notificacionLog.TypeFormat = "HTML";
                notificacionLog.CreatedBy = "system";
                InsertNotificationLog(notificacionLog);
                */

                var emailDisplayName = GetSenderMail(input.serviceId);
                var emailAddress = GetSenderAddress(input.serviceId);

                //Envío el ticket
                //   if (notificationConfig.SendThroughMail)
                //  {
                    var mailSent = SendTicketToClient(notification, notificationTemplate.TemplateSubject, input.finalUserMails.Split(';').ToList(), emailDisplayName, emailAddress);
                    transactionTicket.EmailSent = mailSent;
                    transactionTicket.EmailSentDate = DateTime.Now;
                    transactionTicket.SentBySync = input.sentBySync;
                //   }
                return transactionTicket.TicketNumber;
            }
           }
           catch (NullReferenceException nullex)
           {
                LogRepository.InsertLog(new LogModel
                {
                    exception = nullex,
                    message = "NullReferenceException",
                    module = "PGDataLayer/NotificationRepository/SendNotification",
                    Type = LogType.Exception
                });
                return -1;
           }
           catch (Exception ex)
           {
                LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error!",
                    module = "PGDataLayer/NotificationRepository/SendNotification",
                    Type = LogType.Exception
                });
                return -1;
           }
        }

        private static bool SendTicketToClient(string ticket, string subject, List<string> finalUserMails, string emailDisplayName, string emailAddress)
        {
            try
            {
                var inputParameters = string.Concat("ticket=", ticket, ",finalUserMails.Lenght=", finalUserMails.Count);

                MailerModel mailToSend = new MailerModel();
                mailToSend.Subject = subject;
                mailToSend.To = finalUserMails;
                mailToSend.FromAddress = emailAddress;
                mailToSend.FromDisplayName = emailDisplayName;
                mailToSend.Body = ticket;
                mailToSend.HTML = true;

                return MailerTool.SendEmail(mailToSend);
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error!",
                    module = "PGDataLayer/NotificationRepository/SendTicketToClient",
                    Type = LogType.Exception
                });
                return false;
            }
        }

        #region private
        private static TransactionTicket InsertTransactionTicket(TransactionTicket transactionTicket)
        {
            try
            {
                using (var context = new PaymentGatewayEntities())
                {
                    var transactionTicketTest = new TransactionTicket();
                    transactionTicketTest.IsActive = true;
                    transactionTicketTest.CreatedOn = DateTime.Now;
                    transactionTicketTest.TransactionIdPK = transactionTicket.TransactionIdPK;
                    transactionTicketTest.StatusTemplateId = transactionTicket.StatusTemplateId;
                    context.TransactionTicket.Add(transactionTicketTest);
                    context.SaveChanges();

                    return transactionTicketTest; //version anterior devolvia ticketNumber
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {

                        LogRepository.InsertLog(new LogModel
                        {
                            exception = null,
                            message = "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors: " + "- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage.ToString(),
                            module = "PGDataLayer/NotificationRepository/SendNotification",
                            Type = LogType.Error
                        });
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error al insertar TransactionTicket nbs1",
                    module = "PGDataLayer/NotificationRepository/SendNotification",
                    Type = LogType.Exception
                });
                return null;
            }
        }

        private static string FillTicket(string ticketTemplate, Dictionary<string, string> ticketData, DateTime paymentDate)
        {
            if (!ticketData.ContainsKey("TicketDate"))
            {
                ticketData.Add("TicketDate", paymentDate.ToShortDateString());
                ticketData.Add("TicketTime", paymentDate.ToShortTimeString());
            }

            foreach (var entry in ticketData)
            {
                var parameterToReplate = string.Concat("{", entry.Key, "}");
                ticketTemplate = ticketTemplate.Replace(parameterToReplate, entry.Value);
            }
            return ticketTemplate;
        }

        private static long UpdateTransactionTicket(bool mailSentSuccess, long transactionTicketId) 
        {
            try
            {
                using (var _context = new PaymentGatewayEntities())
                {
                    var ticketToUpdate = _context.TransactionTicket.FirstOrDefault(tt => tt.TransactionTicketId == transactionTicketId);

                    if (ticketToUpdate == null)
                        return -14; 

                    ticketToUpdate.EmailSent = mailSentSuccess;
                    if (mailSentSuccess)
                        ticketToUpdate.EmailSentDate = DateTime.Now;

                    ticketToUpdate.UpdatedBy = "system";
                    ticketToUpdate.UpdatedOn = DateTime.Now;

                    _context.SaveChanges();
                    return ticketToUpdate.TicketNumber;
                }
            }
            catch (DbEntityValidationException dbEx) 
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        LogRepository.InsertLog(new LogModel()
                        {
                            exception = null,
                            message = $"Error de validación de entidad al actualizar: Tipo {validationErrors.Entry.Entity.GetType().Name}, Estado {validationErrors.Entry.State}, Propiedad: {validationError.PropertyName}, Error: {validationError.ErrorMessage}",
                            module = "PGDataLayer/NotificationRepository/UpdateTransactionTicket", 
                            Type = LogType.Error
                        });
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error al actualizar TransactionTicket (nbs1 en Update)", 
                    module = "PGDataLayer/NotificationRepository/UpdateTransactionTicket", 
                    Type = LogType.Exception
                });
                return 0; 
            }
        }

        private static string GetSenderMail(int serviceId)
        {
            using (var context = new PaymentGatewayEntities())
            {
                var senderMail = context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == serviceId).SenderMail;
                if (string.IsNullOrWhiteSpace(senderMail))
                    senderMail = context.AppConfig.FirstOrDefault(app => app.Setting == "TicketSenderEmailDisplayName")
                        .Value;
                return senderMail;
            }
        }

        private static string FormatDecimal(Decimal input, string group_separator, int decimal_digits, string decimal_separator)
        {
            var provider = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            provider.NumberGroupSeparator = group_separator;
            provider.NumberDecimalDigits = decimal_digits;
            provider.NumberDecimalSeparator = decimal_separator;
            return input.ToString("#,0.00", provider);
        }
        #endregion
    }
}