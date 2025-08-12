using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class VersionLog
{
    public int VersionLogId { get; set; }

    public string Type { get; set; } = null!;

    public int Major { get; set; }

    public int Minor { get; set; }

    public int Maintenance { get; set; }

    public int Build { get; set; }

    public string Support { get; set; } = null!;

    public string Revision { get; set; } = null!;

    public int Deploy { get; set; }

    public string RequestBy { get; set; } = null!;

    public DateTime DeployDate { get; set; }
}
