using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorPaywayBackOfficeTransactions
{
    public long MonitorSPSBatchProcessTransactionsId { get; set; }

    public long ProcessId { get; set; }

    public long TransactionIDPK { get; set; }

    public int PGNewStatusId { get; set; }

    public int PGOldStatusId { get; set; }

    public long PGAmount { get; set; }

    public bool IsInPayway { get; set; }

    public bool PaywayAnullment { get; set; }

    public string PaywayStatus { get; set; } = null!;

    public long? PaywayAmount { get; set; }

    public bool IsInconsistentAmount { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }
}
