using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ModuleModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Nullable<int> validatorId { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Description { get; set; }
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
        public virtual ICollection<NotificationLogModel> NotificationLog { get; set; }

        //[DataMember]
        //public virtual ICollection<TicketLogModel> TicketLog { get; set; }
    }
}