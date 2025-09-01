using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class ConfigurationDefaultValue
{
    public int Id { get; set; }

    public int ConfigurationId { get; set; }

    public int WebSvcMethodId { get; set; }

    public int WebSvcMethodParamId { get; set; }

    public string? DefaultValue { get; set; }

    public string? CastToClrtype { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual Configuration1 Configuration { get; set; } = null!;

    public virtual WebSvcMethod WebSvcMethod { get; set; } = null!;

    public virtual WebSvcMethodParam WebSvcMethodParam { get; set; } = null!;
}
