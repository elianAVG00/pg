namespace PGExporter.EF
{
    public class Products
    {
        public int ProductId { get; set; }

        public int ProductCode { get; set; }

        public string? Type { get; set; } = null;

        public string? Description { get; set; } = null;

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; } = null;

        public DateTime CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<Configurations> Configurations { get; set; } = new List<Configurations>();

        public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();
    }
}
