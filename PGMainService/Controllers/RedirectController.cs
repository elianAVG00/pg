using PGMainService.Manager;
using PGMainService.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace PGMainService.Controllers
{

    [AllowAnonymous]
    public class RedirectController : ApiController
    {
        private Utils _utilities = new Utils();

        /// <summary>
        /// Devuelve el estado de salud del servicio de Redirect.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Redirect Service Online")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Post([FromBody] List<FormValue> postForm)
        {
            if (!this.ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);

            var sb = new StringBuilder();

            if (HttpContext.Current.Request.Form.Count > 0)
            {
                sb.Append("<html>");
                sb.AppendFormat("<body onload='document.forms[0].submit()'>Cargando...");

                sb.AppendFormat("<form action='{0}' method='post'>", HttpContext.Current.Request.Form["urlToPost"]);

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                    sb.AppendFormat("<input type='hidden' name='{0}' value='{1}'>", key, HttpContext.Current.Request.Form[key]);

                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");

                this._utilities.InsertLogCommon(LogType.Debug, "Redirect Process");

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(Regex.Replace(sb.ToString(), "^\"|\"$", ""), Encoding.UTF8, "text/html")
                };
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
        }
    }
}