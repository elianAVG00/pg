using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class HTTPResponseModel
    {
        public int HTTPStatusCode { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string PG_Code { get; set; }
        public int StatusCodeId { get; set; }
    }
    public class HTTPResponseQueryModel
    {
        public string pgcode { get; set; }
        public string lang { get; set; }
    }
}