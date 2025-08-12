namespace SPSCierreLote.Models
{
    /// <summary>Información extraída del nombre del archivo lote (IDSITE, producto y fecha).</summary>
    public class FileInformation
    {
        public bool IsValidFilename { get; set; }
        public string FileIDSITE { get; set; }
        public DateTime FileDatetime { get; set; }
        public string FileDATE { get; set; }
        public string FileProductCode { get; set; }
        public string FileName { get; set; }
    }
}
