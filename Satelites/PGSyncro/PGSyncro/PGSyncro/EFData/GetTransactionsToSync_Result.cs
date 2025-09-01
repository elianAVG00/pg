using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.EFData
{
    using System;

    public partial class GetTransactionsToSync_Result
    {
        public int TypeRow { get; set; }
        public Nullable<long> MonitorSyncroProcessRecordsId { get; set; }
        public Nullable<long> MonitorSyncroProcessId { get; set; }
        public long TransactionIdPK { get; set; }
        public int ValidatorId { get; set; }
        public string UniqueCode { get; set; }
        public long TransactionNumber { get; set; }
        public string TransactionId { get; set; }
        public int ServiceId { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
