using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ReporteConciliacionModel
    {

        public long TransactionNumber { get; set; }

        /// <summary>
        /// Just for JobRun Relation
        /// </summary>
        [DataMember]
        public long CommerceItemIdPK { get; set; }

        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public long CommerceItemId { get; set; }
        [DataMember]
        public string NroCommercio { get; set; }
        [DataMember]
        public string NroEnte { get; set; }
        [DataMember]
        public DateTime FechaPago { get; set; }
        [DataMember]
        public DateTime HoraPago { get; set; }
        [DataMember]
        public string TipoTarjeta { get; set; }
        [DataMember]
        public string TarjetaInicio { get; set; }
        [DataMember]
        public string TarjetaFin { get; set; }
        [DataMember]
        public string CodigoAutorizacion { get; set; }
        [DataMember]
        public string NroTicket { get; set; }
        [DataMember]
        public decimal Importe { get; set; }
        [DataMember]
        public string NroLote { get; set; }
        [DataMember]
        public string MerchantId { get; set; }
        [DataMember]
        public string CodigoBarra { get; set; }
    }
}