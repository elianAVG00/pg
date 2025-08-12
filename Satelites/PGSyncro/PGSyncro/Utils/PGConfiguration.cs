using PGSyncro.EFData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Utils
{
    public static class PGConfiguration
    {

        public static void InsertLog(string thread, string message, Exception exception = null) {
            Program.witherror = true;

                using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
                {
                Log newlog = new Log
                {
                    Date = DateTime.Now,
                    Exception = exception.Message + "|" + exception.InnerException?.Message,
                    Message = message,
                    Thread = thread,
                    Type = "Syncro"
                };
                datacontext.Logs.Add(newlog);
                }


            
        }

        public static string GetConfigBySetting(string setting) {

            using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
            {
                return datacontext.AppConfigs.Where(c => c.IsActive && c.Setting == setting).FirstOrDefault()?.Value;
            }


            }
        public static List<ValidatorServiceConfig> GetValidatorsSecurityConfiguration()
        {

            using (PaymentGatewayEntities datacontext = new PaymentGatewayEntities())
            {
                return datacontext.ValidatorServiceConfigs.ToList();
            }


        }
    }
}
