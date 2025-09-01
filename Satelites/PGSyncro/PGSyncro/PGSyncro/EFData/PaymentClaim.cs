using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class PaymentClaim
{
    public long PaymentClaimId { get; set; }

    public long PaymentClaimNumber { get; set; }

    public int? ClaimerId { get; set; }

    public long? TransactionId { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? Amount { get; set; }

    public string? Observation { get; set; }

    public bool IsAutomatic { get; set; }

    public bool IsLocked { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<AnnulmentJobLog> AnnulmentJobLogs { get; set; } = new List<AnnulmentJobLog>();

    public virtual ICollection<AnnulmentRequest> AnnulmentRequests { get; set; } = new List<AnnulmentRequest>();

    public virtual ICollection<AnnulmentResultInfo> AnnulmentResultInfos { get; set; } = new List<AnnulmentResultInfo>();

    public virtual Claimer? Claimer { get; set; }

    public virtual ICollection<CommerceItemClaim> CommerceItemClaims { get; set; } = new List<CommerceItemClaim>();

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<PaymentClaimStatus> PaymentClaimStatuses { get; set; } = new List<PaymentClaimStatus>();

    public virtual Transaction? Transaction { get; set; }
}
