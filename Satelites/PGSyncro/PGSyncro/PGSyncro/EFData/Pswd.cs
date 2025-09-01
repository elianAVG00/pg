using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Pswd
{
    public int Id { get; set; }

    public string? Pswd1 { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
