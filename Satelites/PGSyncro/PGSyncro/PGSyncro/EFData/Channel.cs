using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Channel
{
    public int ChannelId { get; set; }

    public int ChannelCode { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();
}
