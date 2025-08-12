namespace PGExporter.EF
{
    public class ServicesConfig
    {
        public int ServiceConfigId { get; set; }

        public int ServiceId { get; set; }

        public int? ExternalId { get; set; }

        public int? BranchId { get; set; }

        public int? TerminalId { get; set; }

        public bool RptToRendition { get; set; }

        public bool RptToConciliation { get; set; }

        public bool RptToCentralizer { get; set; }

        public int RenditionType { get; set; }

        public bool IsCallbackPosted { get; set; }

        public string? SenderMail { get; set; }

        public string? SenderUrl { get; set; }

        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; } = null;

        public DateTime CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsCommerceItemValidated { get; set; }

        public bool IsPaymentSecured { get; set; }

        public string? ReportsPath { get; set; }

        public DateTime ReportBeginConciliation { get; set; }

        public DateTime ReportBeginCentralizer { get; set; }

        public DateTime ReportBeginRendition { get; set; }

        public bool IncludeBinInExport { get; set; }

        public bool ExcludeFromSpsbatchCloseInPaywayBo { get; set; }

        public virtual Services? Service { get; set; } = null;
    }
}
