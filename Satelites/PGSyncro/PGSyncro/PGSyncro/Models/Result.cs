
using System.Collections.Generic;

namespace PGSyncro.Models
{
    public class Result
    {
        public int id { get; set; }

        public string site_transaction_id { get; set; }

        public int payment_method_id { get; set; }

        public string card_brand { get; set; }

        public long amount { get; set; }

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