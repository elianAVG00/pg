using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class TransactionTicket
{
    public long TransactionTicketId { get; set; }

    public long TransactionIdPk { get; set; }

    public int StatusTemplateId { get; set; }

    public long TicketNumber { get; set; }

    public bool EmailSent { get; set; }

    public DateTime? EmailSentDate { get; set; }

    public bool SentBySync { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
