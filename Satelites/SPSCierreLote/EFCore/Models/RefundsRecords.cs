using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class RefundsRecords
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ServiceId { get; set; }

    public long RefundId { get; set; }

    public int ValidatorId { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual Products1 Product { get; set; } = null!;

    public virtual Refunds Refund { get; set; } = null!;

    public virtual Services1 Service { get; set; } = null!;

    public virtual Validators1 Validator { get; set; } = null!;
}
