using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class JobRunLog
{
    public int JobRunLogId { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime? DateFinish { get; set; }

    public string State { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Parameters { get; set; }

    public string? SendNotificationTo { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AnnulmentJobLog> AnnulmentJobLogs { get; set; } = new List<AnnulmentJobLog>();
}
