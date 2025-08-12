using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class NotificationTemplateModel
    {
        [DataMember]
        public int NotificationTemplateId { get; set; }
        [DataMember]
        public string TemplateSubject { get; set; }
        [DataMember]
        public string TemplateBody { get; set; }
        [DataMember]
        public string TemplateTicket { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }


        [DataMember]
        public virtual ICollection<ChannelStatusTemplateModel> ChannelStateTemplate { get; set; }
    }
}