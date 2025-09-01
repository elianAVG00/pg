using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CommerceItem
{
    public long CommerceItemsId { get; set; }

    public long TransactionIdPk { get; set; }

    public string? Code { get; set; }

    public string? OriginalCode { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public int State { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public DateTime? ReportDateConciliation { get; set; }

    public DateTime? ReportDateCentralizer { get; set; }

    public DateTime? ReportDateRendition { get; set; }

    public virtual ICollection<CommerceItemClaim> CommerceItemClaims { get; set; } = new List<CommerceItemClaim>();

    public virtual ICollection<ReportJobLog> ReportJobLogs { get; set; } = new List<ReportJobLog>();

    public virtual Transaction TransactionIdPkNavigation { get; set; } = null!;
}
