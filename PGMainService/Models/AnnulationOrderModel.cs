using System.Reflection;
using System.Text;

namespace PGMainService.Models
{
    public class AnnulationOrderModel
    {
        public int RequestOrder { get; set; }

        public Operation OperationType { get; set; }

        public long PaymentClaimId { get; set; }

        public string TransactionId { get; set; }

        public string Import { get; set; }

        public string Cents { get; set; }


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