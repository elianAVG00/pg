using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class PaymentValidatorComm
{
    public long PaymentValidatorCommId { get; set; }

    public long TransactionIdPk { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? RequestMessage { get; set; }

    public DateTime? ResponseDate { get; set; }

    public string? ResponseMessage { get; set; }

    public virtual Transaction TransactionIdPkNavigation { get; set; } = null!;
}
