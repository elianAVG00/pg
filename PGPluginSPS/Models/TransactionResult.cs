namespace PGPluginSPS.Models
{
    public class TransactionResult
    {
        public string ResponseMessage { get; set; }

        public string ResponseExtended { get; set; }

        public string ElectronicPaymentCode { get; set; }

        public string ResponseCode { get; set; }

        public string ReturnUrl { get; set; }

        public string MetaData { get; set; }

        public string TransactionId { get; set; }
    }
}