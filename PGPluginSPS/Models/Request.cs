using System;

namespace PGPluginSPS.Models
{
    public class Request
    {
        public string CommerceNumber { get; set; }

        public string OperationNumber { get; set; }

        public Decimal Amount { get; set; }

        public string Payments { get; set; }

        public string CallbackUrl { get; set; }

        public int ProductId { get; set; }

        public string MailAddress { get; set; }

        public string ParamSitio { get; set; }

        public string ReturnUrl { get; set; }

        public string EncryptedData { get; set; }
    }
}