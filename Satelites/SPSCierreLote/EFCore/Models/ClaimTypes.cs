using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ClaimTypes
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<Claims> Claims { get; set; } = new List<Claims>();
}
