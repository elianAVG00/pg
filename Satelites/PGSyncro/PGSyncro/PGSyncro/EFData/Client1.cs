using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Client1
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

    public virtual ICollection<SalePoint> SalePoints { get; set; } = new List<SalePoint>();

    public virtual ICollection<Service1> Service1s { get; set; } = new List<Service1>();
}
