using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ChannelStatusTemplateModel
    {
        [DataMember]
        public int ChannelStateTemplateId { get; set; }
        [DataMember]
        public Nullable<int> ChannelId { get; set; }
        [DataMember]
        public Nullable<int> StatusCodeId { get; set; }
        [DataMember]
        public Nullable<int> NotificationTemplateId { get; set; }


        [DataMember]
        public virtual ChannelModel Channels { get; set; }
        [DataMember]
        public virtual NotificationTemplateModel NotificationTemplate { get; set; }
        [DataMember]
        public virtual StatusCodeModel StatusCode { get; set; }
    }
}