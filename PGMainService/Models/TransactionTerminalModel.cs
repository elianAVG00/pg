using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class TransactionTerminalModel
    {
        public string psp_TransactionId { get; set; }
        public string psp_MerchTxRef { get; set; }
    }

    public class SyncResponseModel
    {
        public List<TransactionValidatorNumber> Synchronized { get; set; }
        public List<TransactionValidatorNumber> NotSynchronized { get; set; }
    }
}