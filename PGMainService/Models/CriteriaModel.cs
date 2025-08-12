using System.ComponentModel.DataAnnotations;

namespace PGMainService.Models
{
    public class CriteriaModelToSearchByEPC
    {


        /// <summary>
        /// Electronic Payment Code
        /// </summary>
        [Required]
        public string EPC { get; set; }

        /// <summary>
        /// Merchant Id
        /// </summary>
        [Required]
        public string merchantCode { get; set; }

    }

    public class CriteriaRePrintModel
    {

        /// <summary>
        /// Fecha de compra (en formato "yyyy-MM-dd")
        /// </summary>
        [Required]
        public string Date { get; set; }

        /// <summary>
        /// External Id del Servicio
        /// </summary>
        [Required]
        public int ExternalId { get; set; }
        /// <summary>
        /// Últimos 4 dígitos del número de la tarjeta de crédito
        /// </summary>
        public string CreditCard4LastDigits { get; set; }

        /// <summary>
        /// Código de Autorización
        /// </summary>
        [Required]
        public string AuthorizationCode { get; set; }

    }

    public class CriteriaModelReportToSearch
    {
        /// <summary>
        /// Identificador del comercio. Este dato es provisto al comienzo del proceso.
        /// </summary>
        /// <example>MLP</example>
        public string MerchantId { get; set; }


        /// <summary>
        /// Fecha de creación del reclamo, desde.
        /// </summary>
        /// <example>yyyyMMdd</example>
        //[RegularExpression("^[0-9]{2}/[0-9]{2}/[0-9]{4}$", ErrorMessage = "Formato de fecha no válido")]
        public string SearchFrom { get; set; }

        /// <summary>
        /// Fecha de creación del reclamo, hasta.
        /// </summary>
        /// <example>yyyyMMdd</example>
        //[RegularExpression("^[0-9]{2}/[0-9]{2}/[0-9]{4}$", ErrorMessage = "Formato de fecha no válido")]
        public string SearchTo { get; set; }

        /// <summary>
        /// Filtrar por un determinado CommerceItem Code (requiere obligatoriamente un TransactionId)
        /// </summary>
        public string CommerceItemCode { get; set; }

        /// <summary>
        /// Filtrar por un determinado TransactionId
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// (NO REQUERIDO) - Solo para procesos automáticos.
        /// </summary>
        public int AutomatedJobRunId { get; set; }
    }


    public class CriteriaModelPaymentToSearch
    {
        /// <summary>
        /// Identificador del comercio. Este dato es provisto al comienzo del proceso.
        /// </summary>
        /// <example>MLP</example>
        [Required]
        public string MerchantId { get; set; }


        /// <summary>
        /// Fecha de creación del reclamo, desde.
        /// </summary>
        /// <example>yyyyMMdd</example>
        //[RegularExpression("^[0-9]{2}/[0-9]{2}/[0-9]{4}$", ErrorMessage = "Formato de fecha no válido")]
        public string SearchFrom { get; set; }

        /// <summary>
        /// Fecha de creación del reclamo, hasta.
        /// </summary>
        /// <example>yyyyMMdd</example>
        //[RegularExpression("^[0-9]{2}/[0-9]{2}/[0-9]{4}$", ErrorMessage = "Formato de fecha no válido")]
        public string SearchTo { get; set; }
    }

    public class CriteriaModelForPaymentClaim : CriteriaModelPaymentToSearch
    {
        /// <summary>
        /// Identificador único de la transacción. Este dato es provisto en el ticket de pago.
        /// </summary>
        /// <example>123456789</example>
        public string TransactionId { get; set; }

        /// <summary>
        /// Código del estado del reclamo (Ver Información, Códigos de estado).
        /// </summary>
        /// <example>101 (reclamo abierto), 106 (Operación de anulación/devolución aprobada)</example>
        public string StatusCode { get; set; }

        /// <summary>
        /// Identificador único del reclamo. Este dato es provisto en el ticket de creación de reclamo.
        /// </summary>
        /// <example>123456789</example>
        public string PaymentClaimNumber { get; set; }

        /// <summary>
        /// Tipo de reclamo (parcial o total)
        /// </summary>
        /// <example>1: Total ó 2: Parcial</example>
        public int? Type { get; set; }

        /// <summary>
        /// Identificador único de tarjeta de crédito de Provincia Net (Ver Información, Listado de Tarjetas).
        /// </summary>
        /// <example>1 (American Express), 4 (Mastercard), 8 (VISA)</example>
        public int? ProductId { get; set; }
    }



}