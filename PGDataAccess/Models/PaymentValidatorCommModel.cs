using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class PaymentValidatorCommModel
    {
        [DataMember]
        public long PaymentValidatorCommId { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public Nullable<System.DateTime> RequestDate { get; set; }
        [DataMember]
        public string RequestMessage { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ResponseDate { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
    }
}