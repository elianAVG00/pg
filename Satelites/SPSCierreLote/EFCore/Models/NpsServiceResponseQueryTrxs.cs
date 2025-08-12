using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class NpsServiceResponseQueryTrxs
{
    public long Id { get; set; }

    public long SvcResId { get; set; }

    public int? Code { get; set; }

    public string? Message { get; set; }

    public string? Extended { get; set; }

    public int? TrxId { get; set; }

    public string? MerchantId { get; set; }

    public string? MerchantMail { get; set; }

    public string? Operation { get; set; }

    public string? TrxSource { get; set; }

    public string? MerchTrxRef { get; set; }

    public string? MerchOrderId { get; set; }

    public long? Amount { get; set; }

    public int? NumPayments { get; set; }

    public long? PaymentAmount { get; set; }

    public DateOnly? FirstPaymentDeferralDate { get; set; }

    public string? Currency { get; set; }

    public string? Country { get; set; }

    public int? Product { get; set; }

    public string? AuthorizationCode { get; set; }

    public int? BatchNbr { get; set; }

    public int? SequenceNbr { get; set; }

    public int? TicketNbr { get; set; }

    public string? CardNbrLfd { get; set; }

    public string? CardNbrFsd { get; set; }

    public string? CardMask { get; set; }

    public string? CardHolderName { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerMail { get; set; }

    public int? ClTrxId { get; set; }

    public long? ClExternalMerchant { get; set; }

    public long? ClExternalTerminal { get; set; }

    public int? ClResponseCode { get; set; }

    public string? ClResponseMessage { get; set; }

    public string? PurchaseDescription { get; set; }

    public string? Plan { get; set; }

    public DateTime? PosDateTime { get; set; }

    public virtual NpsServiceResponses SvcRes { get; set; } = null!;
}
