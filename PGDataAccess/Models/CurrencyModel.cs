using System;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class CurrencyModel
    {

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string IsoCode { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }
    }
}