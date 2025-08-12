namespace SPSCierreLote.Models
{
    /// <summary>Error concreto al comparar un dato PG vs SPS en la homologación.</summary>
    public class SPSErrorValidacionDato
    {
        public string? Valor_DECIDIR { get; set; }
        public string? Valor_PG { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
