using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Rol
{
    public int Id { get; set; }

    public string ShortName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
