using Newtonsoft.Json;
using PGPluginSPS.PGDataAccess;
using RestSharp;
using System;
using System.Runtime.CompilerServices;
using NLog;
using LogTransactionType = PGPluginSPS.PGDataAccess.LogTransactionType;

namespace PGPluginSPS.Utils
{
    public static class LogTool
    {
        public static long InsertLogException(LogTypeModel logType, Exception logException, [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {
            return InsertLog(MapToLogModel(logType, logException.Message, logException, CallerMemberName, CallerFilePath, CallerLineNumber, TransactionNumber));
        }

        public static long InsertLogCommon(LogTypeModel logType, string logMessage, Exception logException = null, [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {
            InsertLog(MapToLogModel(logType, logMessage, logException, CallerMemberName, CallerFilePath, CallerLineNumber, TransactionNumber));
            return 1;
        }

        private static Models.LogModel MapToLogModel(LogTypeModel logType, string logMessage, Exception logException, string CallerMemberName = "", string CallerFilePath = "", int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {
            return new Models.LogModel()
            {
                Exception = logException, // El original esperaba un string (StackTrace) aquí.
                Type = logType,
                Message = logMessage,
                // Uso de interpolación de cadenas y nombres de parámetros más descriptivos.
                Module = $"CallerMemberName: {CallerMemberName} - CallerFilePath {CallerFilePath} - CallerLineNumber {CallerLineNumber} - TN: {TransactionNumber} "
            };
        }

        private static void SaveToNLog(string message)
        {
            LogManager.GetCurrentClassLogger().Fatal(message);
        }

        private static long InsertLog(Models.LogModel logEntry, bool calledFromApiClient = false) // Renombrado de logtoinsert y fromAPIClient
        {
            try
            {
                var webService = new WebServiceInterface();
                var jsonPayload = JsonConvert.SerializeObject(logEntry); 
                var responseJson = webService.ConnectToLDAPI(Method.POST, "/log/add", jsonPayload, calledFromApiClient);

                long logId = JsonConvert.DeserializeObject<long>(responseJson); 

                if (logId != 0)
                {
                    return logId;
                }
                else
                {
                    throw new Exception($"Error Irrecuperable 64 || {JsonConvert.SerializeObject(logEntry)}");
                }
            }
            catch (Exception ex)
            {
                LogTool.SaveToNLog(ex.Message);
                throw new Exception($"Error Irrecuperable: {ex.Message} ||| {ex.InnerException?.Message}");
            }
        }
    }
}