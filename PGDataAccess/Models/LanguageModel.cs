using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class LanguageModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ISO6391 { get; set; }
        [DataMember]
        public string ISO6392 { get; set; }
        [DataMember]
        public string ISO3166 { get; set; }
        [DataMember]
        public string NativeName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual ICollection<PaymentClaimModel> PaymentClaim { get; set; }
    }
}