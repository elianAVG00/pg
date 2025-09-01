using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Product1
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Nps { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string Hsr { get; set; } = null!;

    public string Sps { get; set; } = null!;

    public virtual ICollection<ClaimOperation> ClaimOperations { get; set; } = new List<ClaimOperation>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetails { get; set; } = new List<ConfigurationDetail>();

    public virtual ICollection<RefundsRecord> RefundsRecords { get; set; } = new List<RefundsRecord>();

    public virtual ICollection<ServiceProduct> ServiceProducts { get; set; } = new List<ServiceProduct>();
}
