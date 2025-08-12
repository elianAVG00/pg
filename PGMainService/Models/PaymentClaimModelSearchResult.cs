using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{

    public class PaymentClaimModelSearchResult_Claimer {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string ClaimerMail { get; set; }
    }

    public class CommerceItemToRefundResult {
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public decimal RefundAmount { get; set; }
        public string Description { get; set; }
    }

    public class PaymentClaimModelSearchResult
    {
        public string PaymentClaimNumber { get; set; }
        public List<CommerceItemToRefundResult> ItemsInRefund { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string CurrencyCode { get; set; }
        public string RefundAmount { get; set; }
        public string Observation { get; set; }
        public string TransactionId { get; set; }
        public string MerchantId { get; set; }
        public string Product { get; set; }
        public PaymentClaimModelSearchResult_Claimer Claimer { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public ClaimAdditionalInfo AdditionalInfo { get; set; }
       
    }

    public class ClaimAdditionalInfo {

        public string authorizationCode { get; set; }
        public string operationNumber { get; set; }
        public string originalDateTime { get; set; }
    
    }

}