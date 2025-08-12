using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class PaymentClaimModel
    {

        
        //Added for v4.1+
        public long TransactionNumber { get; set; }

        //FGS-TODO- Dejar solo la varcible CommerceItems
        public List<long> CommerceItemsId { get; set; }

        [DataMember]
        public IEnumerable<CommerceItemModel> CommerceItems { get; set; }

        //Migrated From ClaimResult SP

        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string MerchantId { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public int ProductId { get; set; }

        //End of SP Migration

        [DataMember]
        public long PaymentClaimId { get; set; }
        [DataMember]
        public long PaymentClaimNumber { get; set; }
        [DataMember]
        public Nullable<int> ClaimerId { get; set; }
        [DataMember]
        public Nullable<long> TransactionIdPK { get; set; }
        [DataMember]
        public Nullable<int> CurrencyId { get; set; }
        [DataMember]
        public Nullable<decimal> Amount { get; set; }
        [DataMember]
        public string Observation { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<DateTime> UpdatedOn { get; set; }

        [DataMember]
        public virtual ICollection<AnnulmentRequestModel> AnnulmentRequest { get; set; }
        [DataMember]
        public virtual CurrencyModel Currency { get; set; }
        [DataMember]
        public virtual string ActualStateCode { get; set; }
        [DataMember]
        public virtual ClaimerModel Claimer { get; set; }
        [DataMember]
        public virtual TransactionModel Transaction { get; set; }
        [DataMember]
        public virtual string CurrencyDescription { get; set; }
        [DataMember]
        public StatusCompletedInfo PaymentClaimCompletedStatus { get; set; }



    }
}