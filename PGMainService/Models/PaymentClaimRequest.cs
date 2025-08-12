using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PGMainService.Models
{
    /// <summary>
    /// Solicitud de reclamo
    /// </summary>
    public class PaymentClaimRequest
    {
        /// <summary>
        /// Identificador único de reclamo.
        /// </summary>
        [Required]
        public long PaymentClaimNumber { get; set; }

        /// <summary>
        /// Observaciones de la solicitud.
        /// </summary>
        public string Observations { get; set; }

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

    public class PaymentClaimRequestWithMails : PaymentClaimRequest
    {
        public string MailListToNotificate { get; set; }
    }
}