using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Web.Http.Description;

namespace PGMainService.Models
{

    /// <summary>
    /// Entrada de pago.
    /// </summary>
    public class PaymentInput
    {
        /// <summary>
        /// Opcional. Boolean. Flag para obtener el número de la transacción en la respuesta de la petición. ATENCIÓN: La respuesta posee un formato nuevo.
        /// </summary>
        public bool? WithTransactionNumber { get; set; }



        /// <summary>
        /// Identificación del comercio. Este dato es provisto al comienzo del proceso.
        /// </summary>
        /// <example>TiendaDeEjemplo</example>
        [Required]
        public string MerchantId { get; set; }

        /// <summary>
        /// Código del Pago Electrónico (EPC) que identifica en forma unívoca la petición o intención de pago.
        /// </summary>
        /// <example>7000142114464886</example>
        [Required]
        public string ElectronicPaymentCode { get; set; }

        /// <summary>
        /// Código internacional ISO 4217 de la moneda de pago
        /// </summary>
        /// <example>ARS</example>
        [Required]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Importe total del pago. Máximo 10 dígitos en la parte entera y 2 dígitos fijos para la parte fraccionaria; separados por coma.
        /// </summary>
        /// <example>123,00 ó 123,45</example>
        [Required]
        [RegularExpression(@"^[+]?[0-9]{1,}(?:[0-9]*(?:[.,][0-9]{1})?|(?:,[0-9]{3})*(?:\.[0-9]{1,2})?|(?:\.[0-9]{3})*(?:,[0-9]{1,2})?)$", ErrorMessage = "Importe total del pago. Máximo 10 dígitos en los enteros + 1 carácter para la coma + 2 dígitos para los decimales. Ej.: 10000,50")]
        public string Amount { get; set; }
     
        /*
         1 234 567 890,12
         */



        /// <summary>
        /// Identificador único de tarjeta de crédito de Provincia Net (Ver Información, Listado de Tarjetas).
        /// </summary>
        /// <example>1 (American Express), 4 (Mastercard), 8 (VISA)</example>
        [Required]
        [RegularExpression(@"^[0-9\.]+$", ErrorMessage = "El valor del parámetro ProductId debe de ser entero")]
        public string ProductId { get; set; }
        public int GetProductId() { return int.Parse(ProductId); }
        /// <summary>
        /// Cantidad de cuotas a realizarse en el pago.
        /// </summary>
        /// <example>1 ó 4 ó 12 (etc.)</example>
        [Required]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "El valor del parámetro Payments debe de ser entero")]
        public int Payments { get; set; }

        /// <summary>
        /// Dirección de correo electrónico del usuario final (se enviará el comprobante al mismo).
        /// </summary>
        /// <example>consumidor@cliente.com</example>
        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "La dirección de correo no es válida")]
        public string MailAddress { get; set; }

        /// <summary>
        /// Dirección de retorno para control de aplicación. Una vez concluida la operación se redireccionará a través de un POST los resultados de la transacción a este URL especificado.
        /// </summary>
        /// <example>http://aplicacion/procesarResultado</example>
        [Required]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Dirección URL a donde se volverá automáticamente si el ElectronicPaymentCode está repetido.
        /// Si es vacío o nulo, no se valida el Electronic Payment Code.
        /// </summary>
        //// <example>http://aplicación/regenerarEPC</example>
        public string ValidateEPCreturnUrl { get; set; }

        /// <summary>
        /// Código de barras de la factura, sticker o transacción, hasta 60 dígitos.
        /// </summary>
        /// <example>83576108935698</example>
        public string BarCode { get; set; }

        /// <summary>
        /// Especifica el canal para considerar opciones de inferfaz responsive. Por defecto, Web.
        ///  </summary>
        /// <example>web o mobile</example>
        public string Channel { get; set; }

        /// <summary>
        /// OBSOLETO - Representación string de un JSON que almacena cualquier información. La información de este campo es devuelvo por el callback.
        /// </summary>
        /// <example>{"CommerceItems": [{"Code": "TEST1","Description": "Nombre del producto que ha pagado con un largo 001","Price": "122.56"},{"Code": "TEST2","Description": "Nombre del producto que ha pagado con un largo 002","Price": "1233.56"}]}</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Metadata { get; set; }

        /// <summary>
        /// Representación string de un JSON que almacena cualquier información. La información de este campo es devuelvo por el callback.
        /// </summary>
        /// <example>[{"Code":"Code1","Description":"Compra de producto 10","Amount":"30"},{"Code":"Code2","Description":"Compra de servicio 63","Amount":"20"}]</example>
         public string CommerceItems { get; set; }
        //        public List<CommerceItems> GetCommerceItems() { return StringCIToListCI(CommerceItems); }

        /// <summary>
        /// Si desea dejar constancia de una versión específica de Payment Gateway, especifíquela aquí. Posibles: 3500, 3740, 4100
        /// </summary>
        /// <example>4100</example>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string AppVersion { get; set; }

        /// <summary>
        /// Si desea marcar la transacción como simulada, especifique 1
        /// </summary>
        /// <example>1 ó 0</example>
        [RegularExpression(@"^[0|1]$", ErrorMessage = "El valor del parámetro IsSimulation debe de ser 0 ó 1")]
        public int? IsSimulation { get; set; }

        //##############################################################################
        private PropertyInfo[] _PropertyInfos = null;

        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value);
            }

            return sb.ToString();
        }
    }
}