
using System.ComponentModel.DataAnnotations;

namespace PGMainService.Models
{
    public class UpdateCommerceItemCodeModel
    {
        /// <summary>
        /// Código antiguo de identificación único del elemento (condición)
        /// </summary>
        [Required]
        public string OldCode { get; set; }

        /// <summary>
        /// Código nuevo de identificación único del elemento (seteo)
        /// </summary>
        [Required]
        public string NewCode { get; set; }

        /// <summary>
        /// Código de identificación del servicio
        /// </summary>
        [Required]
        public string merchantCode { get; set; }

        /// <summary>
        /// Identificador de la transacción
        /// </summary>
        [Required]
        public string TransactionId { get; set; }
    }
}