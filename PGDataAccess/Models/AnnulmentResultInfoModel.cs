using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class AnnulmentResultInfoModel
    {
        [DataMember]
        public int AnnulmentResultInfoId { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public long? PaymentClaimId { get; set; }
        [DataMember]
        public int ValidatorId { get; set; }
        [DataMember]
        public string OperationNumber { get; set; }
        [DataMember]
        public string AuthorizationCode { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public System.DateTime OriginalDateTime { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }

    }
}