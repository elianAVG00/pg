using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CarpendiStat
{
    public long CarpendiStatsId { get; set; }

    public int Type { get; set; }

    public string Method { get; set; } = null!;

    public string? Text { get; set; }

    public string? DataRelated { get; set; }

    public long CarpendiProccessId { get; set; }

    public long? TransactionIdPkrelated { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? InnerExceptionMessage { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
