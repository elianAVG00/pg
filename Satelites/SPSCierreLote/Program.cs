using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SPSCierreLote.EFCore.models;
using SPSCierreLote.Models;
using SPSCierreLote.Repositories;
using SPSCierreLote.Tools;
using System.Globalization;
using System.Reflection;

namespace SPSCierreLote;

/// <summary>
/// Punto de entrada del proceso batch “SPS Cierre de Lote”.
/// </summary>
public static class Program
{
    // -------- 1) Campos de configuración / flags --------
    internal static bool IsFtpMethod { get; private set; }
    internal static bool IsValidationActive { get; private set; }
    internal static bool IsFilenameValidationActive { get; private set; }
    internal static bool SaveHistoricByActualDate { get; private set; }
    internal static bool ActivateBatchNumberFixed { get; private set; }
    internal static bool LogOutputEnabled { get; private set; }
    internal static bool ForceMail { get; private set; }

    internal static string FtpUrl = string.Empty;
    internal static string FtpUser = string.Empty;
    internal static string FtpPassword = string.Empty;
    internal static int FtpPort;

    internal static string FolderInvalid = string.Empty;
    internal static string FolderOrigin = string.Empty;
    internal static string FolderDestiny = string.Empty;

    internal static string BatchStatusMailAddress = string.Empty;
    internal static string BatchStatusMailBody = "<table>";
    internal static string BatchFileExtension = string.Empty;

    internal static DateTime FixedBatchBeginDate;

    // -------- 2) Estado de ejecución / métricas --------
    internal static long MonitorProcessId;
    internal static int FilesFound;
    internal static int FilesRead;
    internal static int TotalRecordsRead;
    internal static int TotalRecordsNotRead;
    internal static bool ExecutionWithErrors;
    internal static bool ProcessSingleTxn;
    internal static long SingleTxnIdPk;
    internal static long SingleTransactionNumber;

    internal static IReadOnlyList<SiteProduct> SiteProducts = Array.Empty<SiteProduct>();

    // Logger NLog (registrado en DI, pero mantenido aquí para compatibilidad con Worker).
    internal static ILogger Logger = null!;

    // Versión embebida en el ensamblado.
    internal static readonly string AppVersion = $"SPSCierreLote v.{Assembly.GetExecutingAssembly().GetName().Version}";

    internal static IServiceProvider ServiceProvider { get; private set; } = null!;
    internal static IConfiguration Configuration { get; private set; } = null!;


    // =======================  MAIN  =======================
    private static async Task Main(string[] args)
    {
        // ---------- Host genérico + configuración ----------
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddNLog();
            })
            .ConfigureServices((hostContext, services) =>
            {
                // conexión leída de appsettings.json o variable de entorno
                services.AddDbContext<PgDbContext>(opts =>
                    opts.UseSqlServer(
                        hostContext.Configuration.GetConnectionString("PaymentGatewaySQL")),
                        contextLifetime: ServiceLifetime.Transient,
                        optionsLifetime: ServiceLifetime.Singleton
                        );
            })
            .Build();

        ServiceProvider = host.Services;
        Configuration = host.Services.GetRequiredService<IConfiguration>();
        Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("SPSCierreLote");
        SiteProducts = AppConfigRepository.GetSitios();

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

        // ---------- Cargar flags de configuración ----------
        LoadConfiguration(Configuration);

        Print(AppVersion, "OK");
        Print("INICIO DE PROCESO SPS BATCH", "OK");

        try
        {
            ParseArguments(args);

            Print(IsFtpMethod ? "MÉTODO DE CONEXIÓN: FTP" : "MÉTODO DE CONEXIÓN: FILESERVER", "OK");

            MonitorProcessId = SPSMonitorDatabase.InitNewProcess();
            Print(MonitorProcessId == 0 ? "REGISTRO DE INICIO DE PROCESO" :
                                          $"REGISTRO DE INICIO DE PROCESO [{MonitorProcessId}]",
                                          MonitorProcessId == 0 ? "ERROR" : "OK");

            // --------------- Ejecución principal ---------------
            Worker.ProcessFiles();

            // --------------- Cierre del proceso ----------------
            SPSMonitorDatabase.CloseNewProcess(
                FilesFound,
                FilesRead,
                TotalRecordsRead,
                TotalRecordsNotRead,
                ExecutionWithErrors);

            if (ActivateBatchNumberFixed && DateTime.Now > FixedBatchBeginDate)
                SPSMonitorDatabase.ForceSetSPSBatch();
            else
                Print("FORCE SPS BATCH EXCEPTUADO POR CONFIGURACION", "NO");
        }
        catch (Exception ex)
        {
            ExecutionWithErrors = true;
            SPSMonitorDatabase.CloseNewProcess(
                FilesFound,
                FilesRead,
                TotalRecordsRead,
                TotalRecordsNotRead,
                true);

            Logger.LogError(ex, "Error F01");
            Console.WriteLine($"FATAL ERROR: {ex.Message}");
        }
        finally
        {
            Print("EOP - FIN DEL PROCESO", "OK");

            if (!string.IsNullOrWhiteSpace(BatchStatusMailAddress) && ForceMail)
            {
                BatchStatusMailBody += "</table>";
                Mailer.SendStatus();          // <- clase existente en tu código legado
            }

            Console.ResetColor();
        }

        await host.StopAsync();
    }

    // ======================================================
    // MÉTODOS PRIVADOS
    // ======================================================

    /// <summary>
    /// Carga todos los parámetros desde appsettings.* o desde AppConfig.
    /// </summary>
    private static void LoadConfiguration(IConfiguration cfg)
    {
        IsFtpMethod = cfg["ftp_method"] == "1";
        LogOutputEnabled = cfg["log_output"] == "1";
        FtpUrl = cfg["ftp_url"] ?? string.Empty;
        FtpUser = cfg["ftp_user"] ?? string.Empty;
        FtpPassword = cfg["ftp_pass"] ?? string.Empty;
        FtpPort = int.Parse(cfg["ftp_port"] ?? "21");

        FolderInvalid = IsFtpMethod ? cfg["ftp_warning"] ?? string.Empty
                                                 : cfg["app_ubicacion_invalidos"] ?? string.Empty;
        FolderOrigin = IsFtpMethod ? cfg["ftp_files"] ?? string.Empty
                                                 : cfg["app_ubicacion"] ?? string.Empty;
        FolderDestiny = IsFtpMethod ? cfg["ftp_historic"] ?? string.Empty
                                                 : cfg["app_ubicacion_historico"] ?? string.Empty;

        // -------- flags leídos desde tabla common.AppConfig --------
        BatchStatusMailAddress = AppConfigRepository.GetConfiguration("SPSBatchStatusMail");
        IsValidationActive = AppConfigRepository.GetConfiguration("ValidateSPSBatchProcess") == "1";
        SaveHistoricByActualDate = AppConfigRepository.GetConfiguration("SPSBatchSaveHistoricByActualDate") == "1";
        ActivateBatchNumberFixed = AppConfigRepository.GetConfiguration("ActivateSPSBatchNumberFixed") == "1";
        FixedBatchBeginDate = DateTime.ParseExact(
                                        AppConfigRepository.GetConfiguration("FixedSPSBatchBeginDate"),
                                        "yyyyMMdd",
                                        CultureInfo.InvariantCulture);

        IsFilenameValidationActive = AppConfigRepository.GetConfiguration("ValidateSPSBatchFilenames") == "1";
        BatchFileExtension = AppConfigRepository.GetConfiguration("SPSBatchFileExtension");
    }

    /// <summary>
    /// Procesa los argumentos de línea de comandos.
    /// </summary>
    private static void ParseArguments(string[] args)
    {
        if (args.Length == 0)
        {
            ForceMail = true;
            return;
        }

        string[] tokens = args[0].Split(':');
        string cmd = tokens[0];

        switch (cmd)
        {
            //         d:20250713
            case "d":      // Procesar solo archivos de una fecha AAAAMMDD
                Worker.ParameterDateFlag = true;
                Worker.ParameterDate = DateTime.ParseExact(tokens[1], "yyyyMMdd", null);
                break;

            //         r:LINEATXT (devuelve JSON de la línea parseada)
            case "r":
                Console.WriteLine(
                    System.Text.Json.JsonSerializer.Serialize(
                        SPSFileRepository.ParseRecord(tokens[1])));
                break;

            //         s:123456789  (procesa una única transacción por PK)
            case "s":
                ProcessSingleTxn = true;
                SingleTxnIdPk = long.Parse(tokens[1]);
                using (var ctx = ServiceProvider.GetRequiredService<PgDbContext>())
                {
                    var tai = ctx.TransactionAdditionalInfo
                                .FirstOrDefault(t => t.TransactionIdPK == SingleTxnIdPk);

                    if (tai is null)
                    {
                        Print($"LA TRANSACCIÓN NO FUE ENCONTRADA - TNIDPK: {SingleTxnIdPk}", "WARNING");
                        break;
                    }

                    SingleTransactionNumber = tai.TransactionNumber;
                }
                break;

            default:
                Print("Los argumentos no son válidos", "ERROR");
                Environment.Exit(1);
                break;
        }
    }

    /// <summary>
    /// Imprime por consola y registra en NLog con formato estándar.
    /// </summary>
    internal static void Print(string message, string result)
    {
        string htmlColor = result switch
        {
            "OK" => "339966",
            "NO" => "ff0000",
            "WARNING" => "ff9900",
            "ERROR" => "ff0000",
            _ => "000000"
        };

        ConsoleColor consoleColor = result switch
        {
            "OK" => ConsoleColor.Green,
            "NO" => ConsoleColor.Red,
            "WARNING" => ConsoleColor.Yellow,
            "ERROR" => ConsoleColor.Red,
            _ => ConsoleColor.White
        };

        Console.ForegroundColor = consoleColor;
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string line = $"[{result.ToUpper()}] [{timeStamp}] {message.ToUpper()}";
        Console.WriteLine(line);
        Console.ResetColor();

        if (LogOutputEnabled)
            Logger.LogInformation(line);

        if (!string.IsNullOrWhiteSpace(BatchStatusMailAddress))
        {
            BatchStatusMailBody +=
                $"<tr><td><strong>[<span style=\"color: #{htmlColor};\">{result.ToUpper()}</span>]</strong></td>" +
                $"<td>{message.ToUpper()}</td></tr>";
        }
    }
}
