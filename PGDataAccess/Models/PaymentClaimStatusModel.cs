using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class PaymentClaimStatusModel
    {
        [DataMember]
        public int PaymentClaimStatusId { get; set; }
        [DataMember]
        public Nullable<long> PaymentClaimId { get; set; }
        [DataMember]
        public Nullable<int> StatusCodeId { get; set; }
        [DataMember]
        public string Observation { get; set; }
        [DataMember]
        public Nullable<long> TicketNumber { get; set; }
        [DataMember]
        public bool IsActual { get; set; }
        [DataMember]
        bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }


        [DataMember]
        public virtual PaymentClaimModel PaymentClaim { get; set; }
        [DataMember]
        internal virtual StatusCodeModel Status { get; set; }
    }
}