using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ReporteRenditionExportModel
    {
        [DataMember]
        public string merchantId { get; set; }

        [DataMember]
        public string RenditionFolder { get; set; }

        [DataMember]
        public string serviceName { get; set; }
    }

    [DataContract]
    public class ReporteRenditionModel
    {
        public long TransactionNumber { get; set; }

        [DataMember]
        public long CommerceItemId { get; set; }

        [DataMember]
        public string ElectronicPaymentCode { get; set; }

        [DataMember]
        public string TrxID { get; set; }

        [DataMember]
        public string TrxSource { get; set; }

        [DataMember]
        public string BatchNbr { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public string CustomerMail { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string CardHolderName { get; set; }

        [DataMember]
        public string CardMask { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public string MerchOrderId { get; set; }

        [DataMember]
        public string MerchTrxRef { get; set; }

        [DataMember]
        public string NumPayments { get; set; }

        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string PosDateTime { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Price { get; set; }
    }
}