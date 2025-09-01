using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class AnnulmentRequest
{
    public int AnnulmentRequestId { get; set; }

    public long? PaymentClaimId { get; set; }

    public DateTime? Date { get; set; }

    public string? ResponseModuleCode { get; set; }

    public string? Result { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<AnnulmentJobLog> AnnulmentJobLogs { get; set; } = new List<AnnulmentJobLog>();

    public virtual ICollection<AnnulmentValidatorComm> AnnulmentValidatorComms { get; set; } = new List<AnnulmentValidatorComm>();

    public virtual PaymentClaim? PaymentClaim { get; set; }
}
