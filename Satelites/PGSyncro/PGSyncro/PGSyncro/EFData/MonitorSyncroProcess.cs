using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

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

    public long TotalTransactionsClosedOk { get; set; }

    public long TotalTransactionsClosedNo { get; set; }

    public long TotalTransactionsClosedError { get; set; }

    public long TotalTransactionsClosedNa { get; set; }

    public long TotalTransactionsClosedNabyTimeout { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
