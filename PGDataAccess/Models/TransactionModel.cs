using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PGDataAccess.Models
{
    [DataContract]
    public class TransactionValidatorNumber
    {
        [DataMember]
        public string ValidatorTransactionId { get; set; }

        [DataMember]
        public long TransactionNumber { get; set; }

        [DataMember]
        public string Validator { get; set; }
     
        [DataMember]
        public long TransactionIdPK { get; set; }

    }

    [DataContract]
    public class TransactionModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string InternalNbr { get; set; }

        [DataMember]
        public string Channel { get; set; }

        [DataMember]
        public string Client { get; set; }

        [DataMember]
        public string SalePoint { get; set; }

        [DataMember]
        public string Service { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Validator { get; set; }

        [DataMember]
        public string WebSvcMethod { get; set; }

        [DataMember]
        public string ValidatorTransactionId { get; set; }

        [DataMember]
        public string JSonObject { get; set; }


        [DataMember]
        public int SettingId { get; set; }

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



        //Definitions about TransactionAdditionalInfo
        [DataMember]
        public string ProdVersionUsed { get; set; }


        [DataMember]
        public string AppVersion { get; set; }

        [DataMember]
        public string CustomerMail { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int ServiceId { get; set; }

        [DataMember]
        public int ExternalAppId { get; set; }

        [DataMember]
        public int ValidatorId { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int ChannelId { get; set; }

        [DataMember]
        public string UniqueCode { get; set; }


        [DataMember]
        public int LanguageId { get; set; }

        [DataMember]
        public int ExternalApp { get; set; }

        [DataMember]
        public int? Payments { get; set; }

        [DataMember]
        public string BarCode { get; set; }

        [DataMember]
        public string CallbackUrl { get; set; }

        [DataMember]
        public bool? IsEPCValidated { get; set; }

        [DataMember]
        public string EPCValidateURL { get; set; }


        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public System.DateTime CreatedOn { get; set; }

        [DataMember]
        public decimal CurrentAmount { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public bool IsCommerceItemValidated { get; set; }

        [DataMember]
        public bool IsSimulation { get; set; }

        [DataMember]
        public string TransactionId { get; set; }


    }

    [DataContract]
    public class CheckTransaction
    {
        [DataMember]
        public int CheckResult { get; set; }
        [DataMember]
        public string ValidatorShortName { get; set; }
    }
}