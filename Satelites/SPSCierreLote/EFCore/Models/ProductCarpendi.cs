using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ProductCarpendi
{
    public int ProductCarpendiId { get; set; }

    public int ProductId { get; set; }

    public string CarpendiProductId { get; set; } = null!;

    public bool deleted { get; set; }
}
