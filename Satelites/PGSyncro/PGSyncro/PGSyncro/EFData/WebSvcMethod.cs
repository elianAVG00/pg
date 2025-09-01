using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class WebSvcMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal TrxCost { get; set; }

    public bool IsActive { get; set; }

    public int ValidatorId { get; set; }

    public bool IsRefund { get; set; }

    public virtual ICollection<ConfigurationDefaultValue> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValue>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetailBckpValidatorMethods { get; set; } = new List<ConfigurationDetail>();

    public virtual ICollection<ConfigurationDetail> ConfigurationDetailMainValidatorMethods { get; set; } = new List<ConfigurationDetail>();

    public virtual Validator1 Validator { get; set; } = null!;

    public virtual ICollection<WebSvcMethodParam> WebSvcMethodParams { get; set; } = new List<WebSvcMethodParam>();
}
