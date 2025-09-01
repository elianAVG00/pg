using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Claimer
{
    public int ClaimerId { get; set; }

    public int? DocTypeId { get; set; }

    public string? DocNumber { get; set; }

    public string? LastName { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Cellphone { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual DocType? DocType { get; set; }

    public virtual ICollection<PaymentClaim> PaymentClaims { get; set; } = new List<PaymentClaim>();
}
