using System;
using System.Collections.Generic;
using System.Linq;
using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Models;
using PGDataAccess.Services;
using System.Data.Entity.Validation;

namespace PGDataAccess.Repository
{
    public class NotificationRepository
    {
        public static long? SendNotification(long transactionIdPk, string statusCode, string moduleDescription,
            int serviceId, Dictionary<string, string> ticketModel, string finalUserMails, DateTime paymentDate, bool sentBySync = false )
        {
            var logclave = "";
            foreach (var entry in ticketModel)
                logclave = logclave + entry.Key + ": " + entry.Value + "|";

            LogRepository.InsertLogCommon(LogTypeModel.Debug,
                statusCode + " " + moduleDescription + " " + serviceId + " " + logclave + " " + finalUserMails);

            try
            {
                using (var context = new PGDataEntities())
                {
                    var service = CommonRepository.GetServiceById(serviceId);
                    var statusCodeId = StatusRepository.GetStatusCodeByCode(statusCode).StatusCodeId;
                    var statusTemplate = context.StatusTemplate.FirstOrDefault(st => st.StatusCodeId == statusCodeId);
                    var module = context.Module.FirstOrDefault(m => m.Type == moduleDescription);

                    if (statusTemplate == null || module == null)
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Warning, "DataAccesService.SendNotification", "StatusTemplate or Module not found.",
                                                $"StatusCode: {statusCode}, ModuleDescription: {moduleDescription}");
                        return null;
                    }

                    var notificationConfig = context.NotificationConfig.FirstOrDefault(nt => 
                                                nt.ServiceId == service.ServiceId && nt.StatusTemplateId == statusTemplate.StatusTemplateId);

                    if (notificationConfig == null)
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Warning, "DataAccesService.SendNotification",
                            "NotificationConfig not found.", $"ServiceId: {service.ServiceId}, StatusTemplateId: {statusTemplate.StatusTemplateId}");
                    }
                    
                    //versionSDX utiliza el TransactionTicket
                    var transactionTicket = new TransactionTicket();
                    transactionTicket.TransactionIdPK = transactionIdPk;
                    transactionTicket.CreatedBy = "system";
                    transactionTicket.CreatedOn = DateTime.Now;
                    transactionTicket.SentBySync = sentBySync;
                    transactionTicket.StatusTemplateId = statusTemplate.StatusTemplateId; 
                    InsertTransactionTicket(transactionTicket);

                    //versionPROD utiliza el TicketLogModel
                    var newTicketLog = new TicketLogModel()
                    {
                        CreatedBy = "system",
                        CreatedOn = DateTime.Now
                    };
                    newTicketLog.TicketNumber = InsertTicketLog(newTicketLog);

                    //Inserto el nro de ticket en el diccionario de datos del ticket
                    ticketModel.Add("TicketNumber", transactionTicket.TicketNumber.ToString());

                    var notificationTemplate = GetNotificationTemplate(statusCodeId);

                    if (notificationTemplate == null)
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Error, "NotificationTemplateModel not found for StatusCodeId.", null,
                            "DataAccesService.SendNotification", statusCodeId.ToString());
                        return null;
                    }

                    var ticketTemplate = notificationTemplate.TemplateTicket;
                    ticketTemplate = FillTicket(ticketTemplate, ticketModel, paymentDate);

                    var auxNotificationBuilder = string.Concat(notificationTemplate.TemplateBody, "</br>",
                        notificationConfig.AdditionalHeader, ticketTemplate, notificationConfig.AdditionalFooter);

                    var notification = FillTicket(auxNotificationBuilder, ticketModel, paymentDate);
                    newTicketLog.TicketLogId = UpdateTicketLog(newTicketLog);

                    //Guardo la notificacion
                    var notificacionLog = new NotificationLogModel();
                    notificacionLog.HtmlNotification = notification;
                    notificacionLog.TicketLogId = (int?)transactionTicket.TransactionTicketId;
                    notificacionLog.ModuleId = module.Id;
                    notificacionLog.TypeFormat = "HTML";
                    notificacionLog.CreatedBy = "system";
                    InsertNotificationLog(notificacionLog);

                    var emailDisplayName = GetSenderMail(serviceId);
                    var emailAddress = GetSenderAddress(serviceId);

                    //Envío el ticket
                    //IMPORTANTE: El campo SendThroughMail no esta en SANDBOX
                    //if (notificationConfig.SendThroughMail)
                    var mailSent = SendTicketToClient(notification, notificationTemplate.TemplateSubject,
                        finalUserMails.Split(';'), emailDisplayName, emailAddress);


                    transactionTicket.EmailSent = mailSent;
                    transactionTicket.EmailSentDate = DateTime.Now;
                    transactionTicket.SentBySync = sentBySync;
                    

                    return transactionTicket.TicketNumber;
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                    LogRepository.InsertLogException(LogTypeModel.Error, ex));
            }
        }

        public static int InsertNotificationLog(NotificationLogModel notificationLog)
        {
            using (var context = new PGDataEntities())
            {
                var notificationEntity = Mapper.NotificationLog_ModelToEF(notificationLog);
                context.NotificationLog.Add(notificationEntity);
                context.SaveChanges();
                return notificationEntity.NotificationLogId;
            }
        }

        public static int? InsertNotificationConfig(string serviceName, string statusCode, string templateName)
        {
            using (var context = new PGDataEntities())
            {
                var statusTemplateId = InsertStatusTemplate(statusCode, templateName);
                var serviceEntity = context.Services.FirstOrDefault(s => s.Name == serviceName);
                if (statusTemplateId != null && serviceEntity != null)
                {
                    var notificationConfig = new NotificationConfig();
                    notificationConfig.StatusTemplateId = statusTemplateId;
                    notificationConfig.ServiceId = serviceEntity.ServiceId;
                    notificationConfig.CreatedBy = "system";
                    context.NotificationConfig.Add(notificationConfig);
                    context.SaveChanges();
                    return notificationConfig.NotificationConfigId;
                }
                return null;
            }
        }

        public static int? InsertStatusTemplate(string statusCode, string templateName)
        {
            using (var context = new PGDataEntities())
            {
                var notificationTemplateEntity =
                    context.NotificationTemplate.FirstOrDefault(nt => nt.Name == templateName);
                var statusCodeEntity = context.StatusCode.FirstOrDefault(sc => sc.Code == statusCode);
                if (notificationTemplateEntity != null && statusCodeEntity != null)
                {
                    var statusTemplate = new StatusTemplate();
                    statusTemplate.StatusCodeId = statusCodeEntity.StatusCodeId;
                    statusTemplate.NotificationTemplateId = notificationTemplateEntity.NotificationTemplateId;
                    statusTemplate.CreatedBy = "system";
                    context.StatusTemplate.Add(statusTemplate);
                    context.SaveChanges();
                    return statusTemplate.StatusTemplateId;
                }
                return null;
            }
        }

        #region Private Methods

        private static long InsertTransactionTicket(TransactionTicket transactionTicket)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    transactionTicket.CreatedOn = DateTime.Now;
                    context.TransactionTicket.Add(transactionTicket);
                    context.SaveChanges();
                    return transactionTicket.TicketNumber;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogCommon(LogTypeModel.Error, "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:", "- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage).ToString());
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        public static long InsertTicketLog(TicketLogModel ticketLogModel)
        {
            try
            {
                using (var dataContext = new PGDataEntities())
                {
                    ticketLogModel.CreatedOn = DateTime.Now;
                    TicketLog ticketLogEntity = Mapper.TicketLog_ModelToEF(ticketLogModel);
                    dataContext.TicketLog.Add(ticketLogEntity);
                    dataContext.SaveChanges();
                    return ticketLogEntity.TicketNumber;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Error inserting ticket log.", ex);
            }
        }

        public static int UpdateTicketLog(TicketLogModel ticketLog)
        {
            try
            {
                using (var dataContext = new PGDataEntities())
                {
                    TicketLog ticketLogEntity = dataContext.TicketLog
                        .FirstOrDefault(t => t.TicketNumber == ticketLog.TicketNumber);
                    ticketLogEntity.TypeFormat = ticketLog.TypeFormat;
                    ticketLogEntity.HtmlTicket = ticketLog.HtmlTicket;
                    dataContext.SaveChanges();
                    return ticketLogEntity.TicketLogId;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
                throw new Exception("Error updating ticket log.", ex);
            }
        }

        public static void UpdateTransactionTicket(TransactionTicket transactionTicket)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var savedTransactionTicket =
                        context.TransactionTicket.FirstOrDefault(tt => tt.TicketNumber ==
                                                                       transactionTicket.TicketNumber);

                    savedTransactionTicket.UpdatedOn = DateTime.Now;
                    savedTransactionTicket.UpdatedBy = "system";
                    savedTransactionTicket.EmailSentDate = transactionTicket.EmailSentDate;
                    savedTransactionTicket.EmailSent = transactionTicket.EmailSent;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                    LogRepository.InsertLogException(LogTypeModel.Error, ex));
            }
        }


        private static bool SendTicketToClient(string ticket, string subject, string[] finalUserMails,
            string emailDisplayName, string emailAddress)
        {
            try
            {
                var inputParameters = string.Concat("ticket=", ticket, ",finalUserMails.Lenght=",
                    finalUserMails.Length.ToString());
                LogRepository.InsertLogCommon(LogTypeModel.Info,
                    string.Concat("Parametros: ", inputParameters));


                //FGS - Creo un modelo MailerModel
                var mailToSend = new MailerModel();

                mailToSend.Subject = subject;
                mailToSend.To = finalUserMails.ToList();
                mailToSend.FromAddress = emailAddress;
                mailToSend.FromDisplayName = emailDisplayName;
                mailToSend.Body = ticket;
                mailToSend.HTML = true;

                return MailerService.SendEmail(mailToSend);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                    LogRepository.InsertLogException(LogTypeModel.Error, ex));
            }
        }

        private static Dictionary<string, string> ToDictionary(object myObj)
        {
            return
                (from x in myObj.GetType().GetProperties() select x).ToDictionary(x => x.Name,
                    x =>
                        x.GetGetMethod().Invoke(myObj, null) == null
                            ? ""
                            : x.GetGetMethod().Invoke(myObj, null).ToString());
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

        private static NotificationTemplateModel GetNotificationTemplate(int statusCodeId)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var inputParameters = string.Concat("statusCodeId=", statusCodeId);
                    LogRepository.InsertLogCommon(LogTypeModel.Info, string.Concat("Parametros: ", inputParameters));
                    var statusTemplateEntity =
                        context.StatusTemplate.FirstOrDefault(st => st.StatusCodeId == statusCodeId);
                    return
                        Mapper.NotificationTemplateModel_EFToModel(
                            context.NotificationTemplate.FirstOrDefault(
                                nt => nt.NotificationTemplateId == statusTemplateEntity.NotificationTemplateId));
                }
            }
            catch (NullReferenceException nullex)
            {
                LogRepository.InsertLogException(LogTypeModel.Info, nullex);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                    LogRepository.InsertLogException(LogTypeModel.Error, ex));
            }
        }

        private static string GetSenderMail(int serviceId)
        {
            using (var context = new PGDataEntities())
            {
                var senderMail = context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == serviceId).SenderMail;
                if (string.IsNullOrWhiteSpace(senderMail))
                    senderMail = context.AppConfig.FirstOrDefault(app => app.Setting == "TicketSenderEmailDisplayName")
                        .Value;
                return senderMail;
            }
        }

        private static string GetSenderAddress(int serviceId)
        {
            using (var context = new PGDataEntities())
            {
                var senderURL = context.ServicesConfig.FirstOrDefault(sc => sc.ServiceId == serviceId).SenderURL;
                if (string.IsNullOrWhiteSpace(senderURL))
                    senderURL = context.AppConfig.FirstOrDefault(app => app.Setting == "TicketSenderEmailAddress")
                        .Value;
                return senderURL;
            }
        }

        #endregion Private Methods
    }
}