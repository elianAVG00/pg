namespace SPSCierreLote.Models
{
    /// <summary>
    /// Resultado compuesto al registrar un archivo en <c>MonitorSPSBatchProcessFiles</c>.
    /// </summary>
    public class GenericWithFileInfo
    {
        public Generic generico { get; set; }
        public FileInformation FileInformation { get; set; }
        public bool skipThisFile { get; set; }
        public string? file_content { get; set; }
    }
}
