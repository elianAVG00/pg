using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class TransactionCompletedInfo
    {


        public long TransactionNumber { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string InternalNbr { get; set; }

        [DataMember]
        public string Channel { get; set; }

        [DataMember]
        public ClientModel Client { get; set; }

        [DataMember]
        public string SalePoint { get; set; }

        [DataMember]
        public ServiceModel Service { get; set; }

        [DataMember]
        public ProductModel Product { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Validator { get; set; }

        [DataMember]
        public string WebSvcMethod { get; set; }

        [DataMember]
        public string TransactionId { get; set; }

        [DataMember]
        public string ValidatorTransactionId { get; set; }

        [DataMember]
        public string MerchantId { get; set; }

        [DataMember]
        public string ElectronicPaymentCode { get; set; }

        [DataMember]
        public string CurrencyCode { get; set; }

        [DataMember]
        public string TrxCurrencyCode { get; set; }

        [DataMember]
        public decimal TrxAmount { get; set; }

        [DataMember]
        public decimal ConvertionRate { get; set; }

        [DataMember]
        public IEnumerable<CommerceItemModel> CommerceItems { get; set; }

        [DataMember]
        public IEnumerable<CommerceItemModel> CommerceItems_Annulled { get; set; }

        [DataMember]
        public string AppVersion { get; set; }

        [DataMember]
        public string TransactionCustomerMail { get; set; }

        [DataMember]
        public string UniqueCode { get; set; }

        [DataMember]
        public int? ExternalApp { get; set; }

        [DataMember]
        public string BarCode { get; set; }

        [DataMember]
        public string CallbackUrl { get; set; }

        [DataMember]
        public bool? IsEPCValidated { get; set; }

        [DataMember]
        public string EPCValidateURL { get; set; }

        [DataMember]
        public Nullable<int> Payments { get; set; }
        
        [DataMember]
        public string Currency { get; set; }
        
        [DataMember]
        public string Country { get; set; }
        
        [DataMember]
        public string AuthorizationCode { get; set; }
        
        [DataMember]
        public string CardMask { get; set; }
        
        [DataMember]
        public string CardNbrLfd { get; set; }
        
        [DataMember]
        public string CardHolder { get; set; }
        
        [DataMember]
        public string TicketNumber { get; set; }
        
        [DataMember]
        public string PaymentCustomerEmail { get; set; }
        
        [DataMember]
        public string CreatedBy { get; set; }
        
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        
        [DataMember]
        public string CustomerDocType { get; set; }
        
        [DataMember]
        public string CustomerDocNumber { get; set; }

        [DataMember]
        public IEnumerable<PaymentClaimModel> PaymentClaims { get; set; }

        [DataMember]
        public DateTime PayDate { get; set; }

        [DataMember]
        public StatusCompletedInfo TransactionCompletedStatus { get; set; }

    }
}