namespace SPSCierreLote.Models
{
    /// <summary>Error genérico detectado al procesar/parsing un lote.</summary>
    public class SPSErrores
    {
        public string? ErrorExceptionMessage { get; set; }
        public string? ErrorLine { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
