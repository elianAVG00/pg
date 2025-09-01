using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class GenericCode
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<StatusCode> StatusCodes { get; set; } = new List<StatusCode>();
}
