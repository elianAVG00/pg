using Microsoft.Extensions.Options;
using PGDataAccess;
using PGDemoWebApp.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ProductModel = PGDemoWebApp.Models.ProductModel;

namespace PGDemoWebApp.Services
{
    public class PaymentGatewayApiService : IPaymentGatewayApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<PaymentGatewayApiService> _logger;

        public PaymentGatewayApiService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<PaymentGatewayApiService> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_apiSettings.PaymentGateway_URL ?? throw new ArgumentNullException("BaseUrl no configurada"));

            // Configurar Basic Auth para todas las llamadas
            if (string.IsNullOrWhiteSpace(_apiSettings.Username) || string.IsNullOrWhiteSpace(_apiSettings.Password))
            {
                _logger.LogError("ApiSettings:Username o ApiSettings:Password no están configurados para Basic Auth.");
            }
            else
            {
                var plainTextBytes = Encoding.UTF8.GetBytes($"{_apiSettings.Username}:{_apiSettings.Password}");
                string base64EncodedCredentials = Convert.ToBase64String(plainTextBytes);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedCredentials);
                _logger.LogInformation("HttpClient configurado con Basic Authentication para el usuario: {Username}", _apiSettings.Username);
            }
        }

        public async Task<List<ServicesModel>> GetMerchantsAsync()
        {
            await using var wcfClient = new PGDataServiceClient();
            try
            {
                ServiceModel[] wcfServices = await wcfClient.GetServicesByUserAsync(1);
                await wcfClient.CloseAsync();

                var merchantList = new List<ServicesModel>();
                if (wcfServices != null)
                {
                    foreach (var wcfService in wcfServices)
                    {
                        merchantList.Add(new ServicesModel
                        {
                            Id = wcfService.MerchantId,
                            Name = wcfService.Name,
                            ServiceId = wcfService.ServiceId
                        });
                    }
                }
                //BUSCAR COMO LLENAR EL BARCODE
                _logger.LogInformation("Llamada WCF para traer los services completada. {Count} merchants obtenidos.", merchantList.Count);
                return merchantList;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error durante la llamada WCF a GetServicesByUserAsync.");
                if (wcfClient.State != System.ServiceModel.CommunicationState.Closed)
                {
                    wcfClient.Abort();
                }
                throw;
            }
        }

        public async Task<bool> CheckCommerceItemValidated(int serviceId)
        {
            bool isValidationRequired = false;
            await using var wcfClient = new PGDataServiceClient();
            try
            {
                var serviceConfig = await wcfClient.GetServiceConfigByServiceIdAsync(serviceId);
                await wcfClient.CloseAsync();
                if (serviceConfig != null)
                {
                    isValidationRequired = serviceConfig.IsCommerceItemValidated;
                }
                return isValidationRequired;
            }
            catch (Exception ex)
            {
                wcfClient.Abort();
                throw;
            }

        }

        public async Task<bool> CheckServiceReturnCallbackItems(int serviceId)
        {
            bool isReturningCallback = false;
            await using var wcfClient = new PGDataServiceClient();

            try
            {
                var serviceConfig = await wcfClient.GetServiceConfigByServiceIdAsync(serviceId);

                if (serviceConfig != null)
                {
                    isReturningCallback = serviceConfig.IsCallbackPosted;
                }
                return isReturningCallback;
            }
            catch (Exception)
            {
                wcfClient.Abort();
                throw;
            }

        }

        public async Task<List<ProductModel>> GetProductsAsync(string merchantId)
        {
            if (string.IsNullOrEmpty(merchantId))
            {
                _logger.LogWarning("GetProductsAsync llamado sin merchantId.");
                return [];
            }

            var getProductsEndpoint = $"{_apiSettings.Endpoints?.Products}/{merchantId}";
            try
            {
                var response = await _httpClient.GetAsync(getProductsEndpoint);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener products desde la API.");
                throw;
            }
        }

        public async Task<string> PostPaymentAsync(PaymentInputModel paymentInput)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(paymentInput);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var postPaymentEndpoint = $"{_apiSettings.PaymentGateway_URL}{_apiSettings.Endpoints?.Payment}";
                var response = await _httpClient.PostAsync(postPaymentEndpoint, httpContent);

                if (response.IsSuccessStatusCode) // 2xx
                {
                    return await response.Content.ReadAsStringAsync(); // Este es el HTML de redirect
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en POST /payment. Status: {StatusCode}, Content: {ErrorContent}", response.StatusCode, errorContent);
                    // Podrías intentar deserializar 'errorContent' si esperas un JSON de error
                    throw new HttpRequestException($"Error en POST /payment: {response.StatusCode}. Detalles: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al realizar el POST /payment.");
                throw;
            }
        }

        public async Task<TransactionResultModel?> GetTransactionAsync(string transactionId)
        {
            try
            {
                var endpoint = $"{_apiSettings.PaymentGateway_URL}{_apiSettings.Endpoints?.Transaction}/{transactionId}";
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TransactionResultModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener la transacción {TransactionId} desde la API.", transactionId);
                // Considera devolver null y que la UI muestre un error amigable
                return null;
            }
        }

        public async Task<TransactionInfoEpcModel?> GetTransactionInfoByEpcAsync(string merchantId, string epc)
        {
            if (string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(epc))
            {
                _logger.LogWarning("GetTransactionInfoByEpcAsync llamado con merchantId o epc nulos/vacíos.");
                return null;
            }

            var getTransacionInfoEpcEndpoint = $"{_apiSettings.Endpoints?.Transaction}/{merchantId}/{epc}";
            try
            {
                var response = await _httpClient.GetAsync(getTransacionInfoEpcEndpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("Respuesta de GetTransactionInfoByEpcAsync (raw): {Content}", content);
                    return JsonSerializer.Deserialize<TransactionInfoEpcModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Transacción no encontrada por EPC (404). MerchantId: {MerchantId}, EPC: {Epc}, Endpoint: {Endpoint}", merchantId, epc, getTransacionInfoEpcEndpoint);
                    return null;
                }
                else
                {
                    _logger.LogError("Error al obtener transacción por EPC. Status: {StatusCode}, Content: {ErrorContent}, Endpoint: {Endpoint}",
                                     response.StatusCode, content, getTransacionInfoEpcEndpoint);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción en GetTransactionInfoByEpcAsync. MerchantId: {MerchantId}, EPC: {Epc}, Endpoint: {Endpoint}", merchantId, epc, getTransacionInfoEpcEndpoint);
                return null;
            }
        }
    }

    public class ApiSettings
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PaymentGateway_URL { get; set; }
        public string? PGDataAccess_URL { get; set; }
        public EndpointsSettings? Endpoints { get; set; }
    }

    public class EndpointsSettings
    {
        public string? Merchants { get; set; }
        public string? Products { get; set; }
        public string? Payment { get; set; }
        public string? Transaction { get; set; }
    }
}