using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGMainService.PGDataAccess;

namespace PGMainService.Models
{
    /// <summary>
    /// Detalles del tarjetahabiente
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Código de autorización
        /// </summary>
        /// <example>012345</example>
        public string AuthorizationCode { get; set; } //Moved for 4.1 [Task 1926] //Undone for bug 3260
        /// <summary>
        /// Número de tarjeta enmascarado
        /// </summary>
        /// <example>1234XXXXXXXX5678</example>
        public string CardNumberMasked { get; set; } //Moved for 4.1 [Task 1926] //Undone for bug 3260
        /// <summary>
        /// Correo electrónico del tarjetahabiente
        /// </summary>
        /// <example>tarjetahabiente@dominio.com</example>
        public string CustomerMail { get; set; }
        /// <summary>
        /// Tipo de documento del tarjetahabiente
        /// </summary>
        /// <example>DNI</example>
        public string CustomerDocType { get; set; }
        /// <summary>
        /// Número de documento del tarjetahabiente
        /// </summary>
        /// <example>12345678</example>
        public string CustomerDocNumber { get; set; }
    }
    /// <summary>
    /// Detalles del servicio cliente
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Nombre legal del servicio
        /// </summary>
        /// <example>CLIENTE</example>
        public string ClientLegalName { get; set; }
        /// <summary>
        /// Código corto del servicio
        /// </summary>
        /// <example>CLT</example>
        public string ServiceShortCode { get; set; }
        /// <summary>
        /// Nombre detallado del servicio
        /// </summary>
        /// <example>Cliente de pago</example>
        public string ServiceName { get; set; }
        /// <summary>
        /// Código de identificación del cliente
        /// </summary>
        /// <example>CLIENTE_TEST</example>
        public string MerchantId { get; set; }
    }
    /// <summary>
    /// Detalles del pago
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Código de moneda
        /// </summary>
        /// <example>ARS</example>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Monto de la transacción
        /// </summary>
        /// <example></example>
        public string Amount { get; set; }
        /// <summary>
        /// Cantidad de pagos (cuotas)
        /// </summary>
        /// <example>12</example>
        public string Payments { get; set; }
        /// <summary>
        /// Timestamp de entrada de la transacción
        /// </summary>
        /// <example></example>
        public string PayDateTime { get; set; }
        /// <summary>
        /// Código de producto
        /// </summary>
        /// <example>8</example>
        public string ProductCode { get; set; }
        /// <summary>
        /// Nombre del producto
        /// </summary>
        /// <example>VISA</example>
        public string ProductName { get; set; }
        /// <summary>
        /// Código de barras de la transacción
        /// </summary>
        /// <example>ABC1234567890</example>
        public string BarCode { get; set; } //Added for 4.1 [Task 2502]
    }
    /// <summary>
    /// Detalle de reclamos
    /// </summary>
    public class Claims {
        /// <summary>
        /// Número de reclamo
        /// </summary>
        /// <example>123</example>
        public long ClaimNumber { get; set; }
        /// <summary>
        /// Código de resultado del reclamo
        /// </summary>
        public string ClaimResultCode { get; set; }
        /// <summary>
        /// Mensaje detallado del resultado del reclamo
        /// </summary>
        public string ClaimResultMessage { get; set; }
        /// <summary>
        /// Items procesados en el reclamo
        /// </summary>
        public List<CommerceItems> CommerceItemsRefunded { get; set; } //New for 4.1
        /// <summary>
        /// Reclamante
        /// </summary>
        public Claimer Claimer { get; set; } //New for 4.1
    }
    /// <summary>
    /// Información del reclamante
    /// </summary>
    public class Claimer {
        /// <summary>
        /// Número de móvil
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// Número de documento
        /// </summary>
        public string DocNumber { get; set; }
        /// <summary>
        /// Tipo de documento
        /// </summary>
        public string DocShortName { get; set; }
        /// <summary>
        /// Dirección de correo electrónico
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Apellido/s
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Nombre/s
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Teléfono fijo
        /// </summary>
        public string Phone { get; set; }
    }


    /// <summary>
    /// Detalle del item procesado por reclamo
    /// </summary>
    public class CommerceItems {
        /// <summary>
        /// Descripción del item
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Código del item
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Monto del item
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Código original con el que fue realizada la transacción
        /// </summary>
        public string OriginalCode { get; set; } //New for 4.1 [Task 1926]
        /// <summary>
        /// Estado del item
        /// </summary>
        public int State { get; set; }
    }

    /// <summary>
    /// Datos de resultados de la transacción
    /// </summary>
    public class TransactionResult
    {
        /// <summary>
        /// Número de transacción
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// Código del canal
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// EPC - Electronic Payment Code
        /// </summary>
        public string ElectronicPaymentCode { get; set; }
        /// <summary>
        /// Código del resultado
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// Mensaje detallado del resultado
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// Código genérico del resultado
        /// </summary>
        public string GenericCode { get; set; }
        /// <summary>
        /// Mensaje detallado del código genérico del resultado
        /// </summary>
        public string GenericMessage { get; set; }
        /// <summary>
        /// Datos del pago
        /// </summary>
        public Payment Payment { get; set; }
        /// <summary>
        /// Datos del servicio
        /// </summary>
        public Service Service { get; set; }
        /// <summary>
        /// Datos del tarjetahabiente
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Listado de items procesados en el pago
        /// </summary>
        public List<CommerceItems> CommerceItems { get; set; }
        /// <summary>
        /// Listado de reclamos procesados en la transacción
        /// </summary>
        public List<Claims> Claims { get; set; }
    }

    /// <summary>
    /// Transacciones con reembolsos
    /// </summary>
    public class TransactionWithRefunds {
        /// <summary>
        /// Código de la transacción
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// Timestamp de la transacción
        /// </summary>
        public System.DateTime TransactionDate { get; set; }
        /// <summary>
        /// Electronic Payment Code
        /// </summary>
        public string EPC { get; set; }
        /// <summary>
        /// Código de moneda de la transacción
        /// </summary>
        public string TransactionCurrencyCode { get; set; }
        /// <summary>
        /// Monto de la transacción
        /// </summary> 
        public string TransactionAmount { get; set; }
        /// <summary>
        /// Código de cliente
        /// </summary>
        public string ClientCode { get; set; }
        /// <summary>
        /// Servicio
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Fecha de reembolso
        /// </summary>
        public string RefundDate { get; set; }
        /// <summary>
        /// Código de moneda de reembolso
        /// </summary>
        public string RefundCurrencyCode { get; set; }
        /// <summary>
        /// Monto del reembolso
        /// </summary>
        public string RefundAmount { get; set; }
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Máscara del número de tarjeta
        /// </summary>
        public string CardMask { get; set; }
        /// <summary>
        /// Código de autorización
        /// </summary>
        public string AuthorizationCode { get; set; }
        /// <summary>
        /// Listado de items relacionados a la transacción
        /// </summary>
        public List<CommerceItems> CommerceItems { get; set; }
    
    }
}