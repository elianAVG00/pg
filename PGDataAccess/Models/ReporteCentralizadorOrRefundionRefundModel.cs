using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ReporteCentralizadorOrRefundionRefundModel
    {
        [DataMember]
        public long CommerceItemId { get; set; }

        [DataMember]
        public long TransactionId { get; set; }

        [DataMember]
        public int? ExternalId { get; set; }

        [DataMember]
        public string ElectronicPaymentCode { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public decimal TrxOriginalAmount { get; set; }

        [DataMember]
        public DateTime TrxDateTime { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string CardNumber4Dig { get; set; }

        [DataMember]
        public string TrxAuthCode { get; set; }
        [DataMember]
        public string ClaimerDocType { get; set; }

        [DataMember]
        public string ClaimerDocNumber { get; set; }

        [DataMember]
        public string ClaimerLastName { get; set; }
        [DataMember]
        public string ClaimerFirstName { get; set; }

        [DataMember]
        public string ClaimerEmail { get; set; }

        [DataMember]
        public long ClaimNumber { get; set; }
        [DataMember]
        public DateTime ClaimDateTime { get; set; }


        [DataMember]
        public string CICodeAnnulled { get; set; }
        [DataMember]
        public decimal? AnnulmentAmount { get; set; }

        [DataMember]
        public DateTime? AnnulmentDateTime { get; set; }

        [DataMember]
        public string AnnulmentAuthCode { get; set; }

    }
}