using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ClaimOperations
{
    public long Id { get; set; }

    public long ClaimId { get; set; }

    public int ServiceId { get; set; }

    public int ProductId { get; set; }

    public long OriginalTrxId { get; set; }

    public long AmountCharged { get; set; }

    public byte[]? CardInfo { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual Claims Claim { get; set; } = null!;

    public virtual Products1 Product { get; set; } = null!;

    public virtual ICollection<Refunds> Refunds { get; set; } = new List<Refunds>();

    public virtual Services1 Service { get; set; } = null!;
}
