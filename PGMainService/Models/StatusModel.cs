using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGMainService.Models
{
    public class StatusModel
    {

        public string Status_GET { get; set; }
        public string Status_POST { get; set; }
        public string Status_VPN { get; set; }
        public string Status_Internet { get; set; }
        public string Status_DataAccess { get; set; }
        public string Status_PluginSPS { get; set; }
        public string Status_PluginNPS { get; set; }
    }
    public class IsEPCValidModel
    {
        public string EPC { get; set; }
        public int serviceId { get; set; }
    }

}