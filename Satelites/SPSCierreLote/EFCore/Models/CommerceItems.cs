using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class CommerceItems
{
    public long CommerceItemsId { get; set; }

    public long TransactionIdPK { get; set; }

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

    public virtual ICollection<ReportJobLog> ReportJobLog { get; set; } = new List<ReportJobLog>();

    public virtual Transactions TransactionIdPKNavigation { get; set; } = null!;
}
