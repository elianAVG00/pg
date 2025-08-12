using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class Transaction
    {
        public long IdPK { get; set; }

        public string TransactionIdFromValidator { get; set; }

        public string UniqueCode { get; set; }

        public string CallbackURL { get; set; }

        public long TransactionNumber { get; set; }

        public string CustomerMail { get; set; }

        public Decimal Amount { get; set; }

        public int payments { get; set; }

        public int ValidatorCardCode { get; set; }

        public int ServiceId { get; set; }

        public int ProductId { get; set; }

        public int ValidatorId { get; set; }
    }
}