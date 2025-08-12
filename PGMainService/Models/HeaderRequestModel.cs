using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class HeaderRequestModel
    {
        public string Language { get; set; }
        public string User { get; set; }
        public bool IsAuthorized { get; set; }
        public bool HasAuthentication { get; set; }
    }
}