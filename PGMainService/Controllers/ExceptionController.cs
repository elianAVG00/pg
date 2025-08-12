using PGMainService.Exceptions;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace PGMainService.Controllers
{
    [AllowAnonymous]
    public class ExceptionController : ApiController
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public void Get()
        {
            throw new ApiException("Error for developer", 64 /*0x40*/, new Uri("http://dev.api.com/error/64"));
        }
    }
}