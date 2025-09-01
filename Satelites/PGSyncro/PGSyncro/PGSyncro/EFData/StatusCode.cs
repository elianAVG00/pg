using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class StatusCode
{
    public int StatusCodeId { get; set; }

    public string Code { get; set; } = null!;

    public int GenericCodeId { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<CodeMapping> CodeMappings { get; set; } = new List<CodeMapping>();

    public virtual GenericCode GenericCode { get; set; } = null!;

    public virtual ICollection<PaymentClaimStatus> PaymentClaimStatuses { get; set; } = new List<PaymentClaimStatus>();

    public virtual ICollection<StatusMessage> StatusMessages { get; set; } = new List<StatusMessage>();

    public virtual ICollection<StatusTemplate> StatusTemplates { get; set; } = new List<StatusTemplate>();

    public virtual ICollection<TransactionStatus> TransactionStatuses { get; set; } = new List<TransactionStatus>();
}
