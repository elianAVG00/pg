using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class NotificationLogModel
    {
        [DataMember]
        public int NotificationLogId { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public int? TicketLogId { get; set; }
        [DataMember]
        public string TypeFormat { get; set; }
        [DataMember]
        public string HtmlNotification { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public virtual ModuleModel Module { get; set; }
        
        [DataMember]
        public virtual TicketLogModel TicketLog { get; set; }
    }
}