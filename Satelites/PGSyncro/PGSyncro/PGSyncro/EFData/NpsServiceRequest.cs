using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class NpsServiceRequest
{
    public long Id { get; set; }

    public string? Type { get; set; }

    public string? Version { get; set; }

    public string? TrxSource { get; set; }

    public string? MerchantId { get; set; }

    public string? MerchantMail { get; set; }

    public string? MerchTrxRef { get; set; }

    public string? MerchOrderId { get; set; }

    public long? OriginalTrxId { get; set; }

    public string? ReturnUrl { get; set; }

    public string? FrmBackButtonUrl { get; set; }

    public string? FrmLanguage { get; set; }

    public string? TicketDescription { get; set; }

    public string? ScreenDescription { get; set; }

    public long? Amount { get; set; }

    public int? NumPayments { get; set; }

    public long? PaymentAmount { get; set; }

    public int? Recurrent { get; set; }

    public string? Plan { get; set; }

    public string? Currency { get; set; }

    public string? Country { get; set; }

    public int? Product { get; set; }

    public DateOnly? Exp1Date { get; set; }

    public long? Exp1Amount { get; set; }

    public int? DaysUntilExp2Date { get; set; }

    public DateOnly? Exp2Date { get; set; }

    public long? Exp2Amount { get; set; }

    public DateOnly? Exp3Date { get; set; }

    public long? Exp3Amount { get; set; }

    public long? MinAmount { get; set; }

    public int? ExpMark { get; set; }

    public TimeOnly? ExpTime { get; set; }

    public int? SurchargeAmount { get; set; }

    public int? DaysAvailableToPay { get; set; }

    public string? CardNumber { get; set; }

    public string? CardExpDate { get; set; }

    public int? CardSecurityCode { get; set; }

    public string? CardHolderName { get; set; }

    public string? UserId { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerMail { get; set; }

    public string? CustomerDocType { get; set; }

    public string? CustomerDocNbr { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerAddress { get; set; }

    public string? CustomerCountry { get; set; }

    public string? CustomerProvince { get; set; }

    public string? CustomerLocality { get; set; }

    public string? PurchaseDescription { get; set; }

    public string? SoftDescriptor { get; set; }

    public int? ForceProcessingMethod { get; set; }

    public string? PromotionCode { get; set; }

    public DateOnly? FirstPaymentDeferralDate { get; set; }

    public string? QueryCriteria { get; set; }

    public string? QueryCriteriaId { get; set; }

    public DateTime? PosDateTime { get; set; }

    public virtual ICollection<NpsServiceResponse> NpsServiceResponses { get; set; } = new List<NpsServiceResponse>();
}
