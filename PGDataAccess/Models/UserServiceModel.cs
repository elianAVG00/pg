using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class UserServiceModel
    {
        [DataMember]
        public int UserServiceId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }


        [DataMember]
        public virtual ServiceModel Services { get; set; }
        [DataMember]
        public virtual UserModel User { get; set; }
    }
}