using NLog;
using PGDataAccess.Models;
using System;

namespace PGDataAccess.Tools
{
    public class LogTool
    {
        private static Logger nLogger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Inserta log en la tabla de logs o en archivo con NLog, segun configuracion, 
        /// o bien inserta en archivo de log si no puede trabajar con la base de datos
        /// </summary>
        /// <param name="log"></param>
        public long SaveLog(LogModel log)
        {
            long returnValue = 0;
            string thread = string.Join("//", log.AppLayer, log.CallerMemberName, log.CallerFilePath, log.CallerLineNumber.ToString()); 

            try
            {
                if (IsOkForLog(log.LogType))
                {
                    switch (log.LogType)
                    {
                        //FGS v4.3 - Se cambio el tipo del logger de info a debug
                        case LogTypeModel.Debug:
                            nLogger.Debug("Message: {0}, thread: {1}, exception: {2}", log.Message, thread, log.Exception?.ToString());
                            break;
                        //FGS v4.3 - Se cambio el tipo del logger de info a error
                        case LogTypeModel.Error:
                            nLogger.Error("Message: {0}, thread: {1}, exception: {2}", log.Message, thread, log.Exception?.ToString());
                            break;

                        case LogTypeModel.Info:
                            nLogger.Info("Message: {0}, thread: {1}, exception: {2}", log.Message, thread, log.Exception?.ToString());
                            break;

                        //FGS v4.3 - Se cambio el tipo del logger de info a warn    
                        case LogTypeModel.Warning:
                            nLogger.Warn("Message: {0}, thread: {1}, exception: {2}", log.Message, thread, log.Exception?.ToString());
                            break;
                    }

                    if (System.Configuration.ConfigurationManager.AppSettings["logOnDataBase"]?.ToLower() == "on")
                    {
                        using (var context = new EF.PGDataEntities())
                        {
                            var logToSave = new EF.Logs
                            {
                                Date = DateTime.Now,
                                Type = log.LogType.ToString(),
                                Thread = thread,
                                Message = log.Message,
                                Exception = log.Exception?.ToString(),
                                createdBy = "system",
                                createdOn = DateTime.Now
                            };
                            context.Logs.Add(logToSave);

                            context.SaveChanges();
                            returnValue = logToSave.LogId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //FGS v4.3 - Se aclara en el log cuando la excepción es al intentar loggear
                var errorMessage = string.Format("[LOG ERROR] Message :" + ex.Message + "<br/>" + ex + Environment.NewLine + "StackTrace :" + ex.StackTrace + "" +
                                                       Environment.NewLine + "----------------------------------------------------------------------------" +
                                                       Environment.NewLine);
                nLogger.Error(errorMessage);

                throw new Exception("Ocurrió un error interno en la capa de acceso a datos", ex);

            }
            return returnValue;
        }

        private bool IsOkForLog(LogTypeModel type)
        {
            return (type == LogTypeModel.Debug && (System.Configuration.ConfigurationManager.AppSettings["logDebug"]?.ToLower() == "on")) ||
                   (type == LogTypeModel.Error && (System.Configuration.ConfigurationManager.AppSettings["logError"]?.ToLower() == "on")) ||
                   (type == LogTypeModel.Info && (System.Configuration.ConfigurationManager.AppSettings["logInfo"]?.ToLower() == "on")) ||
                   (type == LogTypeModel.Warning && (System.Configuration.ConfigurationManager.AppSettings["logWarning"]?.ToLower() == "on"));
        }
    }
}