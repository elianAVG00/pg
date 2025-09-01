using PGSyncro.EFData;
using System.Collections.Generic;

namespace PGSyncro.Models
{

    public class IncompleteTransactionsModel
    {
        public List<GetTransactionsToSync_Result> Transactions { get; set; }

        public bool IsCompleted { get; set; }
    }
}