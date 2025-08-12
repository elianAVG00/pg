namespace SPSCierreLote.Models
{
    /// <summary>Registro individual (línea “D…”) dentro del lote SPS.</summary>
    public class SPSFileRecord
    {
        public List<SPSErrorValidacionDato> PGDataInconsistenceList { get; set; } = new();

        public bool HasPGInconsistence { get; set; }
        public bool EsCargo { get; set; }

        public long TransactionIDPK { get; set; }
        public long TransctionId { get; set; }
        public string OriginalTID { get; set; } = string.Empty;

        public int MedioDePago { get; set; }
        public string CardMask { get; set; } = string.Empty;
        public string TipoOperacion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }
        public long Monto { get; set; }
        public string OriginalOperationAmount { get; set; } = string.Empty;

        public string AuthorizationCode { get; set; } = string.Empty;
        public string NumeroDeCupon { get; set; } = string.Empty;
        public string IDSITE { get; set; } = string.Empty;
        public string IDLOTE { get; set; } = string.Empty;

        public int Cuotas { get; set; }
        public DateTime FechaCierre { get; set; }
        public string NumeroDeEstablecimiento { get; set; } = string.Empty;
        public string? IDCLIENTE { get; set; }

        public int Filler { get; set; }

        // Flags de inconsistencia detectados en el SP de validación
        public bool inconsistence_days { get; set; }
        public bool inconsistence_dups { get; set; }
        public bool inconsistence_date { get; set; }
        public bool inconsistence_uniq { get; set; }
        public bool inconsistence_card { get; set; }
        public bool inconsistence_cost { get; set; }
    }
}
