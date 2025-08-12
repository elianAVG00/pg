namespace SPSCierreLote.Models
{
    /// <summary>Trailer (“T…”) que cierra el archivo SPS.</summary>
    public class SPSFileTrailer
    {
        public int CantidadDeRegistros { get; set; }
        public int IDMedioDePago { get; set; }
        public string IDLote { get; set; } = string.Empty;

        public int CantidadCompras { get; set; }
        public long MontoCompras { get; set; }

        public int CantidadDevueltas { get; set; }
        public long MontoDevueltas { get; set; }

        public int CantidadAnuladas { get; set; }
        public long MontoAnuladas { get; set; }

        public int Filler { get; set; }
    }
}
