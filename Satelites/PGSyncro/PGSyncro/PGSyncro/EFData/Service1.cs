using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Service1
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string MerchantId { get; set; } = null!;

    public string MerchantEmail { get; set; } = null!;

    public int OrderNbr { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int ClientId { get; set; }

    public int? ExternalId { get; set; }

    public int? BranchId { get; set; }

    public int? TerminalId { get; set; }

    public string? SenderDisplayName { get; set; }

    public string? SenderEmailAddress { get; set; }

    public virtual ICollection<ClaimOperation> ClaimOperations { get; set; } = new List<ClaimOperation>();

    public virtual Client1 Client { get; set; } = null!;

    public virtual ICollection<RefundsRecord> RefundsRecords { get; set; } = new List<RefundsRecord>();

    public virtual ICollection<ServiceChannel> ServiceChannels { get; set; } = new List<ServiceChannel>();

    public virtual ICollection<ServiceProduct> ServiceProducts { get; set; } = new List<ServiceProduct>();

    public virtual ICollection<SalePoint> SalePoints { get; set; } = new List<SalePoint>();
}
