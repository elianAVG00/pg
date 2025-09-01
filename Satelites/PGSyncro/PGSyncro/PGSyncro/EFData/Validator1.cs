using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class Validator1
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

    public string? PluginServiceRequestUrl { get; set; }

    public string? PluginServiceMethod { get; set; }

    public string? PluginServiceResponseType { get; set; }

    public string? PluginServiceMethodType { get; set; }

    public string? PluginServiceCallbackUrl { get; set; }

    public string? PluginServiceTransactionInfoUrl { get; set; }

    public bool SendMail { get; set; }

    public string? PluginServiceClaimUrl { get; set; }

    public virtual ICollection<AnnulmentResultInfo> AnnulmentResultInfos { get; set; } = new List<AnnulmentResultInfo>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetailBckpValidators { get; set; } = new List<ConfigurationDetail>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetailMainValidators { get; set; } = new List<ConfigurationDetail>();

    public virtual ICollection<RefundsRecord> RefundsRecords { get; set; } = new List<RefundsRecord>();

    public virtual ICollection<ValidatorMessage> ValidatorMessages { get; set; } = new List<ValidatorMessage>();

    public virtual ICollection<WebSvcMethod> WebSvcMethods { get; set; } = new List<WebSvcMethod>();
}
