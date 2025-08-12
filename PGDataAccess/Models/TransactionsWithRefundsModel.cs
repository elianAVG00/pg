using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataAccess.Models
{
    public class TransactionsWithRefundsModel
    {
        public string TransactionId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string EPC { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public decimal TransactionAmount { get; set; }
        public string ClientCode { get; set; }
        public string Service { get; set; }
        public string RefundDate { get; set; }
        public string RefundCurrencyCode { get; set; }
        public string RefundAmount { get; set; }
        public string ProductName { get; set; }
        public string CardMask { get; set; }
        public string AuthorizationCode { get; set; }

    }
}