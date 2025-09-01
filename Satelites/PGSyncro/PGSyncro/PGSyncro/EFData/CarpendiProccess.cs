using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class CarpendiProccess
{
    public long CarpendiProccessId { get; set; }

    public DateTime? BeginOn { get; set; }

    public DateTime? EndOn { get; set; }

    public string Filename { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsActive { get; set; }
}
