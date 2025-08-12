using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorFilesReportRecords
{
    public long MonitorFilesReportRecordsId { get; set; }

    public long MonitorFilesReportProcessId { get; set; }

    public long TransactionIdPK { get; set; }

    public bool IsTotalizer { get; set; }

    public bool IsIncomplete { get; set; }

    public bool Informed { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }
}
