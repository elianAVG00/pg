namespace SPSCierreLote.Models
{
    /// <summary>Relación IDSITE ↔ producto utilizada para validar nombres de archivo.</summary>
    public class SiteProduct
    {
        public int ProductId { get; set; }
        public string? IDSite { get; set; }
        public int SPSNormal { get; set; }
        public int SPSPrisma { get; set; }
    }
}
