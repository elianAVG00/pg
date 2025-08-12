using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PGSyncro.Utils
{
    public static class NetworkTools
    {
        public static WebProxy GetWebProxy()
        {
            //            <!-- Web Proxy Server -->
            if (ConfigurationManager.AppSettings["proxyOn"] == "on")
            {
                string proxyServer = ConfigurationManager.AppSettings["proxyServer"];
                string proxyPort = ConfigurationManager.AppSettings["proxyPort"];
                string proxyDomain = ConfigurationManager.AppSettings["proxyDomain"];
                string proxyUsername = ConfigurationManager.AppSettings["proxyUsername"];
                string proxyPassword = ConfigurationManager.AppSettings["proxyPassword"];
                WebProxy wbProxy;

                try
                {
                    wbProxy = new WebProxy(proxyServer, Int32.Parse(proxyPort))
                    {
                        Credentials = new NetworkCredential(proxyUsername, proxyPassword, proxyDomain)
                    };

                }
                catch (Exception ex)
                {
                    PGConfiguration.InsertLog("GetWebProxy", "No se pudo configurar el proxy ", ex);

                    return null;
                }
                return wbProxy;
            }
            return null;
        }

    }
}
