using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class TransactionResultInfo
{
    public int TransactionResultInfoId { get; set; }

    public long TransactionIdPK { get; set; }

    public string? ResponseCode { get; set; }

    public string? StateMessage { get; set; }

    public string? StateExtendedMessage { get; set; }

    public long? Amount { get; set; }

    public int? Payments { get; set; }

    public string? Currency { get; set; }

    public string? Country { get; set; }

    public string? AuthorizationCode { get; set; }

    public string? CardMask { get; set; }

    public string? CardNbrLfd { get; set; }

    public string? CardHolder { get; set; }

    public string? CustomerDocType { get; set; }

    public string? CustomerDocNumber { get; set; }

    public string? TicketNumber { get; set; }

    public string? CustomerEmail { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? BatchNbr { get; set; }

    public DateTime? SynchronizationDate { get; set; }

    public DateTime? BatchSPSDate { get; set; }

    public DateTime? MailSynchronizationDate { get; set; }

    public virtual Transactions TransactionIdPKNavigation { get; set; } = null!;
}
