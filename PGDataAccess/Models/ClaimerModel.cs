using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ClaimerModel
    {
        [DataMember]
        public int ClaimerId { get; set; }
        [DataMember]
        public string DocNumber { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Cellphone { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        [DataMember]
        public int DocTypeId { get; set; }
        [DataMember]
        public string DocShortName { get; set; }
    }
}