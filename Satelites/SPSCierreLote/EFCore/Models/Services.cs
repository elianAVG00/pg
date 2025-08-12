using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Services
{
    public int ServiceId { get; set; }

    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string MerchantId { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Clients Client { get; set; } = null!;

    public virtual ICollection<Configurations> Configurations { get; set; } = new List<Configurations>();

    public virtual ICollection<ServicesConfig> ServicesConfig { get; set; } = new List<ServicesConfig>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();
}
