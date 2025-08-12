using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGPluginSPS.Controllers
{
    public class HomeController : ApiController
    {
        [AllowAnonymous]
        [Route("")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "PaymentClaimSPS - Service Online.");
        }
    }
}