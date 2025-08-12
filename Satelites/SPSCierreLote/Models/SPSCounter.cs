namespace SPSCierreLote.Models
{
    /// <summary>Contador auxiliar para validar totales del trailer.</summary>
    public record SPSCounter
    {
        public long MontoTotal { get; set; }
        public int CantidadTotal { get; set; }
        public string TipoOperacion { get; set; }
    };
}
