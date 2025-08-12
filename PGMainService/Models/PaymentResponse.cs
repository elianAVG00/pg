using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class PaymentResponseInternal
    {
        public string ValidatorTransactionId { get; set; }
        public string TransactionNumber { get; set; }
        public long TransactionIdPK { get; set; }
        public string HTMLToResponse { get; set; }
        public int ResponseStatus { get; set; }
    }

    public class PaymentResponse {
        public string CardHolderInterface { get; set; }
        public string TransactionId { get; set; }
    }
}