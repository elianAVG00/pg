namespace PGSyncro.Models
{
    public class SyncroStats
    {
        public long TotalTransactionsProcessed { get; set; }

        public long TotalTransactionsClosed { get; set; }

        public long TotalTransactionsWithError { get; set; }

        public long TotalTransactionsNotClosed { get; set; }

        public long TotalTransactionsClosedOK { get; set; }

        public long TotalTransactionsClosedNO { get; set; }

        public long TotalTransactionsClosedERROR { get; set; }

        public long TotalTransactionsClosedNA { get; set; }

        public long TotalTransactionsClosedNAByTIMEOUT { get; set; }
    }
}