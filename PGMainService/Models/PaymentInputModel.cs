namespace PGMainService.Models
{
    /// <summary>
    /// Entrada de pago.
    /// </summary>
    public class PaymentInputModel : PaymentInput
    {
        public string TransactionId { get; set; }

        public long TransactionIdPK { get; set; }

        public string EncSHA1key { get; set; }
    }
}