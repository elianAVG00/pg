using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Validators
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

    public virtual ICollection<Configurations> Configurations { get; set; } = new List<Configurations>();

    public virtual ICollection<ProductsValidators> ProductsValidators { get; set; } = new List<ProductsValidators>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();
}
