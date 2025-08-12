namespace PGExporter.Models
{
    public class Records
    {
        public long TransctionIDPK { get; set; }

        public long MonitorFilesReportRecordsId { get; set; }

        public bool Informed { get; set; }

        public bool Incomplete { get; set; }

        public Decimal Amount { get; set; }
    }
}
