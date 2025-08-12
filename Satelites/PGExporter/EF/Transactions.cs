namespace PGExporter.EF
{
    public class Transactions
    {
        public long Id { get; set; }

        public string? InternalNbr { get; set; } = null;

        public string? Channel { get; set; } = null;

        public string? Client { get; set; } = null;

        public string? SalePoint { get; set; } = null;

        public string? Service { get; set; } = null;

        public string? Product { get; set; } = null;

        public Decimal Amount { get; set; }

        public string? Validator { get; set; } = null;

        public string? WebSvcMethod { get; set; } = null;

        public string? TransactionId { get; set; } = null;

        public string? JsonObject { get; set; } = null;

        public DateTime CreatedOn { get; set; }

        public int SettingId { get; set; }

        public string? MerchantId { get; set; } = null;

        public string? ElectronicPaymentCode { get; set; } = null;

        public string? CurrencyCode { get; set; } = null;

        public string? TrxCurrencyCode { get; set; } = null;

        public Decimal TrxAmount { get; set; }

        public Decimal ConvertionRate { get; set; }

        public virtual ICollection<CommerceItems> CommerceItems { get; set; } = new List<CommerceItems>();

        public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();

        public virtual ICollection<TransactionResultInfo> TransactionResultInfo { get; set; } = new List<TransactionResultInfo>();
    }
}
