using Microsoft.Extensions.Configuration;

namespace PGExporter.Interfaces
{
    public interface IMailerRepository
    {
        Task<bool> SendStatus(IConfiguration _config, string ToEmail, string MailBody);
    }
}
