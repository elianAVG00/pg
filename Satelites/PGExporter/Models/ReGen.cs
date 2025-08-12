using PGExporter.EF;

namespace PGExporter.Models
{
    public class ReGen
    {
        public MonitorFilesReportProcess MonitorFilesReportProcess { get; set; }

        public List<PGExporter.EF.MonitorFilesReportRecords> MonitorFilesReportRecords { get; set; }
    }
}
