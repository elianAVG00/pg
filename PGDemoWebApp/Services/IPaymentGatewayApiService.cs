using PGDemoWebApp.Models;

namespace PGDemoWebApp.Services
{
    public interface IPaymentGatewayApiService
    {
        Task<List<ServicesModel>> GetMerchantsAsync();
        Task<List<ProductModel>> GetProductsAsync(string merchantId);
        Task<string> PostPaymentAsync(PaymentInputModel paymentInput); // Devuelve el HTML de redirect
        Task<TransactionResultModel?> GetTransactionAsync(string transactionId);
        Task<bool> CheckCommerceItemValidated(int serviceId);
        Task<bool> CheckServiceReturnCallbackItems(int serviceId);
        Task<TransactionInfoEpcModel?> GetTransactionInfoByEpcAsync(string merchantId, string epc);
    }
}
