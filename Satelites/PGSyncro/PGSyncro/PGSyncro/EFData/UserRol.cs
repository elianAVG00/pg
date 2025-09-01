using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class UserRol
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RolId { get; set; }

    public bool IsActive { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
