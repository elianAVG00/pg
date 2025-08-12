using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class NPSClaimTransactionRelationModel
    {
        [DataMember]
        public int NPSClaimTransactionRelationId { get; set; }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string merchantTransaction { get; set; }
        [DataMember]
        public string ClaimTransactionId { get; set; }
        [DataMember]
        public string merchantClaimTransaction { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }

    }
}