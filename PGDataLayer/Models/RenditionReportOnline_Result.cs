using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class RenditionReportOnline_Result
    {
        public string ElectronicPaymentCode { get; set; }

        public long TrxID { get; set; }

        public string GenericCode { get; set; }

        public string StatusCode { get; set; }
    }
}