using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using PGDataAccess.Models;
using PGDataAccess.Repository;


namespace PGDataAccess.Services
{
    static public class MailerService
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
                throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString());
            }
        }

        private static bool SendEmail(MailAddress from, MailAddressCollection toList, MailAddressCollection ccList, MailAddressCollection bcList, string subject, string body, List<FileAttachment> attachments, bool isBodyHtml, MailPriority mailPriority)
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

                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (FileAttachment fileattach in attachments)
                        {
                            mailMessage.Attachments.Add(new Attachment(new MemoryStream(fileattach.FileContent), fileattach.FileName + "." + fileattach.FileExtension));
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
                LogRepository.InsertLogException(LogTypeModel.Error, ex);
            }

            return false;
        }

        private static bool SendEmail(MailMessage mailMessage)
        {
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
                LogRepository.InsertLogCommon(LogTypeModel.Debug, "Email Enviado");
                return true;

            }
            catch (SmtpFailedRecipientException ex)
            {
         
                LogRepository.InsertLogException(LogTypeModel.Error, ex);

            }
            catch (SmtpException ex)
            {
        
                LogRepository.InsertLogException(LogTypeModel.Error, ex);

            }
            catch (ArgumentException ex)
            {

                LogRepository.InsertLogException(LogTypeModel.Error, ex);

            }
            catch (ObjectDisposedException ex)
            {

                LogRepository.InsertLogException(LogTypeModel.Error, ex);

            }
            catch (InvalidOperationException ex)
            {

                LogRepository.InsertLogException(LogTypeModel.Error, ex);

            }
            catch (Exception ex)
            {

                LogRepository.InsertLogException(LogTypeModel.Error, ex);
            }

            return false;
        }
    }
}