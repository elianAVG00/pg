using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{

    [DataContract]
    public class TicketModel
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string TicketNumber { get; set; }


        [DataMember]
        public string StatusCode { get; set; }


        [DataMember]
        public string StatusMessage { get; set; }
    }

    public class TicketsModel
    {

        [DataMember]
        public long TransactionNumber { get; set; }

        [DataMember]
        public long TicketNumber { get; set; }


        [DataMember]
        public bool NotificationSent { get; set; }

        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public string StatusMessage { get; set; }
    }
}