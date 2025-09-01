using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class UserService
{
    public int UserServiceId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public bool IsActive { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
