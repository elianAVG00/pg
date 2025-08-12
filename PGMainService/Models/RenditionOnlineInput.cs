using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class RenditionOnlineInput
    {
        public long serviceid { get; set; }

        public int day { get; set; }

        public int month { get; set; }

        public int year { get; set; }
    }
}