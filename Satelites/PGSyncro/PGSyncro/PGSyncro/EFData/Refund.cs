using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Refund
{
    public long Id { get; set; }

    public long ClaimOperationId { get; set; }

    public long OriginalTrxId { get; set; }

    public long AmountToRefund { get; set; }

    public bool ClientValidated { get; set; }

    public bool ClientValidationResult { get; set; }

    public DateTime? ClientValidationDate { get; set; }

    public string ClientMessage { get; set; } = null!;

    public DateTime? RobotRunDate { get; set; }

    public int ValidatorId { get; set; }

    public bool ValidatorValidated { get; set; }

    public bool ValidatorValidationResult { get; set; }

    public DateTime? ValidatorValidationDate { get; set; }

    public string ValidatorMessage { get; set; } = null!;

    public bool CallCenterValidated { get; set; }

    public bool CallCenterValidationResult { get; set; }

    public DateTime? CallCenterValidationDate { get; set; }

    public string CallCenterMessage { get; set; } = null!;

    public DateTime? MailSendedToCustomerDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public short? Retries { get; set; }

    public virtual ClaimOperation ClaimOperation { get; set; } = null!;

    public virtual ICollection<RefundsRecord> RefundsRecords { get; set; } = new List<RefundsRecord>();
}
