using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PGDataAccess.Models
{
    [DataContract]
    public class StatusMessageModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdLanguage { get; set; }
        [DataMember]
        public int StatusCodeId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CreatedOn { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedOn { get; set; }

        [DataMember]
        public virtual LanguageModel Language { get; set; }
        [DataMember]
        public virtual StatusCodeModel StatusCode { get; set; }
    }
}