using PGMainService.PGDataAccess;

namespace PGMainService.Models
{
    internal class TransactionCompletedInfoWithPk
    {
        public long TransactionIdPk { get; set; }

        public TransactionCompletedInfo TransactionCompletedInfo { get; set; }
    }

}