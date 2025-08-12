using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Clients1
{
    public int Id { get; set; }

    public string TributaryCode { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string LegalName { get; set; } = null!;

    public string MailAddress { get; set; } = null!;

    public string MailUserName { get; set; } = null!;

    public string MailPassword { get; set; } = null!;

    public string ReturnUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool RptToRendition { get; set; }

    public bool RptToConciliation { get; set; }

    public bool RptToCentralizer { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int SettingId { get; set; }

    public virtual ICollection<SalePoints> SalePoints { get; set; } = new List<SalePoints>();

    public virtual ICollection<Services1> Services1 { get; set; } = new List<Services1>();
}
