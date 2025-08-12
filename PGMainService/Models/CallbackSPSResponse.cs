using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class SpsCallbackResponse
    {
        public string Motivo { get; set; }

        public string Moneda { get; set; }

        public string Direccionentrega { get; set; }

        public string Validaciondomicilio { get; set; }

        public string codigopedido { get; set; }

        public string nombreentrega { get; set; }

        public DateTime? fechahora { get; set; }

        public string telefonocomprador { get; set; }

        public string barrioentrega { get; set; }

        public string codautorizacion { get; set; }

        public string paisentrega { get; set; }

        public int? cuotas { get; set; }

        public string validafechanac { get; set; }

        public string validanrodoc { get; set; }

        public string titular { get; set; }

        public string pedido { get; set; }

        public string zipentrega { get; set; }

        public decimal? monto { get; set; }

        public string tarjeta { get; set; }

        public DateTime? fechaentrega { get; set; }

        public string emailcomprador { get; set; }

        public string validanropuerta { get; set; }

        public string ciudadentrega { get; set; }

        public string validatipodoc { get; set; }

        public string noperacion { get; set; }

        public string estadoentrega { get; set; }

        public string resultado { get; set; }

        public string mensajeentrega { get; set; }

        public string paramsitio { get; set; }

        public string tipodoc { get; set; }

        public string tipodocdescri { get; set; }

        public int? nrodoc { get; set; }

        public DateTime? fechacontracargo { get; set; }

        public string motivocontracargo { get; set; }

        public string sitecontracargo { get; set; }

        public string resultadoautenticacionvbv { get; set; }

        public string nrotarjetavisible { get; set; }

        public string motivoadicional { get; set; }

        public string nroticket { get; set; }

        public string idmotivo { get; set; }
    }
}