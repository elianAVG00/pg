using PGMainService.PGDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PGMainService.Manager
{

    public class CustomAuthorize : System.Web.Http.AuthorizeAttribute
    {

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        public UserModel CheckCredential(string username, string password)
        {
            PGDataServiceClient _dataContext = new PGDataServiceClient();
            using (_dataContext)
            {
                int userId = _dataContext.LoginUser(username, password);
                if (userId != 0)
                {
                    return _dataContext.GetUser(userId);
                }
                else
                {
                    return null;
                }
            }
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            IPrincipal client = Thread.CurrentPrincipal;
            try
            {
                string auth = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                PGDataServiceClient _dataContext = new PGDataServiceClient();
                if (auth != null)
                {
                    Regex regex = new Regex("Basic ");
                    byte[] encodedDataAsBytes = Convert.FromBase64String(regex.Replace(auth, "", 1));
                    string value = Encoding.UTF8.GetString(encodedDataAsBytes);
                    string username = value.Substring(0, value.IndexOf(':'));
                    string password = value.Substring(value.IndexOf(':') + 1);

                    UserModel objUser = CheckCredential(username, password);
                    if (objUser != null)
                    {
                        List<RolModel> roles = _dataContext.GetRolesByUsername(objUser.username).ToList();
                        List<string> rolshortname = new List<string>();
                        foreach (RolModel rol in roles)
                        {
                            rolshortname.Add(rol.shortName);
                        }
                        string[] rolarray = rolshortname.ToArray();


                        IPrincipal principal = new GenericPrincipal(new GenericIdentity(objUser.username), rolarray);
                        actionContext.Request.GetRequestContext().Principal = principal;
                        SetPrincipal(principal);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

    }

}