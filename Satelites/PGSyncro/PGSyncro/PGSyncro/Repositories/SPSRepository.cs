using Microsoft.Extensions.Logging;
using PGSyncro.Models;
using PGSyncro.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGSyncro.Repositories
{
    public class SPSRepository
    {
        private readonly PGConfiguration _pgConfiguration;
        private readonly ILogger<SPSRepository> _logger;
        public SPSRepository(PGConfiguration pgConfiguration, ILogger<SPSRepository> logger)
        {
            _pgConfiguration = pgConfiguration;
            _logger = logger;
        }
        public async Task<List<SPSNewValidator>> GetSPSValidatorsAsync()
        {
            var spsValidators = new List<SPSNewValidator>();

            string configBySetting = await _pgConfiguration.GetConfigBySettingAsync("SPSAPIMethods");

            if (configBySetting == null)
            {
                _logger.LogWarning("NO SE ENCONTRARON CONFIGURACIONES DE SPS");
                await _pgConfiguration.InsertLogAsync("Main", "No existen servicios de nuevo DECIDIR configurados correctamente");

                return spsValidators;
            }

            List<int> serviceIds = configBySetting.Split(',').Select(int.Parse).ToList();

            _logger.LogInformation("SE ENCONTRARON CONFIGURACIONES DE SPS: {Count}", serviceIds.Count);

            foreach (int serviceId in serviceIds)
            {
                var spsNewValidator = new SPSNewValidator()
                {
                    ServiceID = serviceId,
                    publicAUTHkey = await _pgConfiguration.GetConfigBySettingAsync("ApiKeyPublicServiceId_" + serviceId.ToString())
                };
                spsValidators.Add(spsNewValidator);
            }
            return spsValidators;
        }
    }
}