using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Configuration
{
    public int ConfigurationId { get; set; }

    public int ServiceId { get; set; }

    public int ChannelId { get; set; }

    public int ProductId { get; set; }

    public int ValidatorId { get; set; }

    public string UniqueCode { get; set; } = null!;

    public string? CommerceNumber { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Validator Validator { get; set; } = null!;
}
