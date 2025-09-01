using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ServiceChannel
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int ChannelId { get; set; }

    public virtual Channel1 Channel { get; set; } = null!;

    public virtual ICollection<Configuration1> Configuration1s { get; set; } = new List<Configuration1>();

    public virtual Service1 Service { get; set; } = null!;
}
