using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{

    public class PaymentClaimResponseModel : PaymentClaimRequestResult
    {
        /// <summary>
        /// Id de la solicitud de respuesta
        /// </summary>
        public string AnnulmentRequest { get; set; }
    }
}