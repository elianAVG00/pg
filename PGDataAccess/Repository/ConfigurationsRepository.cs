using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataAccess.Models;
using PGDataAccess.EF;
using PGDataAccess.Mappers;
using PGDataAccess.Tools;

namespace PGDataAccess.Repository
{
    public class ConfigurationsRepository
    {
        /// <summary>
        /// Obtiene validador a utilizar segun configuracion
        /// </summary>
        public static ValidatorModel GetValidatorFromConfiguration(int serviceId, int channelId, int productId)
        {
            using (var context = new PGDataEntities())
            {
                Configurations[] configuration = (
                    from conf in context.Configurations
                    where
                    conf.ServiceId == serviceId &&
                    conf.ChannelId == channelId &&
                    conf.ProductId == productId &&
                    conf.IsActive &&
                    conf.Services.IsActive &&
                    conf.Products.IsActive &&
                    conf.Channels.IsActive
                    select conf
                    ).ToArray();

                if (configuration.Length == 1)
                {
                    int validatorid = configuration.FirstOrDefault().ValidatorId;
                    Validators validator = context.Validators.FirstOrDefault(v => v.ValidatorId == validatorid);
                    if (validator != null)
                    {
                        return Mapper.Validator_EFToModel(validator);
                    }
                    else
                    {
                        LogRepository.InsertLogCommon(LogTypeModel.Warning, String.Format("No se encontró validador para la configuración: {0} configuraciones para los parámetros: Servicio {1} , Producto {2} , Canal {3}, Validador {4}", 
                            configuration.Length, serviceId, productId, channelId, configuration.FirstOrDefault().ValidatorId));
                        return null;
                    }
                }
                else
                {
                    LogRepository.InsertLogCommon(LogTypeModel.Warning, String.Format("Se encontraron: {0} configuraciones para los parámetros: Servicio {1} , Producto {2} , Canal {3}", configuration.Length, serviceId, productId, channelId));
                    return null;
                }

            }
        }

        public static string GetUniqueCode(int serviceId, int channelId, int productId)
        {
            using (var context = new PGDataEntities())
            {
                List<Configurations> configurationList = context.Configurations.Where(c => c.ServiceId == @serviceId && c.ChannelId == channelId && c.ProductId == productId && c.IsActive).ToList();
                if (configurationList.Count == 1)
                {
                    return configurationList[0].UniqueCode;
                }
                else
                    LogRepository.InsertLogCommon(LogTypeModel.Error, string.Format("Configuration error for service - Active records for: {0}, channel: {1}, product: {2} - Total count: {3} records", serviceId, channelId, productId, configurationList.Count));
                return null;

            }
        }
    }
}