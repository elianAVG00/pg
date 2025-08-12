namespace SPSCierreLote.Models
{
    /// <summary>POCO genérico para devolver (Id, estado booleano y mensaje).</summary>
    public class Generic
    {
        public long Id { get; set; }
        public bool Booleano { get; set; }
        public string Cadena { get; set; } = string.Empty;
    }
}
