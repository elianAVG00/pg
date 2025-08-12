using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGPluginSPS.Models
{
    public class RefundResponse
    {
        public String AuthorizationCode { get; set; }

        public String BatchNbr { get; set; }

        public String CapturedAmount { get; set; }

        public String ClExternalMerchant { get; set; }

        public String ClExternalTerminal { get; set; }

        public String ClResponseCode { get; set; }

        public String ClResponseMessage { get; set; }

        public String ClTrxId { get; set; }

        public String Code { get; set; }

        public String Extended { get; set; }

        public String MerchOrderId { get; set; }

        public String MerchTrxRef { get; set; }

        public String MerchantId { get; set; }

        public String Message { get; set; }

        public String OriginalTrxId { get; set; }

        public String PosDateTime { get; set; }

        public String Product { get; set; }

        public String RefundedAmount { get; set; }

        public String SequenceNbr { get; set; }

        public String TicketNbr { get; set; }

        public String TrxId { get; set; }
    }
}