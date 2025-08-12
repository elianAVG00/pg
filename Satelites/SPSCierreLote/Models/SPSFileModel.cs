namespace SPSCierreLote.Models
{
    /// <summary>Modelo global de un archivo SPS parseado.</summary>
    public class SPSFileModel
    {
        public int RecordsRead { get; set; }
        public int RecordsNotRead { get; set; }
        public List<long> UnknownTransactionNumbers { get; set; } = new();
        public bool HasPGInconsistence { get; set; }
        public List<SPSErrores> FileErrorList { get; set; } = new();
        public bool HasFileError { get; set; }
        public FileInformation FileInfoSPS { get; set; } = new();
        public List<SPSFileRecord> Records { get; set; } = new();
        public SPSFileTrailer Trailer { get; set; } = new();
    }
}
