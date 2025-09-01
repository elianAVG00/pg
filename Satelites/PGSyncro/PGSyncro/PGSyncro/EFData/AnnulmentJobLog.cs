using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class AnnulmentJobLog
{
    public int AnnulmentJobLogId { get; set; }

    public int JobRunLogId { get; set; }

    public long PaymentClaimId { get; set; }

    public int? AnnulmentRequestId { get; set; }

    public virtual AnnulmentRequest? AnnulmentRequest { get; set; }

    public virtual JobRunLog JobRunLog { get; set; } = null!;

    public virtual PaymentClaim PaymentClaim { get; set; } = null!;
}
