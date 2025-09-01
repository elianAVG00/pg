using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ExternalApp
{
    public int ExternalAppId { get; set; }

    public int Code { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
