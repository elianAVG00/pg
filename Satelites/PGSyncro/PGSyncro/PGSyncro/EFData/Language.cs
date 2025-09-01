using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Language
{
    public int Id { get; set; }

    public string? Iso6391 { get; set; }

    public string? Iso6392 { get; set; }

    public string? Iso3166 { get; set; }

    public string NativeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<StatusMessage> StatusMessages { get; set; } = new List<StatusMessage>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();
}
