using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorSPSBatchProcessTransactions
{
    public long MonitorSPSBatchProcessTransactionsId { get; set; }

    public long MonitorSPSBatchProcessFilesId { get; set; }

    public long TransactionIDPK { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }

    public bool? HasInconsistenceError { get; set; }

    public string? InconsistenceError { get; set; }

    public bool? InconsistenceDAYS { get; set; }

    public bool? InconsistenceDUPS { get; set; }

    public bool? InconsistenceDATE { get; set; }

    public bool? InconsistenceUNIQ { get; set; }

    public bool? InconsistenceCARD { get; set; }

    public bool? InconsistenceCOST { get; set; }
}
