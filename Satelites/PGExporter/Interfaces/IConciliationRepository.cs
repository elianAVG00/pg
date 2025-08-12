using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface IConciliationRepository
    {
        Task<List<ConciliationReportModel>> GetConciliationData(long id);

        Task<long> GetConciliationReportId();

        ConciliationReportModel MapConciliationDataToReport(ConciliationDataModel model);
    }
}
