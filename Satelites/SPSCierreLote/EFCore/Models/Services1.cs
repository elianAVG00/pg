using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Services1
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

    public virtual ICollection<ClaimOperations> ClaimOperations { get; set; } = new List<ClaimOperations>();

    public virtual Clients1 Client { get; set; } = null!;

    public virtual ICollection<RefundsRecords> RefundsRecords { get; set; } = new List<RefundsRecords>();

    public virtual ICollection<ServiceChannels> ServiceChannels { get; set; } = new List<ServiceChannels>();

    public virtual ICollection<ServiceProducts> ServiceProducts { get; set; } = new List<ServiceProducts>();

    public virtual ICollection<SalePoints> SalePoint { get; set; } = new List<SalePoints>();
}
