using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class DiagnosticResults
    {
        public long TransactionsToSync { get; set; }

        public long NPS_Transactions { get; set; }

        public long SPS_New_Transactions { get; set; }

        public long SPS_Common_Transactions { get; set; }

        public long Transactions_ERROR { get; set; }
        public long Transactions_OK { get; set; }
        public long Transactions_NA { get; set; }
        public long Transactions_NO { get; set; }
        public long WithReponse { get; set; }


        public long WithoutResponse { get; set; }


        public long WithError { get; set; }

        public long WithTransaction { get; set; }

        public long WithoutTransaction { get; set; }

    }



    public class InvalidCodes
    { 
        public int ValidatorId { get; set; }
        public string OriginalCode { get; set; }
        public string ModuleType { get; set; }

    }
}
