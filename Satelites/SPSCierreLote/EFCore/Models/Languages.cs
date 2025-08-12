using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Languages
{
    public int Id { get; set; }

    public string ISO639 { get; set; } = null!;

    public string ISO3166 { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string NPS { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string HSR { get; set; } = null!;

    public string SPS { get; set; } = null!;
}
