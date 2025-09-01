using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Claim
{
    public long Id { get; set; }

    public int ClaimTypeId { get; set; }

    public int ClaimStatusId { get; set; }

    public int DocumentTypeId { get; set; }

    public string DocumentNbr { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string ClaimText { get; set; } = null!;

    public string Comments { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime? ModifiedOn { get; set; }

    public string ClosedBy { get; set; } = null!;

    public DateTime? ClosedOn { get; set; }

    public string? ClaimExternalApp { get; set; }

    public int? ClaimExternalOrderId { get; set; }

    public bool? UpdCrmcase { get; set; }

    public bool? UpdCrminvoice { get; set; }

    public string? CrmCaseId { get; set; }

    public virtual ICollection<ClaimOperation> ClaimOperations { get; set; } = new List<ClaimOperation>();

    public virtual ClaimStatus ClaimStatus { get; set; } = null!;

    public virtual ClaimType ClaimType { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;
}
