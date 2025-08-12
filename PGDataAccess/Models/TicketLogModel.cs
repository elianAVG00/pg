using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class TicketLogModel
    {
        [DataMember]
        public int TicketLogId { get; set; }
        [DataMember]
        public long TicketNumber { get; set; }
        [DataMember]
        public string TypeFormat { get; set; }
        [DataMember]
        public string HtmlTicket { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CreatedOn { get; set; }

        [DataMember]
        public virtual ModuleModel Module { get; set; }
        [DataMember]
        public virtual ICollection<NotificationLogModel> NotificationLog { get; set; }
    }
}