using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class RePrintModel
    {
        public long TransactionNumber { get; set; }

        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public string ElectronicPaymentCode { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public string CreditCardDescription { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
        [DataMember]
        public string Fees { get; set; }
        [DataMember]
        public string CreditCardNumber { get; set; }
        [DataMember]
        public string SecurityCode { get; set; }
        [DataMember]
        public string TicketNumber { get; set; }
        [DataMember]
        public string CardNbrLfd { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string BarCode { get; set; }
        [DataMember]
        public int? ExternalId { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public int? TerminalId { get; set; }

    }
}