using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string? IsoCode { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<PaymentClaim> PaymentClaims { get; set; } = new List<PaymentClaim>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();
}
