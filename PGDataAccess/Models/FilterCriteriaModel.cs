using System;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class FilterCriteriaModel
    {
        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public DateTime? DateStart { get; set; }

        [DataMember]
        public DateTime? DateEnd { get; set; }

        [DataMember]
        public string PaymentClaimNumber { get; set; }

        [DataMember]
        public int? Type { get; set; }

        [DataMember]
        public int? ProductId { get; set; }
    }
}