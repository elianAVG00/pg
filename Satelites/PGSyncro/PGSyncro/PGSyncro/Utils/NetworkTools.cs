using Microsoft.Extensions.Configuration; 
using PGSyncro.Utils; 
using System;
using System.Net;

namespace PGSyncro.Utils
{
    public class NetworkTools
    {
        private readonly IConfiguration _configuration;
        private readonly PGConfiguration _pgConfiguration; 
        public NetworkTools(IConfiguration configuration, PGConfiguration pgConfiguration)
        {
            _configuration = configuration;
            _pgConfiguration = pgConfiguration;
        }
        public WebProxy GetWebProxy()
        {
            if (_configuration["ProxySettings:proxyOn"] == "on")
            {
                string proxyServer = _configuration["ProxySettings:proxyServer"];
                string proxyPort = _configuration["ProxySettings:proxyPort"];
                string proxyDomain = _configuration["ProxySettings:proxyDomain"];
                string proxyUsername = _configuration["ProxySettings:proxyUsername"];
                string proxyPassword = _configuration["ProxySettings:proxyPassword"];

                try
                {
                    var wbProxy = new WebProxy(proxyServer, int.Parse(proxyPort))
                    {
                        Credentials = new NetworkCredential(proxyUsername, proxyPassword, proxyDomain)
                    };
                    return wbProxy;
                }
                catch (Exception ex)
                {
                    _pgConfiguration.InsertLogAsync("GetWebProxy", "No se pudo configurar el proxy ", ex);
                    return null;
                }
            }
            return null;
        }
    }
}