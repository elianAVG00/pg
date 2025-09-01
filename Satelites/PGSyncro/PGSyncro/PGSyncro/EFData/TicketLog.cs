using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class TicketLog
{
    public int TicketLogId { get; set; }

    public long TicketNumber { get; set; }

    public string? TypeFormat { get; set; }

    public string? HtmlTicket { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
}
