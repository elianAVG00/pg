using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ReportJobLog
{
    public int ReportJobLogId { get; set; }

    public long? CommerceItemId { get; set; }

    public int? CentralizerJobRun { get; set; }

    public int? ConciliationJobRun { get; set; }

    public int? RenditionJobRun { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? CentralizerRefundJobRun { get; set; }

    public int? RenditionRefundJobRun { get; set; }

    public virtual CommerceItem? CommerceItem { get; set; }
}
