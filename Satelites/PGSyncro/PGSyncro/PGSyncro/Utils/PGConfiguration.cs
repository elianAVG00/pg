using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PGSyncro.EFData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGSyncro.Utils
{
    public class PGConfiguration
    {
        private readonly PaymentGatewayEntities _context;
        private readonly ILogger<PGConfiguration> _logger;

        public PGConfiguration(PaymentGatewayEntities context, ILogger<PGConfiguration> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InsertLogAsync(string thread, string message, Exception exception = null)
        {
            AppConfig.witherror = true;

            var newLog = new Log
            {
                Date = DateTime.Now,
                Exception = exception != null ? $"{exception.Message}|{exception.InnerException?.Message}" : null,
                Message = message,
                Thread = thread,
                Type = "Syncro"
            };

            _context.Logs.Add(newLog);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetConfigBySettingAsync(string setting)
        {
            var config = await _context.AppConfigs
                                  .Where(c => c.IsActive && c.Setting == setting)
                                  .FirstOrDefaultAsync();
            return config?.Value;
        }

        public async Task<List<ValidatorServiceConfig>> GetValidatorsSecurityConfigurationAsync()
        {
            return await _context.ValidatorServiceConfigs.ToListAsync();
        }
        public async Task<List<int>> GetServicesWithConciliationAsync()
        {
            try
            {
                return await _context.ServicesConfigs
                                  .Where(s => s.RptToConciliation)
                                  .Select(s => s.ServiceId)
                                  .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener servicios con conciliación. Verifica que la tabla 'ServicesConfigs' y la columna 'RptToConciliation' existen.");
                return new List<int>();
            }
        }
    }
}