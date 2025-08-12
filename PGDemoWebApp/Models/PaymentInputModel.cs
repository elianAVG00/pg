using System.ComponentModel.DataAnnotations;

namespace PGDemoWebApp.Models
{
    public class PaymentInputModel
    {
        [Display(Name = "ID de Servicio")]
        [Required(ErrorMessage = "El ID de Servicio es requerido.")]
        public string? MerchantId { get; set; }

        [Display(Name = "ID de Producto")]
        [Required(ErrorMessage = "El ID de Producto es requerido.")]
        public string? ProductId { get; set; }

        [Display(Name = "Código de Moneda (Ej: ARS)")]
        [Required(ErrorMessage = "El código de moneda es requerido.")] // No numérico, es string
        public string? CurrencyCode { get; set; } = "ARS"; // Default

        [Display(Name = "Monto (Ej: 123,45)")]
        [Required(ErrorMessage = "El monto es requerido.")]
        [RegularExpression(@"^\d+([,]\d{1,2})?$", ErrorMessage = "El formato del monto debe ser numérico, con coma opcional para decimales (ej: 123 o 123,45).")]
        public string? Amount { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string? MailAddress { get; set; }

        // Campos automáticos (no necesitan validación en el input del form)
        public int? Payments { get; set; }
        public string? ElectronicPaymentCode { get; set; }
        public string? BarCode { get; set; }
        public string? Channel { get; set; }
        public object? CommerceItems { get; set; } // Puede ser un string JSON o un objeto
        public int IsSimulation { get; set; }
        public string? CallbackUrl { get; set; }
        public string? ValidateEPCreturnUrl { get; set; }
    }

}
