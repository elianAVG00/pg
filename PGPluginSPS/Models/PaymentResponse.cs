using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGPluginSPS.Models
{
    public class PaymentResponse
    {
        public string ValidatorTransactionId { get; set; }
        public long TransactionNumber { get; set; }
        public long TransactionIdPK { get; set; }
        public string HTMLToResponse { get; set; }
        public int ResponseStatus { get; set; }
    }
}