using PGMainService.Models; 
using PGMainService.PGDataAccess; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace PGMainService.Controllers
{

    [AllowAnonymous]
    public class InformationController : ApiController
    {
        private readonly PGDataServiceClient dataContext = new PGDataServiceClient();

        /// <summary>
        /// Devuelve la salud del servicio de Informativo.
        /// </summary>
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Information - Service Online.")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        /// <summary>
        /// Devuelve todas las tarjetas disponibles para determinado comercio.
        /// </summary>
        [Route("Information/creditCards/{merchantId}")]
        public HttpResponseMessage GetAllCreditCards(string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "El campo merchantId no puede ser nulo.");

            ServiceModel service = this.dataContext.GetServiceByMerchantId(merchantId);
            if (service == null)
            {
                // Devolver error si el merchant no existe
                return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError($"Merchant with Id {merchantId} not found."));
            }
            int serviceId = service.ServiceId;
            List<ProductModel> products = this.dataContext.GetProductsByService(serviceId).ToList();

            if (products.Count > 0)
            {
                IEnumerable<CreditCard> creditCards = products.Select(prod => new CreditCard()
                {
                    Id = prod.ProductCode,
                    Name = prod.Description,
                    Type = prod.Type
                });

                var distinctCreditCards = creditCards.GroupBy(cc => cc.Id)
                                                     .Select(group => group.First());

                return Request.CreateResponse(HttpStatusCode.OK, distinctCreditCards);
            }
            else // Si no hay productos
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new HttpError("Actualmente no hay productos que respondan a los parámetros especificados. Consulte con el administrador."));
            }
        }

        /// <summary>
        /// Devuelve toda la información que recibe a través de POST
        /// </summary>
        public HttpResponseMessage Post()
        {
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body>");
            if (HttpContext.Current.Request.Form.Count > 0)
            {
                sb.AppendFormat("<table border='1px'><tr><td><h3>  Parámetro  </h3></td><td><h3>  Valor  </h3></td></tr>");

                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                    sb.AppendFormat("<tr><td style='text-align:center;'>{0}</td><td style='text-align:center;'>{1}</td></tr>", key, HttpContext.Current.Request.Form[key]);

                sb.AppendFormat("</table>");
            }
            else
            {
                sb.AppendFormat("No se han recibido parámetros.");
            }
            sb.Append("</body>");
            sb.Append("</html>");

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(Regex.Replace(sb.ToString(), "^\"|\"$", ""), Encoding.UTF8, "text/html")
            };
            return response;
        }
    }

}