using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface ICentralizerRepository
    {
        Task<List<CentralizerReportModel>> GetCentralizerData(long id);

        Task<long> GetCentralizerReportId();

        CentralizerReportModel MapCentralizerDataToReport(CentralizerDataModel model);
    }
}
