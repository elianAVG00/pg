using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class ConfigurationDetails
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int MainValidatorId { get; set; }

    public int MainValidatorMethodId { get; set; }

    public int MainValidatorRetries { get; set; }

    public int? BckpValidatorId { get; set; }

    public int? BckpValidatorMethodId { get; set; }

    public int BckpValidatorRetries { get; set; }

    public bool AutoFallback { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int ConfigurationId { get; set; }

    public virtual Validators1? BckpValidator { get; set; }

    public virtual WebSvcMethods? BckpValidatorMethod { get; set; }

    public virtual Configurations1 Configuration { get; set; } = null!;

    public virtual Validators1 MainValidator { get; set; } = null!;

    public virtual WebSvcMethods MainValidatorMethod { get; set; } = null!;

    public virtual Products1 Product { get; set; } = null!;
}
