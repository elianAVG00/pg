using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class StatusCodeModel
    {
        [DataMember]
        public int StatusCodeId { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int GenericCodeId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }



        [DataMember]
        public virtual ICollection<PaymentClaimStatusModel> PaymentClaimStatus { get; set; }
        [DataMember]
        public virtual ICollection<ChannelStatusTemplateModel> ChannelStatusTemplate { get; set; }
        //[DataMember]
        //public virtual ICollection<TransactionStatusModel> TransactionStatus { get; set; }
        //[DataMember]
        //public virtual ICollection<CodeMappingModel> CodeMapping { get; set; }
        [DataMember]
        public virtual GenericCodeModel GenericCode { get; set; }
        [DataMember]
        public virtual ICollection<StatusMessageModel> StatusMessage { get; set; }
    }
}