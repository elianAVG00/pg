using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class TransactionAdditionalInfo
{
    public int TransactionAdditionalInfoId { get; set; }

    public long TransactionIdPk { get; set; }

    public string? VersionUsed { get; set; }

    public string? CustomerMail { get; set; }

    public int ProductId { get; set; }

    public int ServiceId { get; set; }

    public int ValidatorId { get; set; }

    public int ClientId { get; set; }

    public int? ExternalApp { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsActive { get; set; }

    public int ChannelId { get; set; }

    public int LanguageId { get; set; }

    public int CurrencyId { get; set; }

    public string MerchantId { get; set; } = null!;

    public string UniqueCode { get; set; } = null!;

    public int Payments { get; set; }

    public string? BarCode { get; set; }

    public string CallbackUrl { get; set; } = null!;

    public bool? IsEpcvalidated { get; set; }

    public string? EpcvalidateUrl { get; set; }

    public decimal CurrentAmount { get; set; }

    public string? ProdVersionUsed { get; set; }

    public bool IsCommerceItemValidated { get; set; }

    public bool IsSimulation { get; set; }

    public long TransactionNumber { get; set; }

    public bool WasTimeOutForced { get; set; }

    public bool WasSyncByJob { get; set; }

    public DateTime? ReportDateConciliation { get; set; }

    public DateTime? ReportDateCentralizer { get; set; }

    public DateTime? ReportDateRendition { get; set; }

    public string? AuthorizationCode { get; set; }

    public string? CardMask { get; set; }

    public string? CardHolder { get; set; }

    public string? TicketNumber { get; set; }

    public string? BatchNbr { get; set; }

    public DateTime? SynchronizationDate { get; set; }

    public DateTime? BatchSpsdate { get; set; }

    public int LastStatus { get; set; }

    public DateTime? ConfirmedByAutoBackOffice { get; set; }

    public DateTime? ConfirmedByManualSupportOn { get; set; }

    public string? ConfirmedByManualSupportBy { get; set; }

    public string? OriginalReason { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Transaction TransactionIdPkNavigation { get; set; } = null!;

    public virtual Validator Validator { get; set; } = null!;
}
