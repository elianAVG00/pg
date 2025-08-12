using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ProductSPSBatch
{
    public int ProductSPSBatchId { get; set; }

    public int ProductId { get; set; }

    public int ProductIdNORMAL { get; set; }

    public int ProductIdPRISMA { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}
