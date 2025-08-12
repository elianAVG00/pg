using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class WebSvcMethods
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal TrxCost { get; set; }

    public bool IsActive { get; set; }

    public int ValidatorId { get; set; }

    public bool IsRefund { get; set; }

    public virtual ICollection<ConfigurationDefaultValues> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValues>();

    public virtual ICollection<ConfigurationDetails> ConfigurationDetailsBckpValidatorMethod { get; set; } = new List<ConfigurationDetails>();

    public virtual ICollection<ConfigurationDetails> ConfigurationDetailsMainValidatorMethod { get; set; } = new List<ConfigurationDetails>();

    public virtual Validators1 Validator { get; set; } = null!;

    public virtual ICollection<WebSvcMethodParams> WebSvcMethodParams { get; set; } = new List<WebSvcMethodParams>();
}
