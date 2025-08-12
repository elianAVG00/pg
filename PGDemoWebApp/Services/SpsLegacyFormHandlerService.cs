using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PGDemoWebApp.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace PGDemoWebApp.Services
{
    public class SpsLegacyFormHandlerService : ISpsLegacyFormHandlerService
    {
        private readonly ILogger<SpsLegacyFormHandlerService> _logger;
        private readonly ApiSettings _apiSettings;
        private readonly ProxySettings _proxySettings;

        public SpsLegacyFormHandlerService(ILogger<SpsLegacyFormHandlerService> logger, IOptions<ApiSettings> apiSettings, IOptions<ProxySettings> proxySettings)
        {
            _logger = logger;
            _apiSettings = apiSettings.Value;
            _proxySettings = proxySettings.Value;
        }

        public async Task<DecidirFormSetupResult> SetupDecidirFormAsync(string paymentRedirectURL, string refererUrl, IUrlHelper urlHelper)
        {
            var inputRegex = new Regex("<input[^>]+name=['\"]?(?<name>[^'\"\\s]+)['\"]?[^>]*value=['\"]?(?<value>[^'\"\\s>]*)", RegexOptions.IgnoreCase);
            var matches = inputRegex.Matches(paymentRedirectURL);

            var postData = new Dictionary<string, string>();
            foreach (Match match in matches)
            {
                string name = match.Groups["name"].Value;
                string value = match.Groups["value"].Value;
                postData[name] = value;
            }

            var actionRegex = new Regex("<form[^>]+action=['\"]?(?<url>[^'\"\\s>]+)", RegexOptions.IgnoreCase);
            string actionUrl = actionRegex.Match(paymentRedirectURL).Groups["url"].Value;

            var handler = CreateHttpClientHandlerWithProxy();
            handler.CookieContainer = new CookieContainer();

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Referrer = new Uri(refererUrl);

                var content = new FormUrlEncodedContent(postData);
                var response = await client.PostAsync(actionUrl, content);
                response.EnsureSuccessStatusCode();

                string cookieHeader = string.Empty;
                var cookies = handler.CookieContainer.GetCookies(new Uri(actionUrl)).Cast<Cookie>();
                if (cookies.Any())
                {
                    // Unimos todas las cookies en un solo string como lo hace el navegador
                    cookieHeader = string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
                    _logger.LogInformation("Cookie de Decidir obtenida: {CookieHeader}", cookieHeader);
                }
                else
                {
                    _logger.LogWarning("No se recibieron cookies de Decidir en el POST a /forms/Validar.");
                }

                string rawHtml = await response.Content.ReadAsStringAsync();
                string htmlCorregido = NormalizeHtmlPaths(rawHtml);

                // meto el 'action' del formulario para apuntar a nuestro handler
                string newActionUrl = urlHelper.Page("/Index", "SubmitCardData") ?? "/Index?handler=SubmitCardData";
                string finalHtml = Regex.Replace(htmlCorregido, @"action\s*=\s*[""']/forms/Transaccion[""']", $"action=\"{newActionUrl}\"", RegexOptions.IgnoreCase);

                return new DecidirFormSetupResult
                {
                    RenderedHtml = finalHtml,
                    SessionCookieHeader = cookieHeader
                };
            }
        }

        public async Task<HttpResponseMessage> SubmitCardDataAsync(Dictionary<string, string> cardFormData, string refererUrl, string? sessionCookie)
        {
            if (cardFormData.TryGetValue("VENCTARJETA", out var vencMMAA) && !string.IsNullOrEmpty(vencMMAA) && vencMMAA.Length == 4)
            {
                string vencAAMM = vencMMAA.Substring(2, 2) + vencMMAA.Substring(0, 2);// ej "31" + "12" = "3112"
                cardFormData["VENCIMIENTO"] = vencAAMM;
            }

            try
            {
                var handler = CreateHttpClientHandlerWithProxy();
                handler.UseCookies = false;
                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Referrer = new Uri(refererUrl);

                    if (!string.IsNullOrEmpty(sessionCookie))
                    {
                        client.DefaultRequestHeaders.Add("Cookie", sessionCookie);
                    }
                    else
                    {
                        _logger.LogWarning("No se encontró ninguna cookie de Decidir en la sesión para reenviar.");
                    }
                    string originalActionUrl = "https://developers.decidir.com/forms/Transaccion";

                    var content = new FormUrlEncodedContent(cardFormData);
                    var response = await client.PostAsync(originalActionUrl, content);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción en OnPostSubmitCardDataAsync.");
                // chequear como mandar el error
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Ocurrió una excepción interna en el servicio proxy.")
                };
            }
        }

        public string NormalizeHtmlPaths(string html)
        {
            string baseUrl = "https://developers.decidir.com";

            html = Regex.Replace(html, @"href\s*=\s*[""'](/[^""']+)[""']", $"href=\"{baseUrl}$1\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"src\s*=\s*[""'](/[^""']+)[""']", $"src=\"{baseUrl}$1\"", RegexOptions.IgnoreCase);

            var regexSdxUrl = new Regex(@"href\s*=\s*[""'](https?://sdx-pgmainservices.provincianet.com.ar/TransactionTerminal[^""']*)[""']", RegexOptions.IgnoreCase);
            var match = regexSdxUrl.Match(html);

            if (match.Success)
            {
                string urlSdxCompleta = match.Groups[1].Value;
                string urlDevCompleta = urlSdxCompleta.Replace("https://sdx-pgmainservices.provincianet.com.ar", _apiSettings.PaymentGateway_URL);

                html = html.Replace(urlSdxCompleta, urlDevCompleta);
            }

            return html;
        }

        private HttpClientHandler CreateHttpClientHandlerWithProxy()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            if (_proxySettings.ProxyOn?.ToLower() == "on")
            {
                _logger.LogInformation("Configurando HttpClient para usar Proxy: {ProxyServer}:{ProxyPort}", _proxySettings.ProxyServer, _proxySettings.ProxyPort);
                try
                {
                    var proxy = new WebProxy
                    {
                        Address = new Uri($"http://{_proxySettings.ProxyServer}:{_proxySettings.ProxyPort}"),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = false
                    };

                    // agrego credenciales si estan configuradas
                    if (!string.IsNullOrEmpty(_proxySettings.ProxyUsername) && _proxySettings.ProxyPassword != null)
                    {
                        proxy.Credentials = new NetworkCredential(
                            _proxySettings.ProxyUsername,
                            _proxySettings.ProxyPassword,
                            _proxySettings.ProxyDomain
                        );
                    }

                    handler.Proxy = proxy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la instancia del WebProxy.");
                }
            }

            return handler;
        }
    }
}
