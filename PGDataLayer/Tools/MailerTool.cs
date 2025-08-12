using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using PGDataLayer.Models;
using PGDataLayer.Repositories;
using Newtonsoft.Json;


namespace PGDataLayer.Tools
{
    public static class MailerTool
    {
        private static SmtpClient smtpClient { get; set; }
        private static NetworkCredential networkCredential { get; set; }

        public static bool SendEmail(MailerModel basicMail)
        {
            try
            {
                MailAddressCollection to = new MailAddressCollection();
                if (basicMail.To != null)
                {
                    string[] toList = basicMail.To.ToArray();
                    foreach (string toItem in toList)
                        to.Add(toItem);
                }

                MailAddressCollection cc = new MailAddressCollection();
                if (basicMail.CC != null)
                {
                    string[] ccList = basicMail.CC.ToArray();
                    foreach (string ccItem in ccList)
                        cc.Add(ccItem);
                }

                MailAddressCollection bcc = new MailAddressCollection();
                if (basicMail.BCC != null)
                {
                    string[] bccList = basicMail.BCC.ToArray();
                    foreach (string bccItem in bccList)
                        bcc.Add(bccItem);
                }

                MailAddress mailFrom = new MailAddress(basicMail.FromAddress, basicMail.FromDisplayName);
                return SendEmail(mailFrom, to, cc, bcc, basicMail.Subject, basicMail.Body, basicMail.Attachment, basicMail.HTML, MailPriority.High);

            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail1");
            }
            return false;
        }

        private static bool SendEmail(MailAddress from, MailAddressCollection toList, MailAddressCollection ccList, MailAddressCollection bcList, String subject, String body, List<FileAttachment> attachments, Boolean isBodyHtml, MailPriority mailPriority)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = from;

                    if (toList.Count > 0)
                        foreach (MailAddress adress in toList)
                            mailMessage.To.Add(adress);

                    if (ccList.Count > 0)
                        foreach (MailAddress mailAddress in ccList)
                            mailMessage.CC.Add(mailAddress);

                    if (bcList.Count > 0)
                        foreach (MailAddress mailAddress in bcList)
                            mailMessage.Bcc.Add(mailAddress);

                    if (attachments != null)
                    {
                        if (attachments.Count > 0)
                        {
                            foreach (FileAttachment fileattach in attachments)
                            {
                                mailMessage.Attachments.Add(new Attachment(new MemoryStream(fileattach.FileContent), fileattach.FileName + "." + fileattach.FileExtension));
                            }
                        }
                    }

                    mailMessage.SubjectEncoding = Encoding.UTF8;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = isBodyHtml;
                    mailMessage.Priority = mailPriority;
                    return SendEmail(mailMessage);
                }

            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail2");
            }

            return false;
        }

        private static bool SendEmail(MailMessage mailMessage)
        {
            mailMessage.Priority = MailPriority.High;
            try
            {
                if (smtpClient == null)
                {
                    smtpClient = new SmtpClient
                    {
                        Host = ConfigurationManager.AppSettings["Smtp_Server"],
                        Port = int.Parse(ConfigurationManager.AppSettings["Smtp_Port"]),
                        EnableSsl = bool.Parse(ConfigurationManager.AppSettings["Smtp_IsSSL"]),
                        Timeout = int.Parse(ConfigurationManager.AppSettings["Smtp_Timeout"]),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["Smtp_DefaultCredential"])
                    };

                    if (!smtpClient.UseDefaultCredentials)
                    {
                        smtpClient.Credentials = new NetworkCredential();
                    }
                }

                smtpClient.Send(mailMessage);
                LogRepository.InsertLog(new LogModel
                {
                   
                    message = "Mail enviado: " + JsonConvert.SerializeObject(mailMessage),
                    module = "Mail",
                    Type = LogType.Info
                });
                return true;

            }
            catch (SmtpFailedRecipientException ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail3");

            }
            catch (SmtpException ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail4");

            }
            catch (ArgumentException ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail5");

            }
            catch (ObjectDisposedException ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail6");

            }
            catch (InvalidOperationException ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail7");

            }
            catch (Exception ex)
            {

                LogRepository.InsertLogException(LogType.Exception, ex, false, "SendEmail8");
            }

            return false;
        }
    }

    public class MailerModel
    {
        public List<string> To { get; set; }

        public List<string> CC { get; set; }

        public List<string> BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool HTML { get; set; }

        public IDictionary<string, string> TemplateKeys { get; set; }

        public string FromAddress { get; set; }

        public string FromDisplayName { get; set; }

        public List<FileAttachment> Attachment { get; set; }

        public List<IncrustedResource> IncrustedResources { get; set; }
    }


    public class FileAttachment
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public byte[] FileContent { get; set; }
    }

    public class IncrustedResource
    {
        public string ContentID { get; set; }

        public FileAttachment Resource { get; set; }
    }

}