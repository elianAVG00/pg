using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class MonitorPaywayBackOfficeTransaction
{
    public long MonitorSpsbatchProcessTransactionsId { get; set; }

    public long ProcessId { get; set; }

    public long TransactionIdpk { get; set; }

    public int PgnewStatusId { get; set; }

    public int PgoldStatusId { get; set; }

    public long Pgamount { get; set; }

    public bool IsInPayway { get; set; }

    public bool PaywayAnullment { get; set; }

    public string PaywayStatus { get; set; } = null!;

    public long? PaywayAmount { get; set; }

    public bool IsInconsistentAmount { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }
}
