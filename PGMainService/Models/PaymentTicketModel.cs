using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class PaymentTicketModel
    {
        public string TransactionId { get; set; }

        public string ServiceDescription { get; set; }

        public string ProductName { get; set; }

        public string CardNumber { get; set; }

        public string AuthorizationCode { get; set; }

        public string CurrencyIsoCode { get; set; }

        public string Amount { get; set; }

        public DateTime PaymentDate { get; set; }

    }
}