using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class PaymentClaimStatus
{
    public int PaymentClaimStatusId { get; set; }

    public long? PaymentClaimId { get; set; }

    public int? StatusCodeId { get; set; }

    public string? Observation { get; set; }

    public long? TicketNumber { get; set; }

    public bool IsActual { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual PaymentClaim? PaymentClaim { get; set; }

    public virtual StatusCode? StatusCode { get; set; }
}
