using PGDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataLayer.EF;
using NLog;
using Newtonsoft.Json;

namespace PGDataLayer.Repositories
{
    public static class LogRepository
    {


        private static Logger nLogger = LogManager.GetCurrentClassLogger();
        private static string _applayer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        internal static void InsertLogException(LogType tipo, Exception ex, bool nulo, string method ="") {

            LogRepository.InsertLog(new LogModel
            {
                exception = ex,
                message = "Error!",
                module = "PGDataLayer/" + method,
                Type = tipo
            });
        
        }
        internal static string InsertLog(LogModel logToInsert, bool inloop = false)
        {
            if (inloop)
            {
                var errorMessage = JsonConvert.SerializeObject(logToInsert) + Environment.NewLine;
                nLogger.Error(errorMessage);
                return "CRITICAL";
            }
            try
            {
                using (var context = new EF.PaymentGatewayEntities())
                {
                    EF.Logs LogEntry = new EF.Logs();
                    LogEntry.Date = DateTime.Now;
                    LogEntry.createdBy = "system - " + AppConfigRepository.GetVersionNumber();
                    LogEntry.deleted = false;
                    LogEntry.Type = logToInsert.Type.ToString();
                    LogEntry.Thread = logToInsert.module;
                    LogEntry.Message = logToInsert.message;
                    LogEntry.Exception = (logToInsert.exception != null) ? logToInsert.exception.Message : "UNKNOWN";
                    LogEntry.InnerException = (logToInsert.exception != null) ? ((logToInsert.exception.InnerException != null) ? logToInsert.exception.InnerException.Message : "UNKNOWN") : "UNKNOWN";
                    LogEntry.Transaction = (logToInsert.transaction != null) ? logToInsert.transaction.ToString() : "";
                    LogEntry.createdOn = DateTime.Now;
                    context.Logs.Add(LogEntry);
                    context.SaveChanges();
                    return LogEntry.LogId.ToString();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = JsonConvert.SerializeObject(logToInsert) + Environment.NewLine;
                nLogger.Error(errorMessage);
                nLogger.Error(ex,"Error al guardar log");
                return "CRITICAL";
            }
        }
    }
}