using System.Reflection;
using System.Text;

namespace PGPluginSPS.Models
{
    public class PaymentInput
    {
        public bool HasCommerceItems { get; set; }

        public int Payments { get; set; }

        public string Amount { get; set; }

        public string BarCode { get; set; }

        public string CallbackUrl { get; set; }

        public int ProductId { get; set; }

        public string ClientLanguage { get; set; }

        public string CurrencyCode { get; set; }

        public string ElectronicPaymentCode { get; set; }

        public string MailAddress { get; set; }

        public string MerchantId { get; set; }

        public string Metadata { get; set; }

        public string ReturnUrl { get; set; }

        public string WindowMode { get; set; }

        public string TryWithAnotherCardUrl { get; set; }

        public string IsMobileDevice { get; set; }

        public string Channel { get; set; }

        public string TransactionId { get; set; }

        public string EncSHA1key { get; set; }

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