using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class SalePoints
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int ClientId { get; set; }

    public virtual Clients1 Client { get; set; } = null!;

    public virtual ICollection<Services1> Service { get; set; } = new List<Services1>();
}
