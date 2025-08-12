using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class CustomMessages
{
    public int Id { get; set; }

    public string ES { get; set; } = null!;

    public string EN { get; set; } = null!;

    public bool IsError { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ValidatorMessages> ValidatorMessages { get; set; } = new List<ValidatorMessages>();
}
