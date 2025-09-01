using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class StatusSvcLog
{
    public long Id { get; set; }

    public string MerchantId { get; set; } = null!;

    public string ElectronicPaymentCode { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}
