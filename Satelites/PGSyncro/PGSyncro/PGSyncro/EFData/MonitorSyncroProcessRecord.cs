using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class MonitorSyncroProcessRecord
{
    public long MonitorSyncroProcessRecordsId { get; set; }

    public long MonitorSyncroProcessId { get; set; }

    public long TransactionIdPk { get; set; }

    public DateTime? BeginProcess { get; set; }

    public DateTime? EndProcess { get; set; }

    public DateTime? ClosedByProcess { get; set; }

    public bool SpsbatchForced { get; set; }

    public bool HasError { get; set; }

    public string? Error { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
