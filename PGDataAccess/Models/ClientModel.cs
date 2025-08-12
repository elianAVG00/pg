using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ClientModel
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string TributaryCode { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string LegalName { get; set; }
        [DataMember]
        public string SupportMail { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public DateTime? UpdatedOn { get; set; }


    
    }
}