using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ProductCarpendi
{
    public int ProductCarpendiId { get; set; }

    public int ProductId { get; set; }

    public string CarpendiProductId { get; set; } = null!;

    public bool Deleted { get; set; }
}
