using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class WebSvcMethodParams
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProposedValue { get; set; } = null!;

    public bool IsActive { get; set; }

    public int WebSvcMethodId { get; set; }

    public virtual ICollection<ConfigurationDefaultValues> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValues>();

    public virtual WebSvcMethods WebSvcMethod { get; set; } = null!;
}
