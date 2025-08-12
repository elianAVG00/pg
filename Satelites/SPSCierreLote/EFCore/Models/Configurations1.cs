using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Configurations1
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

    public virtual ICollection<ConfigurationDefaultValues> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValues>();

    public virtual ICollection<ConfigurationDetails> ConfigurationDetails { get; set; } = new List<ConfigurationDetails>();

    public virtual ServiceChannels ServiceChannel { get; set; } = null!;
}
