using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace PGMainService.Controllers
{
    public class StatusController : ApiController
    {
        /// <summary>
        /// Devuelve estados de salud del servicio.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Roles = "apiStatus,apiAdminServices")]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();

            try
            {
                if (!HttpContext.Current.User.IsInRole("apiStatus"))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "User can't use this method");
                }

                // Si tiene el rol "apiStatus", retorna Forbidden.
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error handling authorization");
            }
        }
    }
}