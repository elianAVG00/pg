namespace PGExporter.Models
{
    public class CentralizerReportModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public long CommerceItemId { get; set; }

        public string Datos { get; set; }

        public string Spacer1 { get; set; }

        public string CodigoBCRA { get; set; }

        public string CodigoR { get; set; }

        public int? CodigoTerminal { get; set; }

        public string Spacer2 { get; set; }

        public int? CodigoSucursal { get; set; }

        public string NroSecuenciaOn { get; set; }

        public long NroSecuenciaTrans { get; set; }

        public string CodigoOperacion { get; set; }

        public string Desde { get; set; }

        public string Hasta { get; set; }

        public int? CodigoEnte { get; set; }

        public long CodigoServicio { get; set; }

        public Decimal Importe { get; set; }

        public string Interes { get; set; }

        public string Recargo { get; set; }

        public string Moneda { get; set; }

        public string CodigoCajero { get; set; }

        public string FondoEducativo { get; set; }

        public string Spacer3 { get; set; }

        public string CodigoSeguridad { get; set; }

        public string FechaVto1 { get; set; }

        public string FechaVto2 { get; set; }

        public string BancoCheque { get; set; }

        public string BancoSucursal { get; set; }

        public string BancoCodPostal { get; set; }

        public string NroCheque { get; set; }

        public string NroCuenta { get; set; }

        public string Plazo { get; set; }

        public string CodigoBarra { get; set; }

        public DateTime FechaPago { get; set; }

        public string Spacer4 { get; set; }

        public string AnioCuota { get; set; }

        public string FoProvi { get; set; }

        public string FormaPago { get; set; }

        public string Jurisdiccion { get; set; }

        public string Spacer5 { get; set; }

        public string Autorizacion { get; set; }

        public string NroAnulacion { get; set; }

        public long IdPK { get; set; }
    }
}
