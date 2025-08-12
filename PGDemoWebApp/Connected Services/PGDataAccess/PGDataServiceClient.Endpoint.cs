using System.ServiceModel;
using System.ServiceModel.Description;

namespace PGDataAccess
{
    public partial class PGDataServiceClient
    {
        static partial void ConfigureEndpoint(
            ServiceEndpoint endpoint,
            ClientCredentials creds)
        {
            var cfg = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddJsonFile(
                            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                            optional: true)
                        .AddEnvironmentVariables()
                        .Build();

            var url = cfg["PaymentGatewayApi:PGDataAccess_URL"];
            if (!string.IsNullOrWhiteSpace(url))
            {
                endpoint.Address = new EndpointAddress(url);
            }
        }
    }
}
