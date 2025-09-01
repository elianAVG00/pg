using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ValidatorServiceConfig
{
    public int ValidatorServiceConfigId { get; set; }

    public int ValidatorId { get; set; }

    public int ServiceId { get; set; }

    public string? ValidatorUser { get; set; }

    public string? ValidatorPass { get; set; }

    public string? HashKey { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Validator Validator { get; set; } = null!;
}
