using System;

namespace PGSyncro.Models
{

    public class SPSXMLServiceResponse
    {
        public string Motivo { get; set; }

        public string Moneda { get; set; }

        public string Direccionentrega { get; set; }

        public string Validaciondomicilio { get; set; }

        public string CodigoPedido { get; set; }

        public string NombreEntrega { get; set; }

        public DateTime? FechaHora { get; set; }

        public string TelefonoComprador { get; set; }

        public string BarrioEntrega { get; set; }

        public string CodAutorizacion { get; set; }

        public string PaisEntrega { get; set; }

        public int? Cuotas { get; set; }

        public string ValidaFechaNac { get; set; }

        public string ValidaNroDoc { get; set; }

        public string Titular { get; set; }

        public string Pedido { get; set; }

        public string ZipEntrega { get; set; }

        public Decimal? Monto { get; set; }

        public string Tarjeta { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string EmailComprador { get; set; }

        public string ValidaNroPuerta { get; set; }

        public string CiudadEntrega { get; set; }

        public string ValidaTipoDoc { get; set; }

        public string NOperacion { get; set; }

        public string EstadoEntrega { get; set; }

        public string Resultado { get; set; }

        public string MensajeEntrega { get; set; }

        public string ParamSitio { get; set; }

        public string TipoDoc { get; set; }

        public string TipoDocDescri { get; set; }

        public int? NroDoc { get; set; }

        public DateTime? FechaContraCargo { get; set; }

        public string MotivoContraCargo { get; set; }

        public string SiteContraCargo { get; set; }

        public string ResultadoAutenticacionVBV { get; set; }

        public string NroTarjetaVisible { get; set; }

        public string MotivoAdicional { get; set; }

        public string NroTicket { get; set; }

        public string IdMotivo { get; set; }
    }
}