using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class LogController : ApiController
    {
        
        [Route("log/add"), HttpPost]
        public HttpResponseMessage AddLog(LogModel value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, LogRepository.InsertLog(value));
        }
        
    }
}