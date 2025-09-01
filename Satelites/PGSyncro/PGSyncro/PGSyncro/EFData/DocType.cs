using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class DocType
{
    public int DocTypeId { get; set; }

    public string? ShortName { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Claimer> Claimers { get; set; } = new List<Claimer>();
}
