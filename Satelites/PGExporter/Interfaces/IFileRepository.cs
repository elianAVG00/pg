using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface IFileRepository
    {
        string AddProcessIdToFolder(long id, string type);

        Task<bool> CreateCentralizerFile();

        Task<bool> CreateConciliationFile();

        Task<bool> CreateRenditionFile(long serviceId, string serviceName);

        Task<bool> CreateRenditionFiles();

        Task<string> Model814ToLine(CentralizerReportModel LineaAEscribir);

        Task<string> ModelConciliationToLine(ConciliationReportModel LineaAEscribir);

        Task<string> ModelRenditionToLine(RenditionReportModel LineaAEscribir);

        string SetFolder();
    }
}
