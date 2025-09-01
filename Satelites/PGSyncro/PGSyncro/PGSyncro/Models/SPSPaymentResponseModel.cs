using System.Collections.Generic;

namespace PGSyncro.Models
{
    public class SPSPaymentResponseModel
    {
        public int limit { get; set; }

        public int offset { get; set; }

        public List<Result> results { get; set; }

        public bool hasMore { get; set; }
    }
}