using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ModuleCode
{
    public int ModuleCodeId { get; set; }

    public int ModuleId { get; set; }

    public string OriginalCode { get; set; } = null!;

    public string? TechnicalInfo { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Module Module { get; set; } = null!;
}
