using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ValidatorServiceConfigModel
    {
        [DataMember]
        public int ValidatorServiceConfigId { get; set; }

        [DataMember]
        public int ValidatorId { get; set; }

        [DataMember]
        public int ServiceId { get; set; }

        [DataMember]
        public string ValidatorUser { get; set; }

        [DataMember]
        public string ValidatorPass { get; set; }

        [DataMember]
        public string HashKey { get; set; }
    }
}