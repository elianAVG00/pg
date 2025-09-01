using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using PGSyncro.EFData;
using PGSyncro.Models;
using PGSyncro.Repositories;
using PGSyncro.Utils;
using PGSyncro.Validators;
using System;
using System.IO;

namespace PGSyncro
{
    public class Program
    {
        internal static bool witherror;
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static bool log_output;
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();          
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));

                    services.AddDbContext<PaymentGatewayEntities>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    services.AddScoped<TransactionRepository>();
                    services.AddScoped<SPSRepository>();
                    services.AddScoped<PGConfiguration>();
                    services.AddScoped<SyncroTransaction>();
                    services.AddScoped<NetworkTools>();
                    services.AddScoped<ConversionTools>();
                    services.AddScoped<SPSCalls>();
                    services.AddHostedService<SyncroWorker>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    logging.AddNLog();
                });

        public class AppSettings
        {
            public bool log_output { get; set; }
            public int SPSTimeOutInMinutes { get; set; }
            public int NPSTimeOutInMinutes { get; set; }
            public string SPSBatchNumberFixed { get; set; }
            public bool ActivateSPSBatchNumberFixed { get; set; }
            public string FixedSPSBatchBeginDate { get; set; }
            public string FixHistoricalSyncro { get; set; }
            public string SPS_API_BASE { get; set; }
            public string SPS_API_TRANSACTION { get; set; }
        }
    }
}