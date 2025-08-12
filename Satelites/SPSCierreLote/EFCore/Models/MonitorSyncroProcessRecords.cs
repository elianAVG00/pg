using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorSyncroProcessRecords
{
    public long MonitorSyncroProcessRecordsId { get; set; }

    public long MonitorSyncroProcessId { get; set; }

    public long TransactionIdPK { get; set; }

    public DateTime? BeginProcess { get; set; }

    public DateTime? EndProcess { get; set; }

    public DateTime? ClosedByProcess { get; set; }

    public bool SPSBatchForced { get; set; }

    public bool HasError { get; set; }

    public string? Error { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
