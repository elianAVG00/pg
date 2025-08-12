using System;
using System.Collections.Generic;
using System.Text;

namespace SPS_ServiceReference.Models
{
    public class SPSPaymentHashModelResponse {
            public string hash { get; set; }
        }

    public class SPSPaymentModel
    {
        public string cancel_url { get; set; }
        public string redirect_url { get; set; }
        public string public_apikey { get; set; }
        public SPSPaymentModelpayment payment { get; set; }
        public SPSPaymentModelcustomer customer { get; set; }
        public SPSPaymentModelsite site { get; set; }
    }
    public class SPSPaymentModelpayment
    {
        public long amount { get; set; }
        public string currency { get; set; }
        public int payment_method_id { get; set; }
        public int installments { get; set; }
        public string payment_type { get; set; }
        public List<object> sub_payments  { get;set;}
    }
    public class SPSPaymentModelcustomer
    {
        public string id { get; set; }
        public string email { get; set; }
        public string ip_address { get; set; }

    }
    public class SPSPaymentModelsite
    {
        public string transaction_id { get; set; }
        public SPSPaymentModeltemplate template { get; set; }
    }
    public class SPSPaymentModeltemplate
    {
        public int id { get; set; }
    }

}
