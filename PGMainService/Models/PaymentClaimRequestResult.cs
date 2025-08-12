namespace PGMainService.Models
{
    public class PaymentClaimRequestResult
    {
        /// <summary>
        /// Código del tipo genérico de respuesta
        /// </summary>
        public string ResponseGenericMessage { get; set; }

        /// <summary>
        /// Identificador del reclamo
        /// </summary>
        public long ClaimNumber { get; set; }

        /// <summary>
        /// Código del estado actual del reclamo
        /// </summary>
        public string ResponseStatusCode { get; set; }

        /// <summary>
        /// Mensaje del estado actual del reclamo
        /// </summary>
        public string ResponseStatusMessage { get; set; }

        /// <summary>
        /// Respuesta
        /// </summary>
        public string ResponseMessage { get; set; }
    }
}