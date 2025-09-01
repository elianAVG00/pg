using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class NotificationConfig
{
    public int NotificationConfigId { get; set; }

    public int? ServiceId { get; set; }

    public int? StatusTemplateId { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string AdditionalHeader { get; set; } = null!;

    public string AdditionalFooter { get; set; } = null!;

    public virtual Service? Service { get; set; }

    public virtual StatusTemplate? StatusTemplate { get; set; }
}
