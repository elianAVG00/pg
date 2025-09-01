using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class NpsServiceResponse
{
    public long Id { get; set; }

    public long? SvcReqId { get; set; }

    public int? Code { get; set; }

    public string? Message { get; set; }

    public string? Extended { get; set; }

    public long? TrxId { get; set; }

    public long? OriginalTrxId { get; set; }

    public string? Session { get; set; }

    public string? FrontPspurl { get; set; }

    public string? MerchantId { get; set; }

    public string? MerchantMail { get; set; }

    public string? MerchTrxRef { get; set; }

    public string? MerchOrderId { get; set; }

    public long? Amount { get; set; }

    public int? NumPayments { get; set; }

    public long? PaymentAmount { get; set; }

    public int? Recurrent { get; set; }

    public long? RefundedAmount { get; set; }

    public long? CapturedAmount { get; set; }

    public long? AuthorizedAmount { get; set; }

    public string? AuthorizationCode { get; set; }

    public int? BatchNbr { get; set; }

    public int? TicketNbr { get; set; }

    public string? TicketDescription { get; set; }

    public string? ScreenDescription { get; set; }

    public int? SequenceNbr { get; set; }

    public string? Currency { get; set; }

    public string? Country { get; set; }

    public int? Product { get; set; }

    public DateOnly? Exp1Date { get; set; }

    public long? Exp1Amount { get; set; }

    public DateOnly? Exp2Date { get; set; }

    public long? Exp2Amount { get; set; }

    public DateOnly? Exp3Date { get; set; }

    public long? Exp3Amount { get; set; }

    public long? MinAmount { get; set; }

    public int? ExpMark { get; set; }

    public TimeOnly? ExpTime { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerMail { get; set; }

    public string? CustomerDocType { get; set; }

    public string? CustomerDocNbr { get; set; }

    public string? Plan { get; set; }

    public DateOnly? FirstPaymentDeferralDate { get; set; }

    public int? ClTrxId { get; set; }

    public long? ClExternalMerchant { get; set; }

    public long? ClExternalTerminal { get; set; }

    public int? ClResponseCode { get; set; }

    public string? ClResponseMessage { get; set; }

    public string? QueryCriteria { get; set; }

    public string? QueryCriteriaId { get; set; }

    public DateTime? PosDateTime { get; set; }

    public string? BarCode { get; set; }

    public virtual ICollection<NpsServiceResponseQueryTrx> NpsServiceResponseQueryTrxes { get; set; } = new List<NpsServiceResponseQueryTrx>();

    public virtual NpsServiceRequest? SvcReq { get; set; }
}
