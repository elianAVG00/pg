using Newtonsoft.Json;
using PGMainService.Manager;
using PGMainService.Models;
using PGMainService.PGDataAccess;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PGMainService.Controllers
{
    public class IndexController : ApiController
    {
        private Utils _utils = new Utils();

        [AppConfig]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("")]
        public HttpResponseMessage GetIndex()
        {
            var stats = new Status();
            try
            {
                stats.Version = this._utils.GetResponse<long>("home", "pgversion");
                stats.VersionHash = this._utils.GetResponse<string>("home", "pgversionhash");

                using (var dataContext = new PGDataServiceClient())
                {
                    stats.IsServiceOn = dataContext.GetAppConfig("IsServiceOnline").Equals("true");
                    stats.IsPaymentServiceOn = dataContext.GetAppConfig("IsPaymentOnline").Equals("true");
                }
                stats.StateDescription = "Health Service is OK";

                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(stats));
            }
            catch (Exception ex)
            {
                stats.IsPaymentServiceOn = false;
                stats.IsServiceOn = false;
                stats.ErrorCode = 741;
                stats.StateDescription = "Servicio no disponible por el momento";
                stats.Version = 0L;
                stats.VersionHash = "";

                return Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(stats));
            }
        }
    }
}
