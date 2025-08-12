using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ServiceModel
    {
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public int ClientId { get; set; }        
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string MerchantId { get; set; }       
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


        [DataMember]
        public Boolean IsCommerceItemsValidated { get; set; }


    
    }
}