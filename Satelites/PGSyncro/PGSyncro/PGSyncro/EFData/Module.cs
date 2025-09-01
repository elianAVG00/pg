using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Module
{
    public int Id { get; set; }

    public int? ValidatorId { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<ModuleCode> ModuleCodes { get; set; } = new List<ModuleCode>();

    public virtual ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
}
