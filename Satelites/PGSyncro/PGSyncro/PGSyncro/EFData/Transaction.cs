using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Transaction
{
    public long Id { get; set; }

    public string InternalNbr { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public string Client { get; set; } = null!;

    public string SalePoint { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string Product { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Validator { get; set; } = null!;

    public string WebSvcMethod { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    public string JsonObject { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public int SettingId { get; set; }

    public string MerchantId { get; set; } = null!;

    public string ElectronicPaymentCode { get; set; } = null!;

    public string CurrencyCode { get; set; } = null!;

    public string TrxCurrencyCode { get; set; } = null!;

    public decimal TrxAmount { get; set; }

    public decimal ConvertionRate { get; set; }

    public virtual ICollection<AnnulmentResultInfo> AnnulmentResultInfos { get; set; } = new List<AnnulmentResultInfo>();

    public virtual ICollection<CommerceItem> CommerceItems { get; set; } = new List<CommerceItem>();

    public virtual ICollection<PaymentClaim> PaymentClaims { get; set; } = new List<PaymentClaim>();

    public virtual ICollection<PaymentValidatorComm> PaymentValidatorComms { get; set; } = new List<PaymentValidatorComm>();

    public virtual ICollection<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; } = new List<TransactionAdditionalInfo>();

    public virtual ICollection<TransactionResultInfo> TransactionResultInfos { get; set; } = new List<TransactionResultInfo>();

    public virtual ICollection<TransactionStatus> TransactionStatuses { get; set; } = new List<TransactionStatus>();
}
