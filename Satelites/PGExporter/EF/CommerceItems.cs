namespace PGExporter.EF
{
    public class CommerceItems
    {
        public long CommerceItemsId { get; set; }

        public long TransactionIdPk { get; set; }

        public string? Code { get; set; }

        public string? OriginalCode { get; set; }

        public string? Description { get; set; } = null;

        public Decimal Amount { get; set; }

        public int State { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; } = null;

        public DateTime CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? ReportDateConciliation { get; set; }

        public DateTime? ReportDateCentralizer { get; set; }

        public DateTime? ReportDateRendition { get; set; }

        public virtual Transactions? TransactionIdPkNavigation { get; set; } = null;
    }
}
