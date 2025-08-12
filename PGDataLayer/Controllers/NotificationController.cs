using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class NotificationController : ApiController
    {

        [Route("notification/SendNotificationComplex"), HttpPost]
        public HttpResponseMessage GetHTTPResponseByStatusCodeOrPGCode(SendNotificationModelComplex input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, NotificationRepository.SendNotification(input));
        }

        [Route("notification/SendNotificationByIDPKAndCode"), HttpPost]
        public HttpResponseMessage GetHTTPResponseByStatusCodeOrPGCode(SendNotificationModel input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, NotificationRepository.GetHTTPResponseByStatusCodeOrPGCode(input));
        }

        [Route("notification/SendQuickTicketOfPayment/{tidpk}"), HttpGet]
        public HttpResponseMessage SendQuickTicketOfPayment(long tidpk)
        {
            return Request.CreateResponse(HttpStatusCode.OK, NotificationRepository.SendQuickTicketOfPayment(tidpk));
        }
    }
}