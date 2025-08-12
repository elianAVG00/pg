using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class DocTypeModel
    {
        [DataMember]
        public int DocTypeId { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public virtual ICollection<ClaimerModel> Claimer { get; set; }
    }
}