using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Clients
{
    public int ClientId { get; set; }

    public string? TributaryCode { get; set; }

    public string ShortName { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? SupportMail { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Services> Services { get; set; } = new List<Services>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; } = new List<TransactionAdditionalInfo>();
}
