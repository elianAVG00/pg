using PGMainService.Manager;
using PGMainService.PGDataAccess;
using System;
using System.Net.Http;
using System.Web.Http;

namespace PGMainService.Controllers
{
    public class TicketController : ApiController
    {
        private Utils _utilities = new Utils();

        /// <summary>
        /// Devuelve la salud del servicio de Ticket
        /// </summary>
        [AllowAnonymous]
        [Route("Ticket")]
        public HttpResponseMessage GetStatus()
        {
            var header = _utilities.GetDataFromHTTPRequest(this.Request.Headers);

            using (var dataContext = new PGDataServiceClient())
            {
                bool isServiceOnline = Convert.ToBoolean(dataContext.GetAppConfig("IsServiceOnline"));
                if (!isServiceOnline)
                {
                    return _utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT_OFF, header.Language);
                }
                else
                {
                    return _utilities.GetHTTPResponse(Constants.PG_HTTP_PING_RESPONSE_PAYMENT, header.Language);
                }
            }
        }
    }
}