using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Models
{
    public class SyncroModel
    {
        public string ServiceType { get; set; }

        public bool HasResponse { get; set; }

        public bool HasError { get; set; }

        public bool HasTransaction { get; set; }

        public string ErrorInQuery { get; set; }

        public string RequestLog { get; set; }

        public string ResponseLog { get; set; }
    }

}
