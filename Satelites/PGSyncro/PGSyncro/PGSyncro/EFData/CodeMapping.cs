using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CodeMapping
{
    public int CodeMappingId { get; set; }

    public int ModuleCodeId { get; set; }

    public int StatusCodeId { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ModuleCode ModuleCode { get; set; } = null!;

    public virtual StatusCode StatusCode { get; set; } = null!;
}
