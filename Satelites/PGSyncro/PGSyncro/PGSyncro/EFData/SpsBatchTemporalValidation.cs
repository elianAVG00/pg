using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class SpsBatchTemporalValidation
{
    public long TransactionNumber { get; set; }

    public int MedioDePago { get; set; }

    public DateTime FechaDeOperacion { get; set; }

    public long CurrentAmount { get; set; }

    public long Idsite { get; set; }

    public bool EsCargo { get; set; }
}
