using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class SpsBatch
{
    public int Id { get; set; }

    public string BatchCommerceNbr { get; set; } = null!;

    public string BatchDate { get; set; } = null!;

    public string BatchCard { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    public string CardCode { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public string OperationDate { get; set; } = null!;

    public string OperationAmount { get; set; } = null!;

    public string AuthorizationCode { get; set; } = null!;

    public string CouponNumber { get; set; } = null!;

    public string CommerceNumber { get; set; } = null!;

    public string BatchNumber { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public int Payments { get; set; }

    public string CloseDate { get; set; } = null!;

    public string EstablishmentNumber { get; set; } = null!;
}
