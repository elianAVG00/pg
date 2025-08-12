using PGDataLayer.Models;
using PGDataLayer.Repositories;
using PGDataLayer.Tools;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    [AuthorizationFilter]
    public class SecurityController : ApiController
    {
        [Route("security/CanUserGetMerchantInfo"), HttpPost]
        public HttpResponseMessage CanUserGetMerchantInfo(MerchantUserInfoInput input)
        {
            return Request.CreateResponse(HttpStatusCode.OK, SecurityRepository.CanUserGetMerchantInfo(input));
        }
    }
}