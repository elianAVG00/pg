using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class StatusTemplate
{
    public int StatusTemplateId { get; set; }

    public int? StatusCodeId { get; set; }

    public int? NotificationTemplateId { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<NotificationConfig> NotificationConfigs { get; set; } = new List<NotificationConfig>();

    public virtual NotificationTemplate? NotificationTemplate { get; set; }

    public virtual StatusCode? StatusCode { get; set; }
}
