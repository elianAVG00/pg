using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class vwGetPaymentTicketInfo
{
    public int TRIId { get; set; }

    public DateTime TAICreatedOn { get; set; }

    public string responseCode { get; set; } = null!;

    public string moduleDescription { get; set; } = null!;

    public int serviceId { get; set; }

    public string? customerMail { get; set; }

    public decimal amount { get; set; }

    public string? authcode { get; set; }

    public string cardmask { get; set; } = null!;

    public string? currencyIsoCode { get; set; }

    public string productName { get; set; } = null!;

    public string? serviceDescription { get; set; }

    public long transactionId { get; set; }
}
