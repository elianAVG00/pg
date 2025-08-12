using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGPluginSPS.Utils;

namespace PGPluginSPS.Models
{
    public class AnnulationOrderModel
    {
        public int PaymentClaimId { get; set; }
        public int RequestOrder { get; set; }
        public WebServiceInterface.Operation OperationType { get; set; }
        public string TransactionId { get; set; }
        public string Import { get; set; }
        public string Cents { get; set; }
    }
}