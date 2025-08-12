using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ProductsValidators
{
    public int ProductValidatorId { get; set; }

    public int ProductId { get; set; }

    public int ValidatorId { get; set; }

    public int CardCode { get; set; }

    public virtual Products Product { get; set; } = null!;

    public virtual Validators Validator { get; set; } = null!;
}
