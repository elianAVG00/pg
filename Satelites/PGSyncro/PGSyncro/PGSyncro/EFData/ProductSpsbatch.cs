using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ProductSpsbatch
{
    public int ProductSpsbatchId { get; set; }

    public int ProductId { get; set; }

    public int ProductIdNormal { get; set; }

    public int ProductIdPrisma { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}
