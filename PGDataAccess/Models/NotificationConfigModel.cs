using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class NotificationConfigModel
    {
        [DataMember]
        public int NotificationConfigId { get; set; }
        [DataMember]
        public Nullable<int> ServiceId { get; set; }
        [DataMember]
        public Nullable<int> ChannelStatusTemplateId { get; set; }
        [DataMember]
        public string AdditionalHeader { get; set; }
        [DataMember]
        public string AdditionalFooter { get; set; }
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
        public virtual Models.ServiceModel Services { get; set; }
        [DataMember]
        public virtual Models.ChannelStatusTemplateModel ChannelStatusTemplate { get; set; }
    }
}