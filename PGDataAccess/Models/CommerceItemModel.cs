using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class CommerceItemModel
    {
        [DataMember]
        public long CommerceItemId { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }

        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public System.DateTime? UpdatedOn { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string OriginalCode { get; set; }


    }
}