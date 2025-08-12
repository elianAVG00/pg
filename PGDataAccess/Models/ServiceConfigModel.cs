using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class ServiceConfigModel
    {
        [DataMember]
        public int ServiceConfigId { get; set; }
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public Nullable<int> ExternalId { get; set; }
        [DataMember]
        public Nullable<int> BranchId { get; set; }
        [DataMember]
        public Nullable<int> TerminalId { get; set; }
        [DataMember]
        public bool RptToRendition { get; set; }
        [DataMember]
        public bool RptToConciliation { get; set; }
        [DataMember]
        public bool RptToCentralizer { get; set; }
        [DataMember]
        public int RenditionType { get; set; }
        [DataMember]
        public string SenderMail { get; set; }
        [DataMember]
        public string SenderURL { get; set; }
        [DataMember]
        public bool IsCallbackPosted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }

        [DataMember]
        public bool ResponseTransactionNumber { get; set; }


        [DataMember]
        public bool IsCommerceItemValidated { get; set; }
        [DataMember]
        public bool IsPaymentSecured { get; set; }
        [DataMember]
        public string ReportsPath { get; set; }
    }
}