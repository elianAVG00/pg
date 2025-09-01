using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProductCode { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsDebit { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    public virtual ICollection<ProductsValidator> ProductsValidators { get; set; } = new List<ProductsValidator>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();
}
