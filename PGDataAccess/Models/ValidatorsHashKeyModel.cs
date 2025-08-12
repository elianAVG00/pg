using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ValidatorsHashKeyModel
    {
        [DataMember]
        public int IdHash { get; set; }
        [DataMember]
        public Nullable<int> IdServiceProductsValidator { get; set; }
        [DataMember]
        public string HashType { get; set; }
        [DataMember]
        public string HashCode { get; set; }
        [DataMember]
        public Nullable<bool> IsActive { get; set; }
    }
}