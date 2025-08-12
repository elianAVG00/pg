using PGExporter.EF;
using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface IProcessRepository
    {
        string? CheckBatchNbr(string? fromTAI, string? fromTRI);

        Task RepairTAITransactions(
          List<TransactionAdditionalInfo> transactionsToRepair);

        Task<bool> RollBackProcess();

        Task MarkAsInformed(IEnumerable<long> data);

        Task RemoveOriginalMark(int type, IEnumerable<long> ids);

        Task MarkAsComplete(IEnumerable<long> data);

        Task CloseProcess(
          long monitorid,
          string filename,
          List<Records> registros,
          bool withError = false,
          List<string> errors = null);
    }
}
