using PGDataLayer.EF;
using PGDataLayer.Models;
using PGDataLayer.Repositories;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PGDataLayer.Controllers
{
    public class HomeController : ApiController
    {
        public string Get()
        {
            return System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString().Replace(".dll","") + " - " + AppConfigRepository.GetVersion("DB");
        }

        [Route("home/pgversionhash"),HttpGet]
        public string PGMainServicesVersionHash()
        {
            return AppConfigRepository.GetVersion("PGMS");
        }

        [Route("home/pgversion"), HttpGet]
        public long PGMainServicesVersion()
        {
            return AppConfigRepository.GetVersionNumber();
        }

        [Route("home/config/{value}")]
        [HttpGet]
        public string GetConfigValue(string value) => AppConfigRepository.GetConfigValue(value);

        [Route("home/configuration/{value}")]
        [HttpGet]
        public AppConfig GetConfig(string value) => AppConfigRepository.GetConfig(value);

        [Route("home/SetConfig")]
        [HttpPost]
        public HttpResponseMessage SetConfig(HTTPResponseQueryModel value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, AppConfigRepository.SetConfig(value.pgcode, value.lang));
        }
    }
}