using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class CommerceItemClaimModel
    {
        [DataMember]
        public int CommerceItemClaimId { get; set; }
        [DataMember]
        public long CommerceItemId { get; set; }
        [DataMember]
        public long PaymentClaimId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}