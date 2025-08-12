using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class StatusSvcLogs
{
    public long Id { get; set; }

    public string MerchantId { get; set; } = null!;

    public string ElectronicPaymentCode { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}
