using System;
using System.Web.Http;
using System.Web.Mvc;
using PGMainService.Areas.HelpPage.ModelDescriptions;
using PGMainService.Areas.HelpPage.Models;
using System.Web.Configuration;
using System.Web.Security;
using System.Collections.Generic;
using PGMainService.PGDataAccess;
using System.Security.Principal;
using System.Linq;
using PGMainService.Manager;
using PGMainService.Models;

namespace PGMainService.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>



    public class HelpController : Controller
    {
        private Utils _utilities = new Utils();
        private readonly PGDataServiceClient _dataContext = new PGDataServiceClient();
        private const string ErrorViewName = "Error";
        private string _webAdminURL = WebConfigurationManager.AppSettings["PGWebAdmin_URL"];

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            _utilities.InsertLogCommon(LogType.Debug, "IsAuth? : " + System.Web.HttpContext.Current.User.Identity.IsAuthenticated.ToString());
            AfterAuthenticateRequest();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                using (PGDataAccess.PGDataServiceClient datacontext = new PGDataAccess.PGDataServiceClient())
                {
                    ViewBag.ActualVersion = "v" + datacontext.GetAppVersion("PGMS");
                }
                ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
                ViewBag.WebAdmin = _webAdminURL;
                return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
            }
            else
            {
                return Redirect(_webAdminURL);
            }
        }




        public ActionResult Api(string apiId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(apiId))
                {
                    HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                    if (apiModel != null)
                    {
                        return View(apiModel);
                    }
                }

                return View(ErrorViewName);
            }
            else
            {
                return Redirect(_webAdminURL);
            }
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(modelName))
                {
                    ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                    ModelDescription modelDescription;
                    if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                    {
                        return View(modelDescription);
                    }
                }

                return View(ErrorViewName);
            }
            else
            {
                return Redirect(_webAdminURL);
            }
        }

        private void AfterAuthenticateRequest()
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                _utilities.InsertLogCommon(LogType.Debug, "FormsCookieName : " + FormsAuthentication.FormsCookieName);
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        _utilities.InsertLogCommon(LogType.Debug, "FormsCookieName : " + FormsAuthentication.FormsCookieName);


                        string cookieValue = Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                        _utilities.InsertLogCommon(LogType.Debug, "Request.Cookies[FormsAuthentication.FormsCookieName].Value : " + cookieValue);

                        string username = FormsAuthentication.Decrypt(cookieValue).Name;
                        _utilities.InsertLogCommon(LogType.Debug, "FormsAuthentication.Decrypt(cookieValue).Name : " + username);
                        string userRoles = String.Empty;
                        List<RolModel> roles = _dataContext.GetRolesByUsername(username).ToList();
                        List<string> rolshortname = new List<string>();
                        foreach (RolModel rol in roles)
                        {
                            rolshortname.Add(rol.shortName);
                        }
                        string[] rolshortArray = rolshortname.ToArray();
                        GenericIdentity prinocipalIdentity = new GenericIdentity(username, "Forms");
                        System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(prinocipalIdentity, rolshortArray);
                    }
                    catch (Exception exception)
                    {
                        _utilities.InsertLogException(LogType.Error, exception);
                        throw new Exception("Ocurrió un error interno");
                    }
                }
            }
        }
    }
}