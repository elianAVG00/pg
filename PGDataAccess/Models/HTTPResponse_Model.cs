using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Runtime.Serialization;
namespace PGDataAccess.Models
{
    [DataContract]
    public class HTTPResponse_Model
    {
        [DataMember]
        public int HTTPStatusCode { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string PG_Code { get; set; }
        [DataMember]
        public int StatusCodeId { get; set; }
    }
}