using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CommerceItemClaim
{
    public int CommerceItemClaimId { get; set; }

    public long CommerceItemId { get; set; }

    public long PaymentClaimId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual CommerceItem CommerceItem { get; set; } = null!;

    public virtual PaymentClaim PaymentClaim { get; set; } = null!;
}
