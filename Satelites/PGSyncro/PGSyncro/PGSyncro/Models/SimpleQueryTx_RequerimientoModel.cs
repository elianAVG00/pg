namespace PGSyncro.Models
{
    public class SimpleQueryTx_RequerimientoModel
    {
        public string MerchantId { get; set; }

        public string QueryCriteriaId { get; set; }

        public string QueryCriteria { get; set; }

        public string SecurityHash { get; set; }
    }
}