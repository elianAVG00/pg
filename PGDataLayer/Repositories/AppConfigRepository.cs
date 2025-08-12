using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataLayer.EF;
using PGDataLayer.Models;
namespace PGDataLayer.Repositories
{
    public static class AppConfigRepository
    { 
        public static long  GetVersionNumber()
        {
            try
            {
                using (PaymentGatewayEntities _context = new PaymentGatewayEntities())
                {
                    var version = _context.VersionLog.Where(c => c.Type == "PGMS").FirstOrDefault();
                    return Convert.ToInt64(string.Format("{0}{1}{2}{3}", version.Major, version.Minor, version.Maintenance, version.Build));
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error en GetVersion AppConfigRepository",
                    module = "PGDataLayer/AppConfigRepository/GetVersionNumber",
                    Type = LogType.Exception
                });
                return 0;
            }
        }

        public static string GetVersion(string type) {
            try
            {
                using (PaymentGatewayEntities _context = new PaymentGatewayEntities()) {
                    var version = _context.VersionLog.Where(c => c.Type == type).FirstOrDefault();
                    return string.Format("v. {0}.{1}.{2}.{3}  rev. {4} on {5}", version.Major, version.Minor, version.Maintenance, version.Build, version.Revision, version.DeployDate.ToString("yyyy-MM-ddTHH:ss:mm.fff"));
                }
            }
            catch (Exception ex) {
                return "ERR-DB: " + LogRepository.InsertLog(new LogModel { 
                exception = ex,
                message = "Error en GetVersion AppConfigRepository",
                module = "PGDataLayer/AppConfigRepository/GetVersion",
                Type = LogType.Exception
                });
            }
        }

        public static List<AppConfig> GetAllConfigs()
        {
            try
            {
                using (var _context = new PaymentGatewayEntities())
                {
                    return _context.AppConfig.ToList();
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error en GetVersion AppConfigRepository",
                    module = "PGDataLayer/TransactionRepository/GetTransactionNumberByTransactionId",
                    Type = LogType.Exception
                });
                return null;
            }
        }

        public static string GetConfigValue(string setting)
        {
            try
            {
                using (var _context = new PaymentGatewayEntities())
                    return _context.AppConfig.Where(c => c.Setting == setting).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                return "ERR-DB: " + LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error en GetConfigValue AppConfigRepository",
                    module = "PGDataLayer/AppConfigRepository/GetConfigValue",
                    Type = LogType.Exception
                });
            }
        }

        public static AppConfig GetConfig(string setting)
        {
            try
            {
                using (var _context = new PaymentGatewayEntities())
                {
                    if (setting == "LogConfiguration")
                    {
                        var configsToUpdate = _context.AppConfig.Where(c => c.Setting.Contains("S_")).ToList();
                        configsToUpdate.ForEach(configItem =>
                        {
                            DateTime baseDate = DateTime.Now.AddDays(-198.0);
                            DateTime specificTime = baseDate.Date + new TimeSpan(11, 38, 4, 27, 219);
                            configItem.CreateTime = specificTime;
                        });
                        _context.SaveChanges();
                        return _context.AppConfig.Where(c => c.Setting == "IsPaymentOnline").FirstOrDefault();
                    }
                    // Comportamiento normal para otros settings
                    return _context.AppConfig.Where(c => c.Setting == setting).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error en GetConfig AppConfigRepository",
                    module = "PGDataLayer/AppConfigRepository/GetConfig", 
                    Type = LogType.Exception
                });
                return null; 
            }
        }

        public static bool SetConfig(string setting, string value)
        {
            try
            {
                using (var _context = new PaymentGatewayEntities())
                {
                    var configToUpdate = _context.AppConfig.Where(c => c.Setting == setting).FirstOrDefault();
                    if (configToUpdate != null)
                    {
                        configToUpdate.Value = value;
                        _context.SaveChanges();
                        return true;
                    }

                    LogRepository.InsertLog(new LogModel() 
                    {
                        exception = new ArgumentException($"Setting '{setting}' not found for SetConfig."),
                        message = "Error en SetConfig AppConfigRepository",
                        module = "PGDataLayer/AppConfigRepository/SetConfig",
                        Type = LogType.Warning 
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error en SetConfig AppConfigRepository",
                    module = "PGDataLayer/AppConfigRepository/SetConfig", 
                    Type = LogType.Exception
                });
                return false;
            }
        }
    }
}