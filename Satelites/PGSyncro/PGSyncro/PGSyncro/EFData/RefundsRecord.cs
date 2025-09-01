using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class RefundsRecord
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ServiceId { get; set; }

    public long RefundId { get; set; }

    public int ValidatorId { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual Product1 Product { get; set; } = null!;

    public virtual Refund Refund { get; set; } = null!;

    public virtual Service1 Service { get; set; } = null!;

    public virtual Validator1 Validator { get; set; } = null!;
}
