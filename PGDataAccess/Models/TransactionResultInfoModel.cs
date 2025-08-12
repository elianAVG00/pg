using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class TransactionResultInfoModel
    {
        [DataMember]
        public int TransactionResultInfoId { get; set; }
        [DataMember]
        public long TransactionIdPK { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string StateMessage { get; set; }
        [DataMember]
        public string StateExtendedMessage { get; set; }
        [DataMember]
        public Nullable<long> Amount { get; set; }
        [DataMember]
        public Nullable<int> Payments { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string AuthorizationCode { get; set; }
        [DataMember]
        public string CardMask { get; set; }
        [DataMember]
        public string CardNbrLfd { get; set; }
        [DataMember]
        public string CardHolder { get; set; }
        [DataMember]
        public string TicketNumber { get; set; }
        [DataMember]
        public string CustomerEmail { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string CustomerDocType { get; set; }
        [DataMember]
        public string CustomerDocNumber { get; set; }
        [DataMember]
        public string BatchNbr { get; set; }
    }
}