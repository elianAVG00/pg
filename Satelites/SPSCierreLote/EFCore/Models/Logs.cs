using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Logs
{
    public long LogId { get; set; }

    public DateTime Date { get; set; }

    public string? Type { get; set; }

    public string? Thread { get; set; }

    public string? Message { get; set; }

    public string? Exception { get; set; }
}
