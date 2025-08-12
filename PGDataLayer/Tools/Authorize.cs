using PGDataLayer.Repositories;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PGDataLayer.Tools
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            string UserWorker = "";
            base.OnActionExecuting(context);
            bool continueWithAction = false;
            try
            {
                string headerValues = context.Request.Headers.GetValues("Authorization").FirstOrDefault().Replace("Basic ", "");
                context.ControllerContext.RequestContext.RouteData.Values["UserId"] = context.Request.Headers.GetValues("UserId").FirstOrDefault();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["User"] + ":" + ConfigurationManager.AppSettings["Password"]);
                if (headerValues == Convert.ToBase64String(plainTextBytes))
                {
                    continueWithAction = true;
                    try
                    {
                        UserWorker = context.Request.Headers.GetValues("UserWorker").FirstOrDefault();
                        context.ControllerContext.RequestContext.RouteData.Values["AdminUser"] = UserWorker;
                    }
                    catch (Exception e1x)
                    {
                        string detailuserworker = "";
                        if (String.IsNullOrWhiteSpace(UserWorker))
                        {
                            detailuserworker = "empty_or_null";
                        }
                        else
                        {
                            detailuserworker = UserWorker;
                        }
                        /*
                       LogRepository.InsertLog(new Models.LogModel
                        {
                            Type = Models.LogType.Debug,
                            module = "Authorization Filter OnActionExecuting",

                            details = "UserWorker: " + detailuserworker,
                            message = "Fallo al intentar recuperar AdminUser"


                        });
                        */
                     //   LogRepository.InsertLogException(Models.LogType.Error, e1x);
                    //    context.ControllerContext.RequestContext.RouteData.Values["AdminUser"] = "AUNF-" + AppConfigRepository.GetConfigValueFromBOSettings("appversion");
                    }

                }

            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(Models.LogType.Error, ex, false);
                continueWithAction = false;
            }
            if (!continueWithAction)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
    }





}