namespace PGMainService.Models
{
    public class PaymentClaimTicketModel
    {
        public string ClaimerName { get; set; }

        public string ClaimerLastName { get; set; }

        public string ServiceDescription { get; set; }

        public string PaymentClaimNumber { get; set; }

        public string TransactionId { get; set; }

        public string CurrencyIsoCode { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionAmount { get; set; }

        public string ProductName { get; set; }

        public string CardNumber { get; set; }

        public string Payments { get; set; }

        public string AnnulmentAmount { get; set; }
    }
}