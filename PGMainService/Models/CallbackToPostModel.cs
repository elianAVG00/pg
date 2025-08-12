using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class CallbackToPostModel
    {
        /// <summary>
        /// Código de Tipo de respuesta
        /// </summary>
        public string ResponseGenericCode { get; set; }
        /// <summary>
        /// Mensaje de Tipo de respuesta
        /// </summary>
        public string ResponseGenericMessage { get; set; }
        /// <summary>
        /// Código de respuesta
        /// </summary>
        public string ResponseCode { get; set; }
        /// <summary>
        /// Mensaje de respuesta
        /// </summary>
        public string ResponseMessage { get; set; }
        /// <summary>
        /// Mensaje de respuesta extendido
        /// </summary>
        public string ResponseExtended { get; set; }
        /// <summary>
        /// Código de pago electrónico
        /// </summary>
        public string ElectronicPaymentCode { get; set; }
        /// <summary>
        /// Identificador de la transacción
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// MetaData de productos
        /// </summary>
        public string MetaData { get; set; }
        /// <summary>
        /// URL To Post
        /// </summary>
        public string ReturnUrl { get; set; }

    }
}