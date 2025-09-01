using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class NotificationTemplate
{
    public int NotificationTemplateId { get; set; }

    public string? Name { get; set; }

    public string? TemplateSubject { get; set; }

    public string? TemplateBody { get; set; }

    public string? TemplateTicket { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<StatusTemplate> StatusTemplates { get; set; } = new List<StatusTemplate>();
}
