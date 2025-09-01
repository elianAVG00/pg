using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class NotificationLog
{
    public int NotificationLogId { get; set; }

    public int ModuleId { get; set; }

    public int? TicketLogId { get; set; }

    public string? TypeFormat { get; set; }

    public string? HtmlNotification { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual TicketLog? TicketLog { get; set; }
}
