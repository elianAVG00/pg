using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class StatusController : ApiController
    {
        [Route("status/GetHTTPResponseByStatusCodeOrPGCode"), HttpPost]
        public HttpResponseMessage GetHTTPResponseByStatusCodeOrPGCode(HTTPResponseQueryModel input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, StatusRepository.GetHTTPResponseByStatusCodeOrPGCode(input.pgcode, input.lang));
        }

        [Route("status/GetValidationResponseByStatusCode"), HttpPost]
        public HttpResponseMessage GetValidationResponseByStatusCode(string pgcode, string lang)
        {
            return Request.CreateResponse(HttpStatusCode.OK, StatusRepository.GetValidationResponseByStatusCode(pgcode, lang));
        }
    }
}