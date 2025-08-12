using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Models
{
   public class PGModels
    {
    }
    public class Status { 
        public int StatusCodeId { get; set; }
        public int GenericCodeId { get; set; }
    }
    public class TransactionOriginal {
        public SyncroModel QueryResponse { get; set; }
        public string ModuleType  { get; set; }
        public string OriginalCode { get; set; }
        public long TransactionIdPK { get; set; }
        public long Amount { get; set; }
        public int Payments { get; set; }
        public string AuthorizationCode { get; set; }
        public string CardMask { get; set; }
        public string Card4LastDigits { get; set; }
        public string CardHolder { get; set; }
        public string TicketNumber { get; set; }
        public string Mail { get; set; }
        public string NroLote { get; set; }

    }
}
