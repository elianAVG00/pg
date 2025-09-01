using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class MonitorFilesReportRecord
{
    public long MonitorFilesReportRecordsId { get; set; }

    public long MonitorFilesReportProcessId { get; set; }

    public long TransactionIdPk { get; set; }

    public bool IsTotalizer { get; set; }

    public bool IsIncomplete { get; set; }

    public bool Informed { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }
}
