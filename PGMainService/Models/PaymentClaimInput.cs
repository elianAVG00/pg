using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PGMainService.Models
{
    /// <summary>
    /// Commerce Item a anular
    /// </summary>
  

    /// <summary>
    /// Entrada de reclamo.
    /// </summary>
    public class PaymentClaimInput
    {
        /// <summary>
        /// Identificación de la transacción. Este dato es provisto en el ticket de pago.
        /// </summary>
        /// <example>123456789</example>
        [Required]
        public string TransactionId { get; set; }

        /// <summary>
        /// Identificación del comercio. Este dato es provisto al comienzo del proceso.
        /// </summary>
        /// <example>TiendaDeEjemplo</example>
        [Required]
        public string MerchantId { get; set; }

        /// <summary>
        /// Tipo de documento del reclamante.
        /// </summary>
        /// <example>DNI, Pasaporte</example>
        [Required]
        public string DocumentType { get; set; }

        /// <summary>
        /// Numero de documento del reclamante.
        /// </summary>
        /// <example>12321456</example>
        [Required]
        public string DocumentNumber { get; set; }


        /// <summary>
        /// Conjunto de Commerce Items a anular
        /// </summary>
        public List<string> ListOfCommerceItemCodeToRefund { get; set; }

        /// <summary>
        /// Nombre del reclamante.
        /// </summary>
        /// <example>Alicia</example>
        [RegularExpression(@"^[a-zA-Z áéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäëïöüÄËÏÖÜÑñ']+$", ErrorMessage = "El {0} ingresado es inválido")]
        [StringLength(50, ErrorMessage = "La longitud del campo {0} no puede superar los {1} caracteres")]
        [Required]
        public string ClaimerName { get; set; }

        /// <summary>
        /// Apellido del reclamante.
        /// </summary>
        /// <example>Bardelli</example>
        [RegularExpression(@"^[a-zA-Z áéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäëïöüÄËÏÖÜÑñ']+$", ErrorMessage = "El {0} ingresado es inválido")]
        [StringLength(50, ErrorMessage = "La longitud del campo {0} no puede superar los {1} caracteres")]
        [Required]
        public string ClaimerLastName { get; set; }

        /// <summary>
        /// Dirección de correo electrónico del reclamante (se enviará el comprobante al mismo).
        /// </summary>
        /// <example>consumidor@cliente.com</example>
        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "El {0} ingresado es inválido")]
        public string ClaimerMail { get; set; }

        /// <summary>
        /// Número de teléfono celular del reclamante.
        /// </summary>
        /// <example>0221 154896556</example>
        [RegularExpression(@"^[\d\- ]+$", ErrorMessage = "El {0} ingresado es inválido")]
        [StringLength(20, ErrorMessage = "La longitud del campo {0} no puede superar los {1} caracteres")]
        public string ClaimerCellphone { get; set; }

        /// <summary>
        /// Número de teléfono fijo del reclamante.
        /// </summary>
        /// <example>0221 4896556</example>
        [RegularExpression(@"^[\d\- ]+$", ErrorMessage = "El {0} ingresado es inválido")]
        [StringLength(20, ErrorMessage = "La longitud del campo {0} no puede superar los {1} caracteres")]
        public string ClaimerPhone { get; set; }

        /// <summary>
        /// Consideraciones que deban tenerse en cuenta al momento de registrar el reclamo.
        /// </summary>
        /// <example>Se registró un pago por error del operador</example>
        public string Observation { get; set; }

        private PropertyInfo[] _PropertyInfos = null;

        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}