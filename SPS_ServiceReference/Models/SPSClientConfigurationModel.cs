using System;
using System.Collections.Generic;
using System.Text;

namespace SPS_ServiceReference.Models
{

    public class ProxySettings
    {
        public bool ProxyActive { get; set; }
        public string Port { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }

    public class SPSClientConfigurationModel
    {
        public ProxySettings ProxyConfiguration { get; set; }
        public string urlbase { get; set; }
        public string urlrequest { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string apikeyPrivate { get; set; }
    }
}
