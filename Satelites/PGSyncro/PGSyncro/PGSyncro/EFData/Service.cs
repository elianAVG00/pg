using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Service
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

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    public virtual ICollection<NotificationConfig> NotificationConfigs { get; set; } = new List<NotificationConfig>();

    public virtual ICollection<ServicesConfig> ServicesConfigs { get; set; } = new List<ServicesConfig>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();

    public virtual ICollection<ValidatorServiceConfig> ValidatorServiceConfigs { get; set; } = new List<ValidatorServiceConfig>();
}
