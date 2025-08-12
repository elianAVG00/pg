using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class PaymentValidatorComm
{
    public long PaymentValidatorCommId { get; set; }

    public long TransactionIdPK { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? RequestMessage { get; set; }

    public DateTime? ResponseDate { get; set; }

    public string? ResponseMessage { get; set; }

    public virtual Transactions TransactionIdPKNavigation { get; set; } = null!;
}
