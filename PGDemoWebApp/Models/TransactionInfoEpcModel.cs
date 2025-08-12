namespace PGDemoWebApp.Models
{
    public class TransactionInfoEpcModel
    {
        public string? ResponseGenericCode { get; set; }
        public string? ResponseGenericMessage { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public string? ResponseExtended { get; set; }
        public string? ElectronicPaymentCode { get; set; }
        public string? TransactionId { get; set; }
        // MetaData de productos
        public object? MetaData { get; set; }
        // URL To Post
        public string? ReturnUrl { get; set; }
    }
}
