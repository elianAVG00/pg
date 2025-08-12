using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class StatusTicketModel
    {
        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public string Ticket { get; set; }
    }
}