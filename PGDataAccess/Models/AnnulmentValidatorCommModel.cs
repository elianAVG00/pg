using System;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class AnnulmentValidatorCommModel
    {
        [DataMember]
        public int AnnulmentValidatorCommId { get; set; }

        [DataMember]
        public Nullable<int> AnnulmentRequestId { get; set; }

        [DataMember]
        public Nullable<System.DateTime> RequestDate { get; set; }

        [DataMember]
        public string RequestMessage { get; set; }

        [DataMember]
        public Nullable<System.DateTime> ResponseDate { get; set; }

        [DataMember]
        public string ResponseMessage { get; set; }

        [DataMember]
        public virtual AnnulmentRequestModel AnnulmentRequest { get; set; }
    }
}