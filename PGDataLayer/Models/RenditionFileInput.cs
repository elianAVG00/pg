using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class RenditionFileInput
    {
        public long serviceid { get; set; }

        public int day { get; set; }

        public int month { get; set; }

        public int year { get; set; }
    }
}