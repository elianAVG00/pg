using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace SPSCierreLote.Tools;

/// <summary>
/// Servicio de correo para el resumen del proceso “SPS Cierre de Lote”.
/// </summary>
internal static class Mailer
{
    // Instancia única de SmtpClient (lazy).
    private static SmtpClient? _smtpClient;

    // ==========================================================
    // 1) ENVÍO DE STATUS FINAL ─────────────────────────────────
    // ==========================================================
    internal static void SendStatus()
    {
        try
        {
            // Nada que enviar ➜ salir.
            if (string.IsNullOrWhiteSpace(Program.BatchStatusMailAddress) ||
                string.IsNullOrWhiteSpace(Program.BatchStatusMailBody))
            {
                Program.Print("STATUS MAIL DESACTIVADO / SIN DESTINATARIO", "NO");
                return;
            }

            // Crea SmtpClient la primera vez.
            _smtpClient ??= BuildSmtpClient();

            using var message = new MailMessage
            {
                From = new MailAddress("no-reply@provincianet.com.ar", "Payment Gateway Monitoreo"),
                Subject = $"[STATUS] Cierre de Lote PRISMA – {Program.MonitorProcessId}",
                Body = Program.BatchStatusMailBody,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Priority = MailPriority.High
            };

            message.To.Add("evgonzalez@provincianet.com.ar");

            _smtpClient.Send(message);
            Program.Print("EMAIL ENVIADO", "OK");
        }
        catch (SmtpFailedRecipientException ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (SmtpFailedRecipientException): {ex.Message}");
        }
        catch (SmtpException ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (SmtpException): {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (ArgumentException): {ex.Message}");
        }
        catch (ObjectDisposedException ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (ObjectDisposedException): {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (InvalidOperationException): {ex.Message}");
        }
        catch (Exception ex)
        {
            LogAndPrint($"EMAIL NO ENVIADO (Exception): {ex.Message}");
        }
    }

    // ==========================================================
    // 2) FACTORY DE SMTPCLIENT ─────────────────────────────────
    // ==========================================================
    private static SmtpClient BuildSmtpClient()
    {
        IConfiguration cfg = Program.Configuration;   // expuesto en Program/Main

        var client = new SmtpClient
        {
            Host = cfg["Smtp_Server"] ?? "localhost",
            Port = int.Parse(cfg["Smtp_Port"] ?? "25"),
            EnableSsl = bool.Parse(cfg["Smtp_IsSSL"] ?? "false"),
            Timeout = int.Parse(cfg["Smtp_Timeout"] ?? "10000"),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = bool.Parse(cfg["Smtp_UseDefaultCredential"] ?? "true")
        };

        // Credenciales explícitas solo si no se usan las del sistema.
        if (!client.UseDefaultCredentials)
        {
            client.Credentials = new NetworkCredential(
                cfg["Smtp_User"] ?? string.Empty,
                cfg["Smtp_Pass"] ?? string.Empty);
        }

        return client;
    }

    // ==========================================================
    // 3) HELPER DE LOG ─────────────────────────────────────────
    // ==========================================================
    private static void LogAndPrint(string msg)
    {
        Program.Print(msg, "ERROR");
        Program.Logger.LogError(msg);
    }
}
