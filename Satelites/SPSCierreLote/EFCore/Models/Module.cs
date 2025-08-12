using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Module
{
    public int Id { get; set; }

    public int? validatorId { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<ModuleCode> ModuleCode { get; set; } = new List<ModuleCode>();
}
