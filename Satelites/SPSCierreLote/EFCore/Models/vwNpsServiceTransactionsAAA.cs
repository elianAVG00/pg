using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class vwNpsServiceTransactionsAAA
{
    public string TransactionId { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? Client { get; set; }

    public string? Service { get; set; }

    public string? Product { get; set; }

    public decimal Amount { get; set; }

    public string? ISO { get; set; }

    public string? Description { get; set; }

    public string? Validator { get; set; }

    public string? Country { get; set; }

    public string? TrxSource { get; set; }

    public string? Operation { get; set; }

    public string? Message { get; set; }

    public string? MerchantId { get; set; }

    public string? MerchTrxRef { get; set; }

    public int? NumPayments { get; set; }

    public long? PaymentAmount { get; set; }

    public DateOnly? FirstPaymentDeferralDate { get; set; }

    public string? AuthorizationCode { get; set; }

    public int? BatchNbr { get; set; }

    public int? TicketNbr { get; set; }

    public string? CardMask { get; set; }

    public string? CardHolderName { get; set; }

    public int? ClTrxId { get; set; }

    public long? ClExternalMerchant { get; set; }

    public long? ClExternalTerminal { get; set; }

    public string? PurchaseDescription { get; set; }
}
