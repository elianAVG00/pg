using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class SpsServiceRequest
{
    public long Id { get; set; }

    public string? CommerceNumber { get; set; }

    public string? OperationNumber { get; set; }

    public string? Amount { get; set; }

    public string? Payments { get; set; }

    public string? CallbackUrl { get; set; }

    public int? ProductId { get; set; }

    public string? MailAddress { get; set; }

    public string? ParamSitio { get; set; }

    public string? ReturnUrl { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<SpsServiceResponse> SpsServiceResponses { get; set; } = new List<SpsServiceResponse>();
}
