using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPS_ServiceReference.Models
{
    public class SPSCommunicationAPI
    {
        public string LogRequest { get; set; }
        public string LogResponse { get; set; }
        public string returnValue { get; set; }
        public bool resultCanBeParsed { get; set; }
    }
}
