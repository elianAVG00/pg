using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ProductCentralizer
{
    public int ProductCentralizerId { get; set; }

    public int ProductId { get; set; }

    public string CentralizerCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool? IsDebit { get; set; }
}
