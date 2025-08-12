using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorFilesReportProcess
{
    public long MonitorFilesReportProcessId { get; set; }

    public DateTime BeginOn { get; set; }

    public DateTime? EndDataOn { get; set; }

    public DateTime? EndProcessOn { get; set; }

    public string? RemoteFile { get; set; }

    public bool HasError { get; set; }

    public string? Error { get; set; }

    public int TotalRecords { get; set; }

    public int TotalInformed { get; set; }

    public long InformedAmount { get; set; }

    public long IncompleteAmount { get; set; }

    public int Type { get; set; }

    public bool WereRollback { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }
}
