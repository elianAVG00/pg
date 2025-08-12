using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ServiceChannels
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int ChannelId { get; set; }

    public virtual Channels1 Channel { get; set; } = null!;

    public virtual ICollection<Configurations1> Configurations1 { get; set; } = new List<Configurations1>();

    public virtual Services1 Service { get; set; } = null!;
}
