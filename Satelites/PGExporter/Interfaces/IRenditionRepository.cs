using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface IRenditionRepository
    {
        Task<List<RenditionReportModel>> GetRenditionData(long id);

        Task<long> GetRenditionReportId(long ServiceID);

        RenditionReportModel MapRenditionDataToReport(RenditionDataModel model);
    }
}
