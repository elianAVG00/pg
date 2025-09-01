using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class AnnulmentValidatorComm
{
    public int AnnulmentValidatorCommId { get; set; }

    public int? AnnulmentRequestId { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? RequestMessage { get; set; }

    public DateTime? ResponseDate { get; set; }

    public string? ResponseMessage { get; set; }

    public virtual AnnulmentRequest? AnnulmentRequest { get; set; }
}
