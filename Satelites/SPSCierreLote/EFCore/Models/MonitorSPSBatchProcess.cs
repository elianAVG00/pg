using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorSPSBatchProcess
{
    public long MonitorSPSBatchProcessId { get; set; }

    public DateTime BeginOn { get; set; }

    public DateTime? EndOn { get; set; }

    public int FilesFound { get; set; }

    public int FilesRead { get; set; }

    public int TotalRecordsRead { get; set; }

    public int TotalRecordsNotRead { get; set; }

    public bool WithValidation { get; set; }

    public bool WithError { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }

    public bool? IsFTP { get; set; }
}
