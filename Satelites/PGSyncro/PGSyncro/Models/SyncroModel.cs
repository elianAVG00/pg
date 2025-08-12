using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Models
{
    public class SyncroStats {
        public long TotalTransactionsProcessed { get;  set; }
        public long TotalTransactionsClosed { get; set; }
        public long TotalTransactionsWithError { get; set; }
        public long TotalTransactionsNotClosed { get; set; }
        public long TotalTransactionsClosedOK { get; set; }
        public long TotalTransactionsClosedNO { get; set; }
        public long TotalTransactionsClosedERROR { get; set; }
        public long TotalTransactionsClosedNA { get; set; }
        public long TotalTransactionsClosedNAByTIMEOUT { get; set; }

    }
    public class SyncroProcess { 
        public SyncroSaveStatus Status { get; set; }
        public SyncroModel Communication { get; set; }
    }
    public class SyncroSaveStatus { 

        public bool ItWasClose { get; set; }
        public bool StatusSaved { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class SyncroModel
    {
        public string ServiceType { get; set; }
        public bool HasResponse { get; set; }

        public bool HasError { get; set; }

        public bool HasTransaction { get; set; }

        public string ErrorInQuery { get; set; }

        public string RequestLog { get; set; }

        public string ResponseLog { get; set; }
    }

}
