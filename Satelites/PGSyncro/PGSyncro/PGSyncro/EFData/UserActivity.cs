using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class UserActivity
{
    public long Id { get; set; }

    public int Ut { get; set; }

    public long? R { get; set; }

    public byte[] A { get; set; } = null!;
}
