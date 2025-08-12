using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ConfigurationModel
    {
        [DataMember]
        public int ConfigurationId { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public int ChannelId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int ValidatorId { get; set; }
        [DataMember]
        public string UniqueCode { get; set; }
        [DataMember]
        public string CommerceNumber { get; set; }

        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ChannelName { get; set; }
        [DataMember]
        public string ValidatorName { get; set; }
        [DataMember]
        public string ProductName { get; set; }


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
    }
}