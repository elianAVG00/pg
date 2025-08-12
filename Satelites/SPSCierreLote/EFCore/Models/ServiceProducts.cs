using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ServiceProducts
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int ProductId { get; set; }

    public long? CommerceNbr { get; set; }

    public virtual Products1 Product { get; set; } = null!;

    public virtual Services1 Service { get; set; } = null!;
}
