using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Configuration1
{
    public int Id { get; set; }

    public int ServiceChannelId { get; set; }

    public DateTime FmDate { get; set; }

    public DateTime ToDate { get; set; }

    public string OperatorEmail { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<ConfigurationDefaultValue> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValue>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetails { get; set; } = new List<ConfigurationDetail>();

    public virtual ServiceChannel ServiceChannel { get; set; } = null!;
}
