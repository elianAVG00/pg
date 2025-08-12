using PGPluginSPS.PGDataAccess;
using System;

namespace PGPluginSPS.Models
{

    public enum LogTransactionType
    {
        Validator,
        TransactionIdPK,
        TransactionNumber,
    }

    public class LogModel
    {
        public LogTypeModel Type { get; set; }

        public string Module { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public long? Transaction { get; set; }
    }
}
