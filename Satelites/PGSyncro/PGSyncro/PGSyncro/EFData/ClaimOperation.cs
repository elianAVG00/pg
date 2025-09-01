using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ClaimOperation
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

    public virtual Claim Claim { get; set; } = null!;

    public virtual Product1 Product { get; set; } = null!;

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual Service1 Service { get; set; } = null!;
}
