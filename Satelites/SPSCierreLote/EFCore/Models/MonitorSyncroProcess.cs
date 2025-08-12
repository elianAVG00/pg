using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorSyncroProcess
{
    public long MonitorSyncroProcessId { get; set; }

    public DateTime BeginOn { get; set; }

    public DateTime? EndOn { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool IsHistorical { get; set; }

    public bool WithProcessError { get; set; }

    public long TotalTransactionsToSync { get; set; }

    public long TotalTransactionsProcessed { get; set; }

    public long TotalTransactionsClosed { get; set; }

    public long TotalTransactionsWithError { get; set; }

    public long TotalTransactionsNotClosed { get; set; }

    public long TotalTransactionsClosedOK { get; set; }

    public long TotalTransactionsClosedNO { get; set; }

    public long TotalTransactionsClosedERROR { get; set; }

    public long TotalTransactionsClosedNA { get; set; }

    public long TotalTransactionsClosedNAByTIMEOUT { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
