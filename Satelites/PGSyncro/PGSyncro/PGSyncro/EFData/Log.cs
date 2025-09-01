using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Log
{
    public long LogId { get; set; }

    public DateTime Date { get; set; }

    public string? Type { get; set; }

    public string? Thread { get; set; }

    public string? Message { get; set; }

    public string? Exception { get; set; }
}
