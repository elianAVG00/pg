using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Rol
{
    public int Id { get; set; }

    public string shortName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }
}
