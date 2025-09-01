using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Bank
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Nps { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string Hsr { get; set; } = null!;

    public string Sps { get; set; } = null!;
}
