using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ProductsValidator
{
    public int ProductValidatorId { get; set; }

    public int ProductId { get; set; }

    public int ValidatorId { get; set; }

    public int CardCode { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Validator Validator { get; set; } = null!;
}
