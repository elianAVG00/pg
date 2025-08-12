using Microsoft.Extensions.Configuration;
using PGExporter.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PGExporter.Repositories
{
    public class MailerRepository(ILogRepository _logrepository) : IMailerRepository
    {
        private SmtpClient? SMTPClient { get; set; }

        public async Task<bool> SendStatus(IConfiguration _config, string ToEmail, string MailBody)
        {
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("no-reply@provincianet.com.ar", "Payment Gateway Monitoreo"),
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                Subject = "[STATUS] Proceso de exportación",
                Body = MailBody,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };
            mailMessage.To.Add(ToEmail);
            mailMessage.Priority = MailPriority.High;
            try
            {
                if (this.SMTPClient == null)
                {
                    IConfigurationSection smtpConfig = _config.GetSection("SMTP");
                    this.SMTPClient = new SmtpClient()
                    {
                        Host = smtpConfig.GetValue<string>("Server"),
                        Port = smtpConfig.GetValue<int>("Port"),
                        EnableSsl = smtpConfig.GetValue<bool>("SSL"),
                        Timeout = smtpConfig.GetValue<int>("TimeOut"),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = smtpConfig.GetValue<bool>("DefaultCredential")
                    };
                    if (!this.SMTPClient.UseDefaultCredentials)
                        this.SMTPClient.Credentials = new NetworkCredential();
                }
                await this.SMTPClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (SmtpFailedRecipientException): " + ex.Message, "ERROR");
            }
            catch (SmtpException ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (SmtpException): " + ex.Message, "ERROR");
            }
            catch (ArgumentException ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (ArgumentException): " + ex.Message, "ERROR");
            }
            catch (ObjectDisposedException ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (ObjectDisposedException): " + ex.Message, "ERROR");
            }
            catch (InvalidOperationException ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (InvalidOperationException): " + ex.Message, "ERROR");
            }
            catch (Exception ex)
            {
                await _logrepository._print("EMAIL NO ENVIADO (Exception): " + ex.Message, "ERROR");
            }
            return false;
        }
    }

}
