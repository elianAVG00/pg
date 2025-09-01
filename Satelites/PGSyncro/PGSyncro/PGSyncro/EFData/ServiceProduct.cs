using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ServiceProduct
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int ProductId { get; set; }

    public long? CommerceNbr { get; set; }

    public virtual Product1 Product { get; set; } = null!;

    public virtual Service1 Service { get; set; } = null!;
}
