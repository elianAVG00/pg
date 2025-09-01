using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class SalePoint
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int ClientId { get; set; }

    public virtual Client1 Client { get; set; } = null!;

    public virtual ICollection<Service1> Services { get; set; } = new List<Service1>();
}
