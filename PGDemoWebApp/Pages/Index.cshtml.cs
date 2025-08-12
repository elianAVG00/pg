using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PGDemoWebApp.Models;
using PGDemoWebApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PGDemoWebApp.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IPaymentGatewayApiService _apiService;
        private readonly ISpsLegacyFormHandlerService _spsFormHandler;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public PaymentInputModel PaymentInput { get; set; } = new PaymentInputModel();

        [BindProperty(SupportsGet = true)]
        [Display(Name = "ID de Transacción")]
        public string? InputTransactionId { get; set; }

        // Propiedades para el callback POST del PGMain
        [BindProperty(SupportsGet = true)] public string? ResponseGenericCode { get; set; }
        [BindProperty(SupportsGet = true)] public string? ResponseGenericMessage { get; set; }
        [BindProperty(SupportsGet = true)] public string? ResponseCode { get; set; }
        [BindProperty(SupportsGet = true)] public string? ResponseMessage { get; set; }
        [BindProperty(SupportsGet = true)] public string? ResponseExtended { get; set; }
        [BindProperty(SupportsGet = true)] public string? ElectronicPaymentCode { get; set; }
        [BindProperty(SupportsGet = true)] public string? TransactionId { get; set; }
        [BindProperty(SupportsGet = true)] public string? MetaData { get; set; }

        public List<SelectListItem> ServicesOptions { get; set; } = [];
        public List<SelectListItem> ProductsOptions { get; set; } = [];
        public List<ServicesModel> FullServicesData { get; set; } = [];

        public string? PaymentRedirectHtml { get; set; }
        public string? TransactionJsonResult { get; set; }
        public string? CallbackMessageToDisplay { get; set; }
        public string? ErrorMessageToDisplay { get; set; }

        public IndexModel(ISpsLegacyFormHandlerService spsFormHandler, IPaymentGatewayApiService apiService, ILogger<IndexModel> logger)
        {
            _spsFormHandler = spsFormHandler;
            _apiService = apiService;
            _logger = logger;
        }

        // Handlers
        public async Task OnGetAsync()
        {
            _logger.LogInformation("OnGetAsync Invocado. QueryString: {QueryString}", Request.QueryString.Value);

            await LoadServiceOptionsAsync();

            bool isProcessingCallbackWithData = !string.IsNullOrEmpty(ResponseCode);//calbackResponseCode
            // Recuperar datos de TempData si existen (para el caso IsCallbackPosted = false)
            var paymentMerchantFromTemp = TempData["PaymentMerchantSent"] as string;
            var paymentEpcFromTemp = TempData["PaymentEPCSent"] as string;
            TempData.Keep("PaymentMerchantSent"); // Mantenerlos por si hay F5
            TempData.Keep("PaymentEPCSent");

            string? transactionIdForAutoQuery = null;

            if (isProcessingCallbackWithData) // Callback con datos (probablemente IsCallbackPosted = true)
            {
                _logger.LogInformation("OnGetAsync: Procesando callback directo con datos.");
                transactionIdForAutoQuery = InputTransactionId;
            }
            else if (!string.IsNullOrEmpty(paymentMerchantFromTemp) && !string.IsNullOrEmpty(paymentEpcFromTemp)) // Callback sin datos directos (usar ep para EPC)
            {
                _logger.LogInformation("OnGetAsync: Procesando callback sin datos directos, usando EPC de TempData. Merchant: {Merchant}, EPC: {EPC}",
                    paymentMerchantFromTemp, paymentEpcFromTemp);
                //chequeo mediante epc y obtengo transactionId
                var transactionInfoByEpc = await _apiService.GetTransactionInfoByEpcAsync(paymentMerchantFromTemp, paymentEpcFromTemp);

                if (transactionInfoByEpc != null && !string.IsNullOrEmpty(transactionInfoByEpc.TransactionId))
                {
                    InputTransactionId = transactionInfoByEpc.TransactionId; // Pre-rellenar campo
                    transactionIdForAutoQuery = InputTransactionId;
                    await QueryAndDisplayTransactionAsync(transactionInfoByEpc.TransactionId);
                }
                else
                {
                    _logger.LogWarning("OnGetAsync: No se pudo obtener TransactionId usando EPC. Merchant: {Merchant}, EPC: {EPC}", paymentMerchantFromTemp, paymentEpcFromTemp);
                    ErrorMessageToDisplay = "No se pudo obtener el estado final de la transacción por EPC.";
                    TransactionJsonResult = "// No se pudo recuperar la transacción usando el EPC proporcionado.";
                }
                TempData.Remove("PaymentMerchantSent"); // Limpiar TempData después de usarlo
                TempData.Remove("PaymentEPCSent");
            }
            else if (!string.IsNullOrEmpty(InputTransactionId) && string.IsNullOrEmpty(TransactionJsonResult) && string.IsNullOrEmpty(CallbackMessageToDisplay)) // Recarga con ID en input, sin resultado JSON ni mensaje de callback previo
            {
                _logger.LogInformation("OnGetAsync: Recarga con InputTransactionId: {TransactionId}. Consultando de nuevo.", InputTransactionId);
                await QueryAndDisplayTransactionAsync(InputTransactionId); // Consulta manual, no es un callback
            }

            // Si obtuvimos un transactionId de alguno de los flujos de callback, procesarlo
            if (!string.IsNullOrEmpty(transactionIdForAutoQuery))
            {
                await ProcessAndDisplayCallbackResultAsync(transactionIdForAutoQuery, this.ResponseMessage, this.ElectronicPaymentCode);
            }

            // Preparar el formulario de "Iniciar un Pago" (EPC, Barcode, Productos si hay MerchantId)
            await PreparePaymentFormInputAsync();
        }

        public IActionResult OnPost() // Handler para los callback que entran
        {
            _logger.LogInformation("Datos del Formulario recibidos en POST:");
            if (Request.HasFormContentType)
            {
                foreach (var key in Request.Form.Keys)
                {
                    _logger.LogInformation("  Form Key: {Key}, Value: {Value}", key, Request.Form[key]);
                }
            }

            return RedirectToPage(new
            {
                InputTransactionId = this.TransactionId,
                ResponseGenericCode = this.ResponseGenericCode,
                ResponseGenericMessage = this.ResponseGenericMessage,
                ResponseCode = this.ResponseCode,
                ResponseMessage = this.ResponseMessage,
                ResponseExtended = this.ResponseExtended,
                ElectronicPaymentCode = this.ElectronicPaymentCode, // El EPC del callback
                CallbackMetaData = this.MetaData
            });
        }

        // Handler para ajax el js
        public async Task<JsonResult> OnGetProductsForMerchantAsync(string merchantId)
        {
            if (string.IsNullOrEmpty(merchantId))
            {
                return new JsonResult(new List<ProductModel>());
            }
            try
            {
                var products = await _apiService.GetProductsAsync(merchantId);
                var selectListData = products.Select(p => new { id = p.Id, name = p.Name }).ToList();
                return new JsonResult(selectListData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OnGetProductsForMerchantAsync para Merchant ID {MerchantId}", merchantId);
                return new JsonResult(new { error = "Error al cargar tarjetas." }) { StatusCode = 500 };
            }
        }

        // Handler para el FORMULARIO DE PAGO
        public async Task<IActionResult> OnPostPaymentAsync()
        {
            // Recargar datos de selects en caso de error de validación y recarga de página
            await LoadServiceOptionsAsync();

            // Genero los campos automáticos
            PaymentInput.ElectronicPaymentCode = "7dgfweg" + new Random().Next(100000000, 999999999).ToString();
            // Asignar BarCode basado en el merchant (ejemplo simple)
            var selectedMerchant = FullServicesData.FirstOrDefault(m => m.Id == PaymentInput.MerchantId);
            PaymentInput.BarCode = selectedMerchant?.BarCode ?? null;

            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(PaymentInput.MerchantId))
                {
                    await LoadProductOptionsAsync(PaymentInput.MerchantId);
                }
                return Page();
            }

            try
            {
                PaymentInput.Payments = 1; //siempre es 1 cuota
                PaymentInput.Channel = "web";
                PaymentInput.IsSimulation = 1;

                // Llamar al método del servicio para verificar la configuración
                bool requiresCommerceItemValidation = await _apiService.CheckCommerceItemValidated(selectedMerchant.ServiceId.Value);

                if (requiresCommerceItemValidation)
                {
                    string amountStringForJson;
                    if (!string.IsNullOrEmpty(PaymentInput.Amount))
                    {
                        amountStringForJson = PaymentInput.Amount.Replace(",", ".");
                        if (!decimal.TryParse(amountStringForJson, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        {
                            _logger.LogWarning("El monto ingresado '{UserAmount}' no es un decimal válido. Usando '0.00' por defecto para CommerceItems.", PaymentInput.Amount);
                            amountStringForJson = "0.00";
                        }
                    }
                    else
                    {
                        _logger.LogWarning("El monto está vacío. Usando '0.00' por defecto para CommerceItems.");
                        amountStringForJson = "0.00";
                    }
                    PaymentInput.CommerceItems = $"[{{\"Code\":\"5500414010000006500021070000000000120572503000000334\",\"Description\":\"Producto Hardcodeado\",\"Amount\":\"{amountStringForJson}\"}}]";
                    PaymentInput.BarCode = "5500414010000006500021070000000000120572503000000334";
                    _logger.LogInformation("CommerceItems hardcodeados para el servicio: {ServiceName} con monto: {Amount}", selectedMerchant.Name, amountStringForJson);
                }
                else
                {
                    PaymentInput.CommerceItems = null;
                }

                // URLs de callback apuntando a esta misma página (Index)
                var callbackBaseUrl = Url.Page("/Index", null, null, Request.Scheme);
                PaymentInput.CallbackUrl = callbackBaseUrl;
                PaymentInput.ValidateEPCreturnUrl = callbackBaseUrl;

                _logger.LogInformation("Iniciando POST /payment con payload: {Payload}", JsonSerializer.Serialize(PaymentInput));
                string initialPaymentFormHtml = await _apiService.PostPaymentAsync(PaymentInput);

                if (!await _apiService.CheckServiceReturnCallbackItems(selectedMerchant.ServiceId.Value))//chequeo si el servicio a pagar devuelve datos (Datos del Formulario recibidos en POST)
                {
                    TempData["PaymentMerchantSent"] = PaymentInput.MerchantId;
                    TempData["PaymentEPCSent"] = PaymentInput.ElectronicPaymentCode;

                }
                else
                {
                    // Limpiar por si acaso quedaron de una ejecución anterior con otro servicio
                    TempData.Remove("PaymentMerchantSent");
                    TempData.Remove("PaymentEPCSent");
                }

                _logger.LogInformation("POST /payment exitoso. HTML de redirección recibido.");

                if (initialPaymentFormHtml.Contains("onload='document.forms[0].submit()'"))
                {
                    string refererUrl = string.Empty;

                    if (selectedMerchant.ServiceId.Value == 27) // SIEP 
                        refererUrl = "https://plataforma-pagos.dev.gba.gob.ar/";
                    else if (selectedMerchant.ServiceId.Value == 21) // Reincidencia --> FALTA CORRECTA URL
                        refererUrl = "https://URLReincidencia/";
                    else if (selectedMerchant.ServiceId.Value == 8) // SACIT --> FALTA CORRECTA URL
                        refererUrl = "https://URLSACIT/";

                    var formResult = await _spsFormHandler.SetupDecidirFormAsync(initialPaymentFormHtml, refererUrl, Url);
                    if (!string.IsNullOrEmpty(formResult.SessionCookieHeader))
                    {
                        HttpContext.Session.SetString("DecidirSessionCookie", formResult.SessionCookieHeader);
                    }

                    PaymentRedirectHtml = formResult.RenderedHtml;
                }
                else
                {
                    PaymentRedirectHtml = initialPaymentFormHtml;
                }

                return Content(PaymentRedirectHtml, "text/html");
            }
            catch (Exception ex)
            {
                ErrorMessageToDisplay = $"Error al procesar el pago: {ex.Message}";
                _logger.LogError(ex, "Error en OnPostPaymentAsync durante el pago.");
                if (!string.IsNullOrEmpty(PaymentInput.MerchantId))
                {
                    await LoadProductOptionsAsync(PaymentInput.MerchantId);
                }
                return Page();
            }
        }

        // Handler para el no superform
        public async Task<IActionResult> OnPostSubmitCardDataAsync()
        {
            _logger.LogInformation("Handler OnPostSubmitCardDataAsync. Interceptando POST a Decidir.");

            string refererUrl = "https://developers.decidir.com/forms/Transaccion";
            string? sessionCookie = HttpContext.Session.GetString("DecidirSessionCookie");

            //obtengo la data del post, data del form tarjeta
            var postData = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());

            var response = await _spsFormHandler.SubmitCardDataAsync(postData, refererUrl, sessionCookie);

            if (response.IsSuccessStatusCode)
            {
                string finalHtml = await response.Content.ReadAsStringAsync();
                return Content(_spsFormHandler.NormalizeHtmlPaths(finalHtml), "text/html");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error al procesar el pago con la tarjeta. Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                await LoadServiceOptionsAsync(); // recargo datos para mostrar todo vacio
                return Page();
            }
        }

        // Handler para consulta transaction
        public async Task<IActionResult> OnPostQueryTransactionAsync()
        {
            // Recargar datos de selects por si se necesita al recargar la página
            await LoadServiceOptionsAsync();
            if (!string.IsNullOrEmpty(PaymentInput.MerchantId))
            {
                await LoadProductOptionsAsync(PaymentInput.MerchantId);
            }

            CallbackMessageToDisplay = null; // Limpiar mensaje de callback
            if (string.IsNullOrWhiteSpace(InputTransactionId))
            {
                ModelState.AddModelError(nameof(InputTransactionId), "Debe ingresar un ID de transacción.");
                ErrorMessageToDisplay = "Debe ingresar un ID de transacción para consultar.";
                return Page();
            }
            await QueryAndDisplayTransactionAsync(InputTransactionId);

            return Page();
        }

        // --- MÉTODOS DE CARGA DE DATOS ---
        private async Task LoadServiceOptionsAsync()
        {
            try
            {
                FullServicesData = await _apiService.GetMerchantsAsync();
                ServicesOptions = FullServicesData.Select(s => new SelectListItem { Value = s.Id, Text = s.Name }).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessageToDisplay = "Error al cargar Servicios/Merchants. Por favor, intente recargar.";
                _logger.LogError(ex, "Fallo al cargar Servicios/Merchants");
                ServicesOptions = [];
                FullServicesData = [];
            }
        }

        private async Task LoadProductOptionsAsync(string? merchantId)
        {
            if (!string.IsNullOrEmpty(merchantId))
            {
                try
                {
                    var productsData = await _apiService.GetProductsAsync(merchantId);
                    ProductsOptions = productsData.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                }
                catch (Exception ex)
                {
                    ErrorMessageToDisplay = $"Error al cargar Tarjetas para el servicio {merchantId}.";
                    _logger.LogError(ex, "Fallo al cargar productos para el merchant {MerchantId} seleccionado en postback", merchantId);
                    ProductsOptions = [];
                }
            }
            else
            {
                ProductsOptions = [];
            }
        }

        private async Task QueryAndDisplayTransactionAsync(string transactionIdToQuery)
        {
            if (string.IsNullOrWhiteSpace(transactionIdToQuery)) return;

            _logger.LogInformation("Consultando transacción ID (automático o manual): {TransactionId}", transactionIdToQuery);

            try
            {
                var transactionData = await _apiService.GetTransactionAsync(transactionIdToQuery);
                if (transactionData != null)
                {
                    var serializerOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    TransactionJsonResult = JsonSerializer.Serialize(transactionData, serializerOptions);
                }
                else
                {
                    TransactionJsonResult = $"// No se encontró la transacción con ID: {transactionIdToQuery} o hubo un error en la consulta.";
                    _logger.LogWarning("No se encontró la transacción o error en consulta para ID: {TransactionId}", transactionIdToQuery);
                }
            }
            catch (Exception ex)
            {
                TransactionJsonResult = $"// Error al consultar la transacción: {ex.Message}";
                _logger.LogError(ex, "Error en ConsultarTransaccionAsync para ID: {TransactionId}", transactionIdToQuery);
            }
        }

        private async Task ProcessAndDisplayCallbackResultAsync(string transactionId, string? responseMessage, string? epcFromCallback)
        {
            InputTransactionId = transactionId;
            await QueryAndDisplayTransactionAsync(transactionId);

            // Formatear el mensaje de callback
            if (!string.IsNullOrEmpty(responseMessage))
            {
                CallbackMessageToDisplay = $"{responseMessage} (ID: {transactionId}, EPC: {epcFromCallback ?? "N/A"})";
                if (responseMessage.ToLower().Contains("aprobada"))
                {
                    _logger.LogInformation("Email de comprobante (simulado) para TX: {TransactionId}", transactionId);
                }
            }
            else if (!string.IsNullOrEmpty(TransactionJsonResult))//o chequear TransactionJsonResult.StartsWith("// Error") si devuelve algo como error
            {
                // Si no hubo ResponseMessage directo pero la consulta fue exitosa, generar un mensaje genérico
                CallbackMessageToDisplay = $"Transacción (ID: {transactionId}) procesada. Detalles abajo.";
            }
            else if (string.IsNullOrEmpty(ErrorMessageToDisplay)) // Solo si no hay un error más específico ya
            {
                CallbackMessageToDisplay = $"Callback recibido para TX ID: {transactionId}. Detalles abajo.";
            }
        }

        private async Task PreparePaymentFormInputAsync()
        {
            // Generar EPC si no existe o si el actual no es del callback, evita que el EPC del callback sobreescriba el del formulario si el usuario quiere hacer un nuevo pago
            if (PaymentInput.ElectronicPaymentCode == null ||
                (!string.IsNullOrEmpty(this.ElectronicPaymentCode) && PaymentInput.ElectronicPaymentCode == this.ElectronicPaymentCode && string.IsNullOrEmpty(this.ResponseCode)))
            {
                // si el form EPC está vacío, genera uno nuevo.
                if (string.IsNullOrEmpty(PaymentInput.ElectronicPaymentCode))
                    PaymentInput.ElectronicPaymentCode = "7dgfweg" + new Random().Next(100000000, 999999999).ToString();
            }

            if (string.IsNullOrEmpty(PaymentInput.BarCode) && FullServicesData.Any())
            {
                var serviceForBarcode = FullServicesData.FirstOrDefault(s => s.Id == PaymentInput.MerchantId) ?? FullServicesData.FirstOrDefault();
                PaymentInput.BarCode = serviceForBarcode?.BarCode ?? $"DEF_BAR_{DateTime.Now.Ticks % 1000000}";
            }

            // Cargar productos si ya hay un servicio seleccionado en PaymentInput (ej. error de validación en POST) y la lista de opciones de producto está vacía (para no recargar innecesariamente en F5)
            if (!string.IsNullOrEmpty(PaymentInput.MerchantId) && !ProductsOptions.Any())
            {
                await LoadProductOptionsAsync(PaymentInput.MerchantId);
            }
        }
    }
}

