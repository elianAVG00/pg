using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PGMainService.PGDataAccess;
using System.Net;
using System.Web.Http;
using NLog;

namespace PGMainService.Manager
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AppConfigAttribute : ActionFilterAttribute
    {
        public string ErrorTicketNumber = RandomString(10);
        private static Logger nLogger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            int serviceStatus = CheckServiceOnline();
            if (serviceStatus == 0)
            {
                var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Service is temporarily offline due maintenance");
                throw new HttpResponseException(response);

            }
            else if (serviceStatus == 2)
            {
                var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Service is temporarily offline, Please contact the administrator with the following identification: NPG-" + ErrorTicketNumber);
                throw new HttpResponseException(response);

            }


        }


        public int CheckServiceOnline()
        {
            int ServiceState = 0;
            try
            {
                //Check if service is online
                PGDataServiceClient _dataContext = new PGDataServiceClient();
                using (_dataContext)
                {
                    bool IsServiceOnline = Convert.ToBoolean(_dataContext.GetAppConfig("IsServiceOnline"));
                    if (IsServiceOnline)
                    {
                        ServiceState = 1;
                    }
                    else
                    {
                        ServiceState = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                string innerexception = "";
                if (ex.InnerException != null) {
                    innerexception = ex.InnerException.Message ?? "";
                }
                string filename = nLogger.Name;
                nLogger.Error("Code: {3} | Message: {0}, thread: {1}, exception: {2}", ex.Message, "AppConfigAttribute", innerexception, ErrorTicketNumber);
                ServiceState = 2;
            }
            return ServiceState;
        }

        public static string RandomString(int length)
        {
            const string chars = "123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}