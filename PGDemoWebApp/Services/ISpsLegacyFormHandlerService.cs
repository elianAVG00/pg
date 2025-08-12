using Microsoft.AspNetCore.Mvc;
using PGDemoWebApp.Models;

namespace PGDemoWebApp.Services
{
    public interface ISpsLegacyFormHandlerService
    {
        Task<DecidirFormSetupResult> SetupDecidirFormAsync(string paymentRedirectURL, string refererUrl, IUrlHelper urlHelper);
        Task<HttpResponseMessage> SubmitCardDataAsync(Dictionary<string, string> cardFormData, string refererUrl, string? sessionCookie);
        string NormalizeHtmlPaths(string html);
    }
}
