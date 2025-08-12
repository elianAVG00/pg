using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class GetPrismaCodes
{
    public int ProductId { get; set; }

    public string UniqueCode { get; set; } = null!;

    public int Normal { get; set; }

    public int Prisma { get; set; }
}
