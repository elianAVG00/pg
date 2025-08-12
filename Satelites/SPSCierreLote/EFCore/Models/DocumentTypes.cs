using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class DocumentTypes
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string NPS { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string HSR { get; set; } = null!;

    public string SPS { get; set; } = null!;

    public virtual ICollection<Claims> Claims { get; set; } = new List<Claims>();
}
