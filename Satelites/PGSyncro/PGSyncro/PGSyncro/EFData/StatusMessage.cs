using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class StatusMessage
{
    public int Id { get; set; }

    public int IdLanguage { get; set; }

    public int StatusCodeId { get; set; }

    public string Message { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Language IdLanguageNavigation { get; set; } = null!;

    public virtual StatusCode StatusCode { get; set; } = null!;
}
