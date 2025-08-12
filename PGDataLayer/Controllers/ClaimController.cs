using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class ClaimController : ApiController
    {
            [Route("status/CanUserWorkWithPaymentClaimByPaymentClaimNumber"), HttpPost]
            public HttpResponseMessage CanUserWorkWithPaymentClaimByPaymentClaimNumber(HttpQueryClaim value)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ClaimRepository.CanUserWorkWithPaymentClaimByPaymentClaimNumber(value.user, value.claimnumber));
            }
    }
}