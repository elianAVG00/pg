using PGMainService.PGDataAccess;
using System.Collections.Generic;

namespace PGMainService.Models
{
    public class PaymentClaimInputModel
    {
        public string TransactionId { get; set; }



        //Added to 4.1
        public List<CommerceItemModel> CommerceItems { get; set; }
        public decimal TotalAmountToAnnulment { get; set; }
        //End of 4.1


        public string MerchantId { get; set; }

        public int ProductId { get; set; }

        public string Observation { get; set; }

        public int CurrencyId { get; set; }

        public long IdTransaction { get; set; }

        public ClaimerModel ClaimerModel { get; set; }
    }
}