using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class MonitorSpsbatchProcessTransaction
{
    public long MonitorSpsbatchProcessTransactionsId { get; set; }

    public long MonitorSpsbatchProcessFilesId { get; set; }

    public long TransactionIdpk { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }

    public bool? HasInconsistenceError { get; set; }

    public string? InconsistenceError { get; set; }

    public bool? InconsistenceDays { get; set; }

    public bool? InconsistenceDups { get; set; }

    public bool? InconsistenceDate { get; set; }

    public bool? InconsistenceUniq { get; set; }

    public bool? InconsistenceCard { get; set; }

    public bool? InconsistenceCost { get; set; }
}
