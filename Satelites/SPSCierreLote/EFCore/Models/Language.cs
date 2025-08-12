using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Language
{
    public int Id { get; set; }

    public string? ISO6391 { get; set; }

    public string? ISO6392 { get; set; }

    public string? ISO3166 { get; set; }

    public string NativeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();
}
