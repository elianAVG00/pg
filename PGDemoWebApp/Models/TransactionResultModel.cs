namespace PGDemoWebApp.Models
{
    public class TransactionResultModel
    {
        /// Número de transacción
        public string? TransactionId { get; set; }

        /// Código del canal
        public string? ChannelCode { get; set; }

        /// EPC - Electronic Payment Code
        public string? ElectronicPaymentCode { get; set; }

        /// Código del resultado
        public string? ResultCode { get; set; }

        /// Mensaje detallado del resultado
        public string? ResultMessage { get; set; }

        /// Código genérico del resultado
        public string? GenericCode { get; set; }

        /// Mensaje detallado del código genérico del resultado
        public string? GenericMessage { get; set; }

        /// Datos del pago
        public PaymentDetail? Payment { get; set; }

        /// Datos del servicio
        public ServiceDetail? Service { get; set; }

        /// Datos del tarjetahabiente
        public CustomerDetail? Customer { get; set; }

        /// Listado de items procesados en el pago
        public List<CommerceItemsDetail>? CommerceItems { get; set; }

        /// Listado de reclamos procesados en la transacción
        public List<ClaimsDetail>? Claims { get; set; }

        // --- Clases Anidadas ---

        /// Detalle del item procesado por reclamo
        public class CommerceItemsDetail
        {
            /// Descripción del item
            public string? Description { get; set; }

            /// Código del item
            public string? Code { get; set; }

            /// Monto del item
            public string? Amount { get; set; }

            /// Código original con el que fue realizada la transacción. New for 4.1 [Task 1926]
            public string? OriginalCode { get; set; }

            /// Estado del item
            public int State { get; set; }
        }

        /// Información del reclamante
        public class Claimer
        {
            /// Número de móvil
            public string? Cellphone { get; set; }

            /// Número de documento
            public string? DocNumber { get; set; }

            /// Tipo de documento
            public string? DocShortName { get; set; }

            /// Dirección de correo electrónico
            public string? Email { get; set; }

            /// Apellido/s
            public string? LastName { get; set; }

            /// Nombre/s
            public string? FirstName { get; set; }

            /// Teléfono fijo
            public string? Phone { get; set; }
        }

        /// Detalle de reclamos
        public class ClaimsDetail
        {
            /// Número de reclamo. Ejemplo: 123
            public long ClaimNumber { get; set; }

            /// Código de resultado del reclamo
            public string? ClaimResultCode { get; set; }

            /// Mensaje detallado del resultado del reclamo
            public string? ClaimResultMessage { get; set; }

            /// Items procesados en el reclamo. New for 4.1
            public List<CommerceItemsDetail>? CommerceItemsRefunded { get; set; }

            /// Reclamante. New for 4.1
            public Claimer? Claimer { get; set; }
        }

        /// Detalles del pago
        public class PaymentDetail
        {
            /// Código de moneda. Ejemplo: ARS
            public string? CurrencyCode { get; set; }

            /// Monto de la transacción
            public string? Amount { get; set; }

            /// Cantidad de pagos (cuotas). Ejemplo: siempre 1
            public string? Payments { get; set; }

            /// Timestamp de entrada de la transacción
            public string? PayDateTime { get; set; }

            /// Código de producto. Ejemplo: 8
            public string? ProductCode { get; set; }

            /// Nombre del producto. Ejemplo: VISA
            public string? ProductName { get; set; }

            /// Código de barras de la transacción. Ejemplo: ABC1234567890. Added for 4.1 [Task 2502]
            public string? BarCode { get; set; }
        }

        /// Detalles del servicio cliente
        public class ServiceDetail
        {
            /// Nombre legal del servicio. Ejemplo: CLIENTE
            public string? ClientLegalName { get; set; }

            /// Código corto del servicio. Ejemplo: CLT
            public string? ServiceShortCode { get; set; }

            /// Nombre detallado del servicio. Ejemplo: Cliente de pago
            public string? ServiceName { get; set; }

            /// Código de identificación del cliente. Ejemplo: CLIENTE_TEST
            public string? MerchantId { get; set; }
        }

        /// Detalles del tarjetahabiente
        public class CustomerDetail
        {
            /// Código de autorización. Ejemplo: 012345. Moved for 4.1 [Task 1926] //Undone for bug 3260
            public string? AuthorizationCode { get; set; }

            /// Número de tarjeta enmascarado. Ejemplo: 1234XXXXXXXX5678. Moved for 4.1 [Task 1926] //Undone for bug 3260
            public string? CardNumberMasked { get; set; }

            /// Correo electrónico del tarjetahabiente. Ejemplo: tarjetahabiente@dominio.com
            public string? CustomerMail { get; set; }

            /// Tipo de documento del tarjetahabiente. Ejemplo: DNI
            public string? CustomerDocType { get; set; }

            /// Número de documento del tarjetahabiente. Ejemplo: 12345678
            public string? CustomerDocNumber { get; set; }
        }
    }
}