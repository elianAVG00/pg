using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class WebSvcMethodParam
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProposedValue { get; set; } = null!;

    public bool IsActive { get; set; }

    public int WebSvcMethodId { get; set; }

    public virtual ICollection<ConfigurationDefaultValue> ConfigurationDefaultValues { get; set; } = new List<ConfigurationDefaultValue>();

    public virtual WebSvcMethod WebSvcMethod { get; set; } = null!;
}
