using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Client
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

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();
}
