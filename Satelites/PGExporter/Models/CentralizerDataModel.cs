namespace PGExporter.Models
{
    public class CentralizerDataModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public long IdPK { get; set; }

        public long CommerceIdPK { get; set; }

        public int TerminalId { get; set; }

        public int BranchID { get; set; }

        public int ExternalId { get; set; }

        public Decimal Amount { get; set; }

        public string Code { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ServiceId { get; set; }
    }
}
