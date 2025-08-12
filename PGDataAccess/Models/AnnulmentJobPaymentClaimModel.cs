using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class AnnulmentJobPaymentClaimModel
    {
        [DataMember]
        public int AnnulmentJobPaymentClaimId { get; set; }
        [DataMember]
        public Nullable<int> AnnulmentJobId { get; set; }
        [DataMember]
        public Nullable<int> PaymentClaimId { get; set; }
        [DataMember]
        public Nullable<int> AnnulmentRequestId { get; set; }



        [DataMember]
        public virtual PaymentClaimModel PaymentClaim { get; set; }
        [DataMember]
        public virtual AnnulmentRequestModel AnnulmentRequest { get; set; }
    }
}