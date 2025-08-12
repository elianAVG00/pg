using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class TicketDetailsModel
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string ClientLegalName { get; set; }
        [DataMember]
        public string ServiceDescripcion { get; set; }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string Payments { get; set; }
    }
}