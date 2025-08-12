using PGMainService.Manager; 
using PGMainService.Models; 
using PGMainService.PGDataAccess; 
using System;
using System.Collections.Generic;
using System.Globalization; 
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace PGMainService.Controllers
{
    public class SyncController : ApiController
    {
        [AppConfig]
        [Authorize(Roles = "apiSyncronize")]
        [Route("Sync")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Sync - Service Online.")
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [AppConfig]
        [Authorize(Roles = "apiSyncronize")]
        [Route("Sync")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Post(SyncCriteriaModel syncCriteria)
        {
            DateTime fechadesde;
            DateTime fechahasta;

            // Lógica de fechas por defecto y parseo:
            if (syncCriteria != null)
            {
                fechadesde = !string.IsNullOrWhiteSpace(syncCriteria.SearchFrom)
                               ? DateTime.ParseExact(syncCriteria.SearchFrom, "yyyyMMdd", CultureInfo.InvariantCulture) 
                               : DateTime.Today; 
                fechahasta = !string.IsNullOrWhiteSpace(syncCriteria.SearchTo)
                               ? DateTime.ParseExact(syncCriteria.SearchTo, "yyyyMMdd", CultureInfo.InvariantCulture) 
                               : DateTime.Today; 
            }
            else 
            {
                fechadesde = DateTime.Now.AddDays(-1.0);
                fechahasta = fechadesde.AddDays(0.0); 
            }

            // Ajuste de tiempo:
            DateTime desdeABuscar = new DateTime(fechadesde.Year, fechadesde.Month, fechadesde.Day, 0, 0, 0);
            DateTime hastaABuscar = new DateTime(fechahasta.Year, fechahasta.Month, fechahasta.Day, 23, 59, 59);

            using (var dataContext = new PGDataServiceClient())
            {
                var incompleteTransactions = dataContext.GetNATransactions(desdeABuscar, hastaABuscar);

                string urlPluginSPS = $"{WebConfigurationManager.AppSettings["Plugin_SPS_URL"]}/{WebConfigurationManager.AppSettings["PluginMethod_closeOperation"]}";
                string urlPluginNPS = $"{WebConfigurationManager.AppSettings["Plugin_NPS_URL"]}/{WebConfigurationManager.AppSettings["PluginMethod_closeOperation"]}";

                if (incompleteTransactions == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, string.Format("No se pudieron buscar transacciones desincronizadas"));

                if (!incompleteTransactions.Any()) 
                    return Request.CreateResponse(HttpStatusCode.OK, string.Format("No se encontraron transacciones desincronizadas"));


                foreach (var transToComplete in incompleteTransactions) 
                {
                    if (string.IsNullOrWhiteSpace(transToComplete.ValidatorTransactionId))
                    {
                        this.CloseByTimeOut(transToComplete.TransactionIdPK);
                    }
                    else
                    {
                        string urlPlugin = (transToComplete.Validator == "NPS") ? urlPluginNPS : urlPluginSPS;
                        if (this.SyncToPlugin(urlPlugin, transToComplete.TransactionNumber) == "PGPAYMENTSENT") 
                        {
                            this.CloseByTimeOut(transToComplete.TransactionIdPK);
                        }
                    }
                }

                // Refetch después de intentar cerrar/sincronizar
                var afterRecheckTransactions = dataContext.GetNATransactions(desdeABuscar, hastaABuscar);

                var completed = incompleteTransactions
                    .Where(olds => !afterRecheckTransactions.Any(x => x.ValidatorTransactionId == olds.ValidatorTransactionId))
                    .ToList();

                var notCompleted = incompleteTransactions
                    .Join(afterRecheckTransactions,
                          olds => olds.ValidatorTransactionId,
                          news => news.ValidatorTransactionId,
                          (olds, news) => news) 
                    .ToList();

                foreach (var tvn in completed) 
                {
                    dataContext.UpdateTransactionSyncByJob(tvn.TransactionNumber); 
                }

                var responseData = new SyncResponseModel()
                {
                    Synchronized = completed,
                    NotSynchronized = notCompleted
                };

                return new HttpResponseMessage()
                {

                    Content = new StringContent("Sync OK", Encoding.UTF8, "application/json")
                };
            }
        }

        private void CloseByTimeOut(long transactionIdPK)
        {
            using (var dataContext = new PGDataServiceClient())
            {
                var transactionToCheckTime = dataContext.GetTransactionById(transactionIdPK);

                if (transactionToCheckTime == null)
                { 
                    return;
                }

                int minutesToTimeout = Convert.ToInt32(dataContext.GetAppConfiguration(transactionToCheckTime.Validator + "_Timeout"));

                DateTime transactionCreationTime = transactionToCheckTime.CreatedOn; 
                DateTime timeoutLimit = transactionCreationTime.AddMinutes(minutesToTimeout); 
                DateTime currentTime = DateTime.Now; 

                if (currentTime > timeoutLimit)
                {
                    dataContext.UpdateTransactionCloseByTimeOut(transactionToCheckTime.Id);
                }
            }
        }

        private string SyncToPlugin(string urlPlugin, long validatorTransactionId) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Interpolación de cadenas para la URI.
                    client.BaseAddress = new Uri($"{urlPlugin}/{validatorTransactionId}");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        responseContent = responseContent.Replace("\"", ""); // Limpieza de comillas
                        return responseContent == "PGPAYMENTSENT" ? "PGPAYMENTSENT" : ""; 
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception) 
            {
                return "";
            }
        }
    }
}