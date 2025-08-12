using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS_ServiceReference.Models
{
    public class SPSPaymentResponseModel
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public List<Result> results { get; set; }
        public bool hasMore { get; set; }
    }

    public class SPSPaymentResponseModelWithLog {
        public string LogResponse { get; set; }
        public string LogRequest { get; set; }
        public SPSPaymentResponseModel respuesta {get; set;}
    }

    public class StatusDetails
    {
        public string ticket { get; set; }
        public string card_authorization_code { get; set; }
        public string address_validation_code { get; set; }
        public object error { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Identification
    {
        public string type { get; set; }
        public string number { get; set; }
    }

    public class CardHolder
    {
        public Identification identification { get; set; }
        public string name { get; set; }
    }

    public class CardData
    {
        public string card_number { get; set; }
        public CardHolder card_holder { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string site_transaction_id { get; set; }
        public int payment_method_id { get; set; }
        public string card_brand { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public StatusDetails status_details { get; set; }
        public string date { get; set; }
        public Customer customer { get; set; }
        public string bin { get; set; }
        public int installments { get; set; }
        public object first_installment_expiration_date { get; set; }
        public string payment_type { get; set; }
        public List<object> sub_payments { get; set; }
        public string site_id { get; set; }
        public object fraud_detection { get; set; }
        public object aggregate_data { get; set; }
        public object establishment_name { get; set; }
        public object spv { get; set; }
        public object confirmed { get; set; }
        public object pan { get; set; }
        public object customer_token { get; set; }
        public string emv_issuer_data { get; set; }
        public string token { get; set; }
        public CardData card_data { get; set; }
    }

}
