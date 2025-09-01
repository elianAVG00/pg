using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class GetPrismaCode
{
    public int ProductId { get; set; }

    public string UniqueCode { get; set; } = null!;

    public int Normal { get; set; }

    public int Prisma { get; set; }
}
