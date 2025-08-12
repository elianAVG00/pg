using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class RenditionData
    {
        public string EPC { get; set; }

        public long TransactionNumber { get; set; }

        public string GenericCode { get; set; }

        public string StatusCode { get; set; }
    }
}