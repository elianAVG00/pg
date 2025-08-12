using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataAccess.Models;
using System.Runtime.Serialization;
namespace PGDataAccess.Models
{
    [DataContract]
    public enum LogTransactionType {
        [EnumMember]
        Validator,
        [EnumMember]
        TransactionIdPK,
        [EnumMember]
        TransactionNumber,

    }

    [DataContract]
    public class LogModel
    {
        [DataMember]
        public LogTypeModel LogType { get; set; }
        [DataMember]
        public string CallerMemberName { get; set; }
        [DataMember]
        public string CallerFilePath { get; set; }
        [DataMember]
        public int CallerLineNumber { get; set; }
        [DataMember]
        public string InnerException { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Exception { get; set; }
        [DataMember]
        public LogTransactionType? TransactionType { get; set; }
        [DataMember]
        public string TransactionNumber { get; set; }
        [DataMember]
        public string AppLayer { get; set; }
    }
}