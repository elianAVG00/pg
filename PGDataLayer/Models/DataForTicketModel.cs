using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class DataForTicketModel
    {
        public long TransactionIdPK { get; set; }

        public int ServiceId { get; set; }

        public DateTime CreatedOn { get; set; }

        public long TransactionNumber { get; set; }

        public Decimal CurrentAmount { get; set; }

        public int StatusCodeId { get; set; }

        public string ProductName { get; set; }

        public string ServiceDescription { get; set; }

        public string CurrencyISO { get; set; }

        public string CustomerMail { get; set; }
    }
}