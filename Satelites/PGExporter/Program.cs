using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using PGExporter.Repositories;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace PGExporter
{
    internal class Program
    {
        internal static string[] parameters;
        internal static string appversion = "PGExporter v." + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static Stopwatch totalTime = new Stopwatch();
        public static bool auto = false;
        public static string userName = WindowsIdentity.GetCurrent().Name;
        public static IConfiguration _configuration;
        public static bool FTPMethod = false;
        public static bool log_output = true;
        public static List<Configurations> configurations = new List<Configurations>();
        public static List<Channels> channels = new List<Channels>();
        public static List<Products> products = new List<Products>();
        public static List<ServicesConfig> serviceConfigurations = new List<ServicesConfig>();
        public static List<ProductCentralizer> productsCentralizer = new List<ProductCentralizer>();
        public static List<ServiceModel> services2rendition = new List<ServiceModel>();
        public static int DatabaseTimeOut = 3600;
        public static string filenameToDownload = "";
        public static long ProcessId = 0;
        public static int ChunkSize = 500;
        public static string? ExportStatusMail = null;
        public static string StatusMailBody = "<table style=\"font-family:Arial; font-size:1em;\">";
        public static bool ActivateSPSBatchNumberFixed = false;
        public static string? SPSBatchNumberFixed = null;
        public static Stopwatch clock = new Stopwatch();
        public static bool regen = false;
        public static long regenid = 0;
        public static long regensid = 0;
        public static bool nomail = false;

        private static void Main(string[] args)
        {
            Program.parameters = args;
            HostApplicationBuilder applicationBuilder = Host.CreateApplicationBuilder(args);
            Program.ChunkSize = applicationBuilder.Configuration.GetValue<int>("ChunkSize");
            Program.log_output = applicationBuilder.Configuration.GetValue<bool>("LogOutput");
            Program.FTPMethod = applicationBuilder.Configuration.GetSection("FTP").GetValue<bool>("IsOn");
            Program.DatabaseTimeOut = applicationBuilder.Configuration.GetValue<int>("DatabaseTimeOut");
            string PaymentGatewayConnection = applicationBuilder.Configuration.GetConnectionString("PaymentGateway");
            applicationBuilder.Services.AddDbContext<PaymentContext>(options => options.UseSqlServer(PaymentGatewayConnection, o => o.UseCompatibilityLevel(110)));
            applicationBuilder.Services.AddScoped<ILogRepository, LogRepository>();
            applicationBuilder.Services.AddScoped<IProcessRepository, ProcessRepository>();
            applicationBuilder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            applicationBuilder.Services.AddScoped<ICentralizerRepository, CentralizerRepository>();
            applicationBuilder.Services.AddScoped<IConciliationRepository, ConciliationRepository>();
            applicationBuilder.Services.AddScoped<IRenditionRepository, RenditionRepository>();
            applicationBuilder.Services.AddScoped<IFileRepository, FileRepository>();
            applicationBuilder.Services.AddScoped<IMailerRepository, MailerRepository>();
            applicationBuilder.Services.AddSingleton<IApplication, Application>();
            applicationBuilder.Build().Services.GetRequiredService<IApplication>().Run().Wait();
        }
    }
}