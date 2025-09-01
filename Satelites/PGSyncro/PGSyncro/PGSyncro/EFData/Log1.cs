using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Log1
{
    public long LogId { get; set; }

    public DateTime Date { get; set; }

    public string Type { get; set; } = null!;

    public string Thread { get; set; } = null!;

    public string? Message { get; set; }

    public string? Exception { get; set; }

    public string? InnerException { get; set; }

    public string? Transaction { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool Deleted { get; set; }
}
