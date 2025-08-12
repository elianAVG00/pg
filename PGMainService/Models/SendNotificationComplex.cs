using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class SendNotificationModelComplex
    {
        public long transactionIdPk { get; set; }
        public string statusCode { get; set; }
        public string moduleDescription { get; set; }
        public int serviceId { get; set; }
        public Dictionary<string, string> ticketModel { get; set; }

        public string finalUserMails { get; set; }

        public DateTime paymentDate { get; set; }

        public bool sentBySync { get; set; }

    }
}