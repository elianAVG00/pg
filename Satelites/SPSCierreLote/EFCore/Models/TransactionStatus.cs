using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class TransactionStatus
{
    public int TransactionStatusId { get; set; }

    public long TransactionsId { get; set; }

    public int StatusCodeId { get; set; }

    public bool IsActual { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool? IsActive { get; set; }

    public virtual Transactions Transactions { get; set; } = null!;
}
