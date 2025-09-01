using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ValidatorMessage
{
    public int Id { get; set; }

    public int Code { get; set; }

    public string Description { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int CustomMessageId { get; set; }

    public int ValidatorId { get; set; }

    public virtual CustomMessage CustomMessage { get; set; } = null!;

    public virtual Validator1 Validator { get; set; } = null!;
}
