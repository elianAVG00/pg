using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class AnnulmentRequestModel
    {
        [DataMember]
        public int AnnulmentRequestId { get; set; }
        [DataMember]
        public Nullable<long> PaymentClaimId { get; set; }
        [DataMember]
        public Nullable<System.DateTime> Date { get; set; }
        [DataMember]
        public string ResponseModuleCode { get; set; }
        [DataMember]
        public string Result { get; set; }
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


        [DataMember]
        public virtual PaymentClaimModel PaymentClaim { get; set; }
        [DataMember]
        public virtual ICollection<AnnulmentValidatorCommModel> AnnulmentValidatorComm { get; set; }
    }
}