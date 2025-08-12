using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class SpsBatchTemporalValidation
{
    public long TransactionNumber { get; set; }

    public int MedioDePago { get; set; }

    public DateTime FechaDeOperacion { get; set; }

    public long CurrentAmount { get; set; }

    public long IDSITE { get; set; }

    public bool EsCargo { get; set; }
}
