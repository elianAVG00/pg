using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ConfigurationDefaultValues
{
    public int Id { get; set; }

    public int ConfigurationId { get; set; }

    public int WebSvcMethodId { get; set; }

    public int WebSvcMethodParamId { get; set; }

    public string? DefaultValue { get; set; }

    public string? CastToCLRType { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual Configurations1 Configuration { get; set; } = null!;

    public virtual WebSvcMethods WebSvcMethod { get; set; } = null!;

    public virtual WebSvcMethodParams WebSvcMethodParam { get; set; } = null!;
}
