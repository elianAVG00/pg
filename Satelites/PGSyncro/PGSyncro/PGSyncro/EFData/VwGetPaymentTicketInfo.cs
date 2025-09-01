using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class VwGetPaymentTicketInfo
{
    public int Triid { get; set; }

    public DateTime TaicreatedOn { get; set; }

    public string ResponseCode { get; set; } = null!;

    public string ModuleDescription { get; set; } = null!;

    public int ServiceId { get; set; }

    public string? CustomerMail { get; set; }

    public decimal Amount { get; set; }

    public string? Authcode { get; set; }

    public string Cardmask { get; set; } = null!;

    public string? CurrencyIsoCode { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ServiceDescription { get; set; }

    public long TransactionId { get; set; }
}
