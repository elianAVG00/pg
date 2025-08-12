using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class Validators1
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool AllowIntTrx { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? PluginServiceRequestURL { get; set; }

    public string? PluginServiceMethod { get; set; }

    public string? PluginServiceResponseType { get; set; }

    public string? PluginServiceMethodType { get; set; }

    public string? PluginServiceCallbackURL { get; set; }

    public string? PluginServiceTransactionInfoURL { get; set; }

    public bool SendMail { get; set; }

    public string? PluginServiceClaimURL { get; set; }

    public virtual ICollection<ConfigurationDetails> ConfigurationDetailsBckpValidator { get; set; } = new List<ConfigurationDetails>();

    public virtual ICollection<ConfigurationDetails> ConfigurationDetailsMainValidator { get; set; } = new List<ConfigurationDetails>();

    public virtual ICollection<RefundsRecords> RefundsRecords { get; set; } = new List<RefundsRecords>();

    public virtual ICollection<ValidatorMessages> ValidatorMessages { get; set; } = new List<ValidatorMessages>();

    public virtual ICollection<WebSvcMethods> WebSvcMethods { get; set; } = new List<WebSvcMethods>();
}
