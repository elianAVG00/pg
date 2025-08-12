using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class RolModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string shortName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

    }
}