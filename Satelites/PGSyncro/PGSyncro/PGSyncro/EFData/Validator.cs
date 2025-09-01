using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Validator
{
    public int ValidatorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool GenerateTransactionId { get; set; }

    public bool SendMail { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    public virtual ICollection<ProductsValidator> ProductsValidators { get; set; } = new List<ProductsValidator>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();

    public virtual ICollection<ValidatorServiceConfig> ValidatorServiceConfigs { get; set; } = new List<ValidatorServiceConfig>();
}
