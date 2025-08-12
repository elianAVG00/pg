using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Products1
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int NPS { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string HSR { get; set; } = null!;

    public string SPS { get; set; } = null!;

    public virtual ICollection<ClaimOperations> ClaimOperations { get; set; } = new List<ClaimOperations>();

    public virtual ICollection<ConfigurationDetails> ConfigurationDetails { get; set; } = new List<ConfigurationDetails>();

    public virtual ICollection<RefundsRecords> RefundsRecords { get; set; } = new List<RefundsRecords>();

    public virtual ICollection<ServiceProducts> ServiceProducts { get; set; } = new List<ServiceProducts>();
}
