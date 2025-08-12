using PGDataAccess.Tools;
using PGDataAccess.Models;
using System;
using System.Runtime.CompilerServices;

namespace PGDataAccess.Repository
{
    public static class LogRepository
    {
        private static LogTool logTool = new LogTool();
        private static string _applayer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static long SaveLog(LogModel log)
        {
            return logTool.SaveLog(log);
        }

        /// <summary>
        /// Metodo exclusivo del DataAccess
        /// Adapta Entrada de Log con excepción al método SaveLog
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logException"></param>
        /// <param name="CallerMemberName"></param>
        /// <param name="CallerFilePath"></param>
        /// <param name="CallerLineNumber"></param>
        public static long InsertLogException(LogTypeModel logType, Exception logException, [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {
            string message = logException.Message;
            string exception = logException.StackTrace;
            //Si existe InnerException agrego el mensaje
            if (logException.InnerException != null)
            {
                message += " - InnerException: " + logException.InnerException.Message;
            }

            return logTool.SaveLog(MapToLogModel(logType, message, exception, CallerMemberName, CallerFilePath, CallerLineNumber, TransactionNumber, logtransactionType));
        }

        public static long InsertLogCommon(LogTypeModel logType, string logMessage, string logException = "", [CallerMemberName] string CallerMemberName = "", [CallerFilePath] string CallerFilePath = "", [CallerLineNumber] int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {

            return logTool.SaveLog(MapToLogModel(logType, logMessage, logException, CallerMemberName, CallerFilePath, CallerLineNumber, TransactionNumber, logtransactionType));
        }

        public static LogModel MapToLogModel(LogTypeModel logType, string logMessage, string logException, string CallerMemberName = "", string CallerFilePath = "", int CallerLineNumber = 0, string TransactionNumber = "", LogTransactionType? logtransactionType = null)
        {
            var logToSave = new LogModel();
            logToSave.Exception = logException;
            logToSave.LogType = logType;
            logToSave.Message = logMessage;
            logToSave.TransactionNumber = TransactionNumber;
            logToSave.CallerFilePath = CallerFilePath;
            logToSave.CallerLineNumber = CallerLineNumber;
            logToSave.CallerMemberName = CallerMemberName;
            logToSave.AppLayer = _applayer;
            logToSave.TransactionType = logtransactionType;
            return logToSave;
        }
    }
}