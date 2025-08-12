using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGPluginSPS.Models
{
    public class RefundRequest
    {
        public String Enviroment { get; set; }

        public String Amount { get; set; }

        public String CardExpDate { get; set; }

        public String CardNumber { get; set; }

        public String CardSecurityCode { get; set; }

        public String MerchTrxRef { get; set; }

        public String MerchantId { get; set; }

        public String OriginalTrxId { get; set; }

        public String TrxSource { get; set; }

        public String UserId { get; set; }
    }
}