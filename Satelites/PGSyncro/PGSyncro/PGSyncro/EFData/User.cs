using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public int PswdId { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public virtual Pswd Pswd { get; set; } = null!;

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();
}
