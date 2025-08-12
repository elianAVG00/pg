using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataLayer.EF;
using PGDataLayer.Models;
namespace PGDataLayer.Repositories
{
    internal static class StatusRepository
    {
        internal static string GetValidationResponseByStatusCode(string StatusCode, string language = "es")
        {
            string httpResponse = "Error in validation code. Please contact the administrator";
            try
            {
                using (var context = new PaymentGatewayEntities())
                {
                    httpResponse = (from sc in context.StatusCode
                                    join sm in context.StatusMessage
                                        on sc.StatusCodeId equals sm.StatusCodeId
                                    where (sc.Code == StatusCode)
                                    && sc.IsActive && sm.IsActive
                                    && (sm.Language.ISO6391 == language || sm.Language.ISO6391 == language || sm.Language.ISO6392 == language)
                                    select sm.Message).FirstOrDefault();
                    return httpResponse;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                        LogRepository.InsertLog(new LogModel
                                        {
                                            exception = ex,
                                            message = "Error en Modulo de estados",
                                            module = "PGDataLayer/StatusRepository/GetValidationResponseByStatusCode",
                                            Type = LogType.Exception
                                        }));
            }
        }

        internal static StatusCode GetStatusCodeByCode(string statusCode)
        {
            try
            {
                using (var context = new PaymentGatewayEntities())
                {
                   return context.StatusCode.Where(c=>c.Code == statusCode).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                         LogRepository.InsertLog(new LogModel
                                         {
                                             exception = ex,
                                             message = "Error en Modulo de estados",
                                             module = "PGDataLayer/StatusRepository/GetStatusCodeByCode",
                                             Type = LogType.Exception
                                         }));
            }
        }

        internal static HTTPResponseModel GetHTTPResponseByStatusCodeOrPGCode(string StatusCodeOrPGCode, string language = "es")
        {
            HTTPResponseModel httpResponse = new HTTPResponseModel();
            HTTPResponseModel httpResponseERR = new HTTPResponseModel();
            httpResponseERR.HTTPStatusCode = 500;
            httpResponseERR.Message = "NACK - Service is unavailable or there is an error on the request - Error HTTP500";
            try
            {
                using (var context = new PaymentGatewayEntities())
                {
                    httpResponse = (from sc in context.StatusCode
                                    join sm in context.StatusMessage
                                        on sc.StatusCodeId equals sm.StatusCodeId
                                    join cm in context.CodeMapping
                                    on sm.StatusCodeId equals cm.StatusCodeId
                                    join mc in context.ModuleCode
                                    on cm.ModuleCodeId equals mc.ModuleCodeId
                                    where (sc.Code == StatusCodeOrPGCode || mc.OriginalCode == StatusCodeOrPGCode)
                                    && sc.IsActive && sm.IsActive
                                    && (sm.Language.ISO6391 == language || sm.Language.ISO6391 == language || sm.Language.ISO6392 == language)
                                    select new HTTPResponseModel
                                    {
                                        Message = sm.Message,
                                        StatusCode = sc.Code,
                                        PG_Code = mc.OriginalCode,
                                        StatusCodeId = sc.StatusCodeId
                                    }).FirstOrDefault();
                    if (httpResponse != null)
                    {
                        httpResponse.HTTPStatusCode = Convert.ToInt16(httpResponse.StatusCode.Split('_')[1]);
                        return httpResponse;
                    }
                    else
                    {
                        return httpResponseERR;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno - ERR-" +
                                        LogRepository.InsertLog(new LogModel
                                        {
                                            exception = ex,
                                            message = "Error en Modulo de estados",
                                            module = "PGDataLayer/StatusRepository/GetHTTPResponseByStatusCodeOrPGCode",
                                            Type = LogType.Exception
                                        }) );
            }
        }
    }
}