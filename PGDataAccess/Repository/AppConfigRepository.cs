using PGDataAccess.EF;
using PGDataAccess.Models;
using PGDataAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PGDataAccess.Repository
{
    public class AppConfigRepository
    {
        #region App Config methods

        public static List<AppConfigModel> GetAllConfigs()
        {
            List<AppConfigModel> configsListToReturn = new List<AppConfigModel>();
            using (var context = new PGDataEntities())
            {
                configsListToReturn = (from appconfig in context.AppConfig
                                       where appconfig.IsActive 
                                       select new AppConfigModel
                                       {
                                           Description = appconfig.Description,
                                           Setting = appconfig.Setting,
                                           Value = appconfig.Value,
                                           ConfigType = appconfig.ConfigType,
                                           ViewOrderInWebAdmin = appconfig.ViewOrderInWebAdmin,
                                           ViewSpecialBackgroundColor = appconfig.ViewSpecialBackgroundColor
                                       }).ToList();
            }
            return configsListToReturn;
        }

        public static void SetAppConfiguration(string setting, string newvalue)
        {

            using (var context = new PGDataEntities())
            {
                AppConfig appConfigToUpdate = new AppConfig();
                appConfigToUpdate = context.AppConfig.FirstOrDefault(appc => appc.Setting == setting);
                appConfigToUpdate.Value = newvalue;
                context.SaveChanges();
            }

        }

        public static string GetAppVersion(string appVersion) { 
           string versionToReturn = "unknown";        
            using (var context = new PGDataEntities())
            {
                VersionLog versionToMap = new VersionLog();
                versionToMap = context.VersionLog.FirstOrDefault(v => v.Type == appVersion);
                versionToReturn = versionToMap.Major + "." + versionToMap.Minor + "." + versionToMap.Maintenance + "." + versionToMap.Build;
            }
            return versionToReturn;
        }

        public static string GetAppConfiguration(string setting)
        {
            string appConfigToReturn = "";
            using (var context = new PGDataEntities())
            {
                AppConfig appConfigToSelect = new AppConfig();
                appConfigToSelect = context.AppConfig.FirstOrDefault(appc => appc.Setting == setting && appc.IsActive);
                appConfigToReturn = appConfigToSelect.Value;
                if (appConfigToReturn == null)
                {
                    appConfigToReturn = "";
                }
            }
            return appConfigToReturn;
        }

        #endregion

        #region Status Page Related Methods

        public static bool IsPaymentOffline()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    return Convert.ToBoolean((from appconfig in context.AppConfig
                                              where appconfig.Setting == "IsPaymentOffline"
                                              select appconfig.Value).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return false;
            }
        }

        public static bool IsServiceOffline()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    return Convert.ToBoolean((from appconfig in context.AppConfig
                                              where appconfig.Setting == "IsServiceOffline"
                                              select appconfig.Value).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return false;
            }
        }

        public static bool IsJobConsoleTaskOffline(string task)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    return Convert.ToBoolean((from appconfig in context.AppConfig
                                              where appconfig.Setting == "IsJobTask_" + task + "_Offline"
                                              select appconfig.Value).FirstOrDefault());

                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return false;
            }
        }

        public static string TestDBUpdate()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    AppConfig appConfigToUpdate = new AppConfig();
                    appConfigToUpdate = context.AppConfig.FirstOrDefault(appc => appc.Setting == "TEMPORALappStatusChecker");
                    appConfigToUpdate.Value = "testing TEMPstatus...UPDATE";
                    context.SaveChanges();
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return "ERROR";
            }
        }

        public static string TestDBInsert()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    AppConfig appConfigToInsert = new AppConfig();
                    appConfigToInsert.ConfigType = "TEMP";
                    appConfigToInsert.Description = "Temporal Record for status check";
                    appConfigToInsert.IsActive = true;
                    appConfigToInsert.ViewOrderInWebAdmin = 0;
                    appConfigToInsert.ViewSpecialBackgroundColor = "";
                    appConfigToInsert.Setting = "TEMPORALappStatusChecker";
                    appConfigToInsert.Value = "testing TEMPstatus...";
                    context.AppConfig.Add(appConfigToInsert);
                    context.SaveChanges();
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return "ERROR";
            }
        }

        public static string TestDBSelect()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    AppConfig appConfigToSelect = new AppConfig();
                    appConfigToSelect = context.AppConfig.FirstOrDefault(appc => appc.Setting == "appStatusChecker" && appc.Value.ToLower() == "testing status...");

                    if (appConfigToSelect != null)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "NOSELECT";
                    }
                
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return "ERROR";
            }
        }

        public static string TestDBDelete()
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    AppConfig appConfigToRemove = new AppConfig();
                    appConfigToRemove = context.AppConfig.FirstOrDefault(appc => appc.Setting == "TEMPORALappStatusChecker");
                    if (appConfigToRemove != null)
                    {
                        context.AppConfig.Remove(appConfigToRemove);
                        context.SaveChanges();
                    }
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return "ERROR";
            }
        }

        public static AppInfo GetStatus()
        {

            AppInfo appInfoToReturn = new AppInfo();
            appInfoToReturn.DataBaseName = "Can not be checked";
            appInfoToReturn.DataBaseSource = "Can not be checked";
            appInfoToReturn.DASServerName = "Information is not available";
            appInfoToReturn.DASPhysicalPath = "Information is not available";
            appInfoToReturn.DASVirtualPath = "Information is not available";
            appInfoToReturn.AppVersion = "No disponible";
            appInfoToReturn.DBVersion = "No disponible";

            //Se realizan Tries differentes para agrupar posibles errores y que consiga la mayor cantidad de variables sin saltar al catch.
            try
            {
                appInfoToReturn.DASServerName = System.Web.Hosting.HostingEnvironment.SiteName;
                appInfoToReturn.DASPhysicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                appInfoToReturn.DASVirtualPath = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
            }

            try
            {
                appInfoToReturn.AppVersion = "Current Version: " + GetAppVersion("PGMS") + " - LegacyVersion: " + GetAppVersion("PGv2");
                appInfoToReturn.DBVersion = GetAppVersion("DB");

            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
              }

            try
            {


                appInfoToReturn.logInfo = System.Configuration.ConfigurationManager.AppSettings["logInfo"];
                appInfoToReturn.logError = System.Configuration.ConfigurationManager.AppSettings["logError"];
                appInfoToReturn.logDebug = System.Configuration.ConfigurationManager.AppSettings["logDebug"];
                appInfoToReturn.logWarning = System.Configuration.ConfigurationManager.AppSettings["logWarning"];
                appInfoToReturn.logOnDataBase = System.Configuration.ConfigurationManager.AppSettings["logOnDataBase"];

                // Clean null values and to lower
                appInfoToReturn.logInfo = (appInfoToReturn.logInfo == null) ? "Web Config App Key can not be read" : appInfoToReturn.logInfo.ToLower();
                appInfoToReturn.logError = (appInfoToReturn.logError == null) ? "Web Config App Key can not be read" : appInfoToReturn.logError.ToLower();
                appInfoToReturn.logDebug = (appInfoToReturn.logDebug == null) ? "Web Config App Key can not be read" : appInfoToReturn.logDebug.ToLower();
                appInfoToReturn.logWarning = (appInfoToReturn.logWarning == null) ? "Web Config App Key can not be read" : appInfoToReturn.logWarning.ToLower();
                appInfoToReturn.logOnDataBase = (appInfoToReturn.logOnDataBase == null) ? "Web Config App Key can not be read" : appInfoToReturn.logOnDataBase.ToLower();
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
              }
            try
            {
                using (var context = new PGDataEntities())
                {
                    appInfoToReturn.DataBaseName = context.Database.Connection.Database;
                    appInfoToReturn.DataBaseSource = context.Database.Connection.DataSource;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
              }

            return appInfoToReturn;
        }

        public static string TestSMTP()
        {

            try
            {
                MailerModel mailToSend = new MailerModel();
                mailToSend.To = new List<string>();
                mailToSend.To.Add("test@provincianet.com.ar");
                mailToSend.FromDisplayName = "Test PG Status";
                mailToSend.FromAddress = "test@provincianet.com.ar";
                mailToSend.Subject = "Test";
                mailToSend.Body = "";

                MailerService.SendEmail(mailToSend);

                return "OK";

            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        public static List<string> GetLastPayment()
        {
            List<string> lastTransaction = new List<string>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    var lastPay = (from trans in context.Transactions
                                    select (DateTime?)trans.CreatedOn).Max();

                    var lastPayed = (from trans in context.Transactions
                                      join ts in context.TransactionStatus
                                      on trans.Id equals ts.TransactionsId
                                      where ts.StatusCodeId == 5
                                      select (DateTime?)trans.CreatedOn).Max();

                    var lastClaim = context.PaymentClaim.Max(t => (DateTime?)t.CreatedOn);

                    var lastClaimApproved = (from trans in context.Transactions
                                              join ts in context.TransactionStatus
                                              on trans.Id equals ts.TransactionsId
                                              where ts.StatusCodeId == 36
                                              select (DateTime?)trans.CreatedOn).Max();

                    var lastNPS = (from trans in context.Transactions
                                      join tai in context.TransactionAdditionalInfo
                                        on trans.Id equals tai.TransactionIdPK
                                      join ts in context.TransactionStatus
                                        on trans.Id equals ts.TransactionsId
                                      where ts.StatusCodeId == 5 && tai.ValidatorId == 1
                                      select new { trans.TransactionId, tai.CreatedOn, tai.TransactionNumber }).OrderByDescending(t => t.CreatedOn).FirstOrDefault();

                    var lastSPS = (from trans in context.Transactions
                                    join tai in context.TransactionAdditionalInfo
                                    on trans.Id equals tai.TransactionIdPK
                                    join ts in context.TransactionStatus
                                    on trans.Id equals ts.TransactionsId
                                    where ts.StatusCodeId == 5 && tai.ValidatorId == 3
                                    select new { trans.TransactionId, tai.CreatedOn, tai.TransactionNumber }).OrderByDescending(t => t.CreatedOn).FirstOrDefault();

                    lastTransaction.Add(lastPay.ToString());
                    lastTransaction.Add(lastPayed.ToString());
                    lastTransaction.Add(lastPayed.ToString());
                    lastTransaction.Add(lastClaimApproved.ToString());
                    lastTransaction.Add("Transaction ID: " + lastNPS.TransactionNumber + " (Validator ID: " + lastNPS.TransactionId + ") on: " + lastNPS.CreatedOn);
                    lastTransaction.Add("Transaction ID: " + lastSPS.TransactionNumber + " (Validator ID: " + lastSPS.TransactionId + ") on: " + lastSPS.CreatedOn);
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                lastTransaction = null;
            }
            return lastTransaction;
        }

        //Save the TFS and new build
        public static string SaveNewBuild(string TFSChangeSet) {

            try
            {
                using (var context = new PGDataEntities())
                {
                    VersionLog versionToUpdate = context.VersionLog.FirstOrDefault(vl => vl.Type == "PGMS");
                    versionToUpdate.Build = versionToUpdate.Build + 1;
                    versionToUpdate.Revision = TFSChangeSet;
                    context.SaveChanges();
                    return "Version Updated";
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
            }
            return "Version Not Updated";
        
        }
        #endregion
    }
}