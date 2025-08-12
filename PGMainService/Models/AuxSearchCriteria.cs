using System;
using System.Net.Http;

namespace PGMainService.Models
{
    public class AuxSearchCriteria
    {
        public DateTime SearchFrom = new DateTime();
        public DateTime SearchTo = new DateTime();
        public string MerchantId;
        public string TransactionId;
        public string CommerceItem;
        public bool FilterByCommerceItem = false;
        public bool FilterByTransactionId = false;
        public HttpResponseMessage DataValidationResponse;
    }

}