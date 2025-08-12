using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ReporteCentralizadorModel
    {

        [DataMember]
        public long CommerceItemId { get; set; }

        [DataMember]
        public string Datos { get; set; }
        [DataMember]
        public string Spacer1 { get; set; }
        [DataMember]
        public string CodigoBCRA { get; set; }
        [DataMember]
        public string CodigoR { get; set; }
        [DataMember]
        public int? CodigoTerminal { get; set; }
        [DataMember]
        public string Spacer2 { get; set; }
        [DataMember]
        public int? CodigoSucursal { get; set; }
        [DataMember]
        public string NroSecuenciaOn { get; set; }
        [DataMember]
        public long NroSecuenciaTrans { get; set; }
        [DataMember]
        public string CodigoOperacion { get; set; }
        [DataMember]
        public string Desde { get; set; }
        [DataMember]
        public string Hasta { get; set; }
        [DataMember]
        public int? CodigoEnte { get; set; }
        [DataMember]
        public long CodigoServicio { get; set; }
        [DataMember]
        public decimal Importe { get; set; }
        [DataMember]
        public string Interes { get; set; }
        [DataMember]
        public string Recargo { get; set; }
        [DataMember]
        public string Moneda { get; set; }
        [DataMember]
        public string CodigoCajero { get; set; }
        [DataMember]
        public string FondoEducativo { get; set; }
        [DataMember]
        public string Spacer3 { get; set; }
        [DataMember]
        public string CodigoSeguridad { get; set; }
        [DataMember]
        public string FechaVto1 { get; set; }
        [DataMember]
        public string FechaVto2 { get; set; }
        [DataMember]
        public string BancoCheque { get; set; }
        [DataMember]
        public string BancoSucursal { get; set; }
        [DataMember]
        public string BancoCodPostal { get; set; }
        [DataMember]
        public string NroCheque { get; set; }
        [DataMember]
        public string NroCuenta { get; set; }
        [DataMember]
        public string Plazo { get; set; }
        [DataMember]
        public string CodigoBarra { get; set; }
        [DataMember]
        public DateTime FechaPago { get; set; }
        [DataMember]
        public string Spacer4 { get; set; }
        [DataMember]
        public string AnioCuota { get; set; }
        [DataMember]
        public string FoProvi { get; set; }
        [DataMember]
        public string FormaPago { get; set; }
        [DataMember]
        public string Jurisdiccion { get; set; }
        [DataMember]
        public string Spacer5 { get; set; }
        [DataMember]
        public string Autorizacion { get; set; }
        [DataMember]
        public string NroAnulacion { get; set; }
    }
}