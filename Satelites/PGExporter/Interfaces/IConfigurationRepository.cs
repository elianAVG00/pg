using PGExporter.EF;
using PGExporter.Models;

namespace PGExporter.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<string> GetConfiguration(string setting);

        Task<List<ProductCentralizer>> GetProductsCentralizer();

        Task<List<Channels>> GetChannels();

        Task<List<Products>> GetProducts();

        Task<List<ServicesConfig>> GetServicesConfigurations();

        Task<List<Configurations>> GetConfigurations();

        Task<List<ServiceModel>> GetServicesToRendition();

        Task<ReGen?> GetProcess(long id);
    }
}
