using System;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class StatusCodeStatusMessageModel
    {
        [DataMember]
        public string Message { set; get; }

        [DataMember]
        public string Code { set; get; }

        [DataMember]
        public string Observation { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}