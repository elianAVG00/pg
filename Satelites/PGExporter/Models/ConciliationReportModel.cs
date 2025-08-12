namespace PGExporter.Models
{
    public class ConciliationReportModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public long IdPK { get; set; }

        public bool IsIncomplete { get; set; }

        public string TransactionNumber { get; set; }

        public string TransactionId { get; set; }

        public long CommerceItemId { get; set; }

        public string NroCommercio { get; set; }

        public string NroEnte { get; set; }

        public DateTime FechaPago { get; set; }

        public DateTime HoraPago { get; set; }

        public string TipoTarjeta { get; set; }

        public string TarjetaInicio { get; set; }

        public string TarjetaFin { get; set; }

        public string CodigoAutorizacion { get; set; }

        public string NroTicket { get; set; }

        public Decimal Importe { get; set; }

        public string NroLote { get; set; }

        public string MerchantId { get; set; }

        public string CodigoBarra { get; set; }

        public bool IsDebit { get; set; }
    }
}
