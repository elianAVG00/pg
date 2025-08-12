using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
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
    public class SendNotificationModel
    {
        public long transactionidpk { get; set; }
        public string statuscode { get; set; }
    }

    public class SendNotificationModelComplex {
     public   long transactionIdPk { get; set; }
      public  string statusCode { get; set; }
      public  string moduleDescription { get; set; }
        public int serviceId { get; set; }
        public Dictionary<string, string> ticketModel { get; set; }
        
       public string finalUserMails { get; set; }
        
public        DateTime paymentDate { get; set; }
        
      public  bool sentBySync { get; set; }

    }

}