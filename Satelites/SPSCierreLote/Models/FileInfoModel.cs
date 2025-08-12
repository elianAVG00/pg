namespace SPSCierreLote.Models
{
    /// <summary>Metadatos del archivo localizado en origen/FTP.</summary>
    public class FileInfoModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        /// <summary>Fecha/hora según el sistema de archivos o la lista FTP.</summary>
        public DateTime DateCreated { get; set; }
    }
}
