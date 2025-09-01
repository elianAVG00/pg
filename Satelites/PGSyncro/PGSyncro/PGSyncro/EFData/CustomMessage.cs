using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CustomMessage
{
    public int Id { get; set; }

    public string Es { get; set; } = null!;

    public string En { get; set; } = null!;

    public bool IsError { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ValidatorMessage> ValidatorMessages { get; set; } = new List<ValidatorMessage>();
}
