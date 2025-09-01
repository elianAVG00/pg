using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class AnnulmentResultInfo
{
    public int AnnulmentResultInfoId { get; set; }

    public long TransactionId { get; set; }

    public long? PaymentClaimId { get; set; }

    public int ValidatorId { get; set; }

    public string? OperationNumber { get; set; }

    public string? AuthorizationCode { get; set; }

    public bool IsActive { get; set; }

    public DateTime OriginalDateTime { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual PaymentClaim? PaymentClaim { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual Validator1 Validator { get; set; } = null!;
}
