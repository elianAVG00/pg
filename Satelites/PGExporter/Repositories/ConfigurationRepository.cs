using Microsoft.EntityFrameworkCore;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;

namespace PGExporter.Repositories
{
    public class ConfigurationRepository(PaymentContext _context, ILogRepository _logrepository) : IConfigurationRepository
    {
        public async Task<string> GetConfiguration(string setting)
        {
            try
            {
                AppConfig flntempl = await _context.AppConfig.Where(c => c.Setting == setting).FirstOrDefaultAsync();
                return flntempl.Value;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetConfiguration", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<ReGen?> GetProcess(long id)
        {
            ReGen process;
            try
            {
                MonitorFilesReportProcess flntempl = await _context.MonitorFilesReportProcess.Where(c => c.MonitorFilesReportProcessId == id).FirstOrDefaultAsync();
                if (flntempl == null)
                    return null;

                List<MonitorFilesReportRecords> recs = await _context.MonitorFilesReportRecords.Where(c => c.MonitorFilesReportProcessId == id).ToListAsync();
                if (recs.Count > 0)
                    return new ReGen()
                    {
                        MonitorFilesReportProcess = flntempl,
                        MonitorFilesReportRecords = recs
                    };
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetProcess", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
            return null;
        }

        public async Task<List<ProductCentralizer>> GetProductsCentralizer()
        {
            try
            {
                List<ProductCentralizer> listAsync = await _context.ProductCentralizer.Where(c => c.IsActive).ToListAsync();
                return listAsync;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetProductsCentralizer", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<List<Channels>> GetChannels()
        {
            List<Channels> channels;
            try
            {
                List<Channels> listAsync = await _context.Channels.Where(c => c.IsActive).ToListAsync();
                return listAsync;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetChannels", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<List<Products>> GetProducts()
        {
            List<Products> products = new List<Products>();
            try
            {
                List<Products> listAsync = await _context.Products.Where(c => c.IsActive).ToListAsync();
                return listAsync;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetProducts", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
            return products;
        }

        public async Task<List<ServicesConfig>> GetServicesConfigurations()
        {
            List<ServicesConfig> servicesConfigurations = new List<ServicesConfig>();
            try
            {
                servicesConfigurations = await _context.ServicesConfig.Where(sc => sc.IsActive).ToListAsync();
                return servicesConfigurations;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetServicesConfigurations", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
            return servicesConfigurations;
        }

        public async Task<List<Configurations>> GetConfigurations()
        {
            List<Configurations> configurations = new List<Configurations>();
            try
            {
                configurations = await _context.Configurations.Where(cc => cc.IsActive).ToListAsync();
                return configurations;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetConfigurations", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
            return configurations;
        }

        public async Task<List<ServiceModel>> GetServicesToRendition()
        {
            List<ServiceModel> servicesToRendition = new List<ServiceModel>();
            try
            {
                servicesToRendition = await (
                                        from s in _context.Services
                                        join sc in _context.ServicesConfig
                                            on s.ServiceId equals sc.ServiceId
                                        where s.IsActive
                                              && sc.IsActive
                                              && sc.RptToRendition
                                        select new ServiceModel
                                        {
                                            ServiceId = s.ServiceId,

                                            // Mapea aquí el resto de propiedades de ServiceModel:(?
                                            // Ejemplo:
                                            Name = s.Name
                                        }
                                    ).ToListAsync();
                return servicesToRendition;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConfigurationRepository.GetServicesToRendition", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
            return servicesToRendition;
        }
    }
}
