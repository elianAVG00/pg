using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PGSyncro.Utils
{
    public static class MailerService
    {
        private static SmtpClient smtpClient { get; set; }

        private static NetworkCredential networkCredential { get; set; }

        public static bool SendEmail(MailerModel basicMail)
        {
            try
            {
                MailAddressCollection toList = new MailAddressCollection();
                if (basicMail.To != null)
                {
                    foreach (string addresses in basicMail.To.ToArray())
                        toList.Add(addresses);
                }
                MailAddressCollection ccList = new MailAddressCollection();
                if (basicMail.CC != null)
                {
                    foreach (string addresses in basicMail.CC.ToArray())
                        ccList.Add(addresses);
                }
                MailAddressCollection bcList = new MailAddressCollection();
                if (basicMail.BCC != null)
                {
                    foreach (string addresses in basicMail.BCC.ToArray())
                        bcList.Add(addresses);
                }
                return MailerService.SendEmail(new MailAddress(basicMail.FromAddress, basicMail.FromDisplayName), toList, ccList, bcList, basicMail.Subject, basicMail.Body, basicMail.Attachment, basicMail.HTML, MailPriority.High);
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex, "Error F06");
                return false;
            }
        }

        private static bool SendEmail(
          MailAddress from,
          MailAddressCollection toList,
          MailAddressCollection ccList,
          MailAddressCollection bcList,
          string subject,
          string body,
          List<FileAttachment> attachments,
          bool isBodyHtml,
          MailPriority mailPriority)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = from;
                    if (toList.Count > 0)
                    {
                        foreach (MailAddress to in (Collection<MailAddress>)toList)
                            mailMessage.To.Add(to);
                    }
                    if (ccList.Count > 0)
                    {
                        foreach (MailAddress cc in (Collection<MailAddress>)ccList)
                            mailMessage.CC.Add(cc);
                    }
                    if (bcList.Count > 0)
                    {
                        foreach (MailAddress bc in (Collection<MailAddress>)bcList)
                            mailMessage.Bcc.Add(bc);
                    }
                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (FileAttachment attachment in attachments)
                            mailMessage.Attachments.Add(new Attachment((Stream)new MemoryStream(attachment.FileContent), $"{attachment.FileName}.{attachment.FileExtension}"));
                    }
                    mailMessage.SubjectEncoding = Encoding.UTF8;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = isBodyHtml;
                    mailMessage.Priority = mailPriority;
                    return MailerService.SendEmail(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex, "Error F07");
            }
            return false;
        }

        private static bool SendEmail(MailMessage mailMessage)
        {
            try
            {
                if (MailerService.smtpClient == null)
                {
                    MailerService.smtpClient = new SmtpClient()
                    {
                        Host = ConfigurationManager.AppSettings["Smtp_Server"],
                        Port = int.Parse(ConfigurationManager.AppSettings["Smtp_Port"]),
                        EnableSsl = bool.Parse(ConfigurationManager.AppSettings["Smtp_IsSSL"]),
                        Timeout = int.Parse(ConfigurationManager.AppSettings["Smtp_Timeout"]),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["Smtp_DefaultCredential"])
                    };
                    if (!MailerService.smtpClient.UseDefaultCredentials)
                        MailerService.smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential();
                }
                MailerService.smtpClient.Send(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                Program.Logger.Error((Exception)ex, "Error F08");
            }
            catch (SmtpException ex)
            {
                Program.Logger.Error((Exception)ex, "Error F09");
            }
            catch (ArgumentException ex)
            {
                Program.Logger.Error((Exception)ex, "Error F10");
            }
            catch (ObjectDisposedException ex)
            {
                Program.Logger.Error((Exception)ex, "Error F11");
            }
            catch (InvalidOperationException ex)
            {
                Program.Logger.Error((Exception)ex, "Error F12");
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex, "Error F13");
            }
            return false;
        }
    }
}
