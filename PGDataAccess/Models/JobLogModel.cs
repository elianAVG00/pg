using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace PGDataAccess.Models
{
    [DataContract]
    public class JobLogModel
    {
        [DataMember]
        public int JobRunLogId { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public bool IsFinish { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Parameters { get; set; }
        [DataMember]
        public string SendNotificationTo { get; set; }


    }
}