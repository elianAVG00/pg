using Microsoft.Extensions.Configuration;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using PGExporter.Tools;
using System.Text;

namespace PGExporter
{
    public class Application(ILogRepository _logrepository, IMailerRepository _mailerRepository, IConfigurationRepository _configRepository, IFileRepository _filerepository) : IApplication
    {
        public async Task Run()
        {
            try
            {
                Program.nomail = Program.parameters.ContainsIgnoreCase("nomail");
                Program.auto = Program.parameters.ContainsIgnoreCase("auto");
                Program.totalTime.Start();

                if (!Program.auto)
                    this.InitConsole();

                await _logrepository._print(Program.appversion + " (Misc. Production Support Tools)");
                await _logrepository._print("by Leandro Tupone Bardelli\n");

                if (Program.parameters.Length == 0)
                    this.PrintAyuda();

                else if (Program.parameters.ContainsIgnoreCase("all"))
                {
                    await CrearReporteria(_configRepository, true, true, true);
                }
                else
                {
                    if (Program.parameters.ContainsIgnoreCase("regen"))
                    {
                        Program.nomail = true;
                        Program.regen = true;
                        Program.regenid = Convert.ToInt64(Program.parameters[1]);
                        Program.regensid = Convert.ToInt64(Program.parameters[2]);
                    }
                    bool centra = Program.parameters.ContainsIgnoreCase("-centralizador");
                    bool conci = Program.parameters.ContainsIgnoreCase("-conciliador");
                    bool rendi = Program.parameters.ContainsIgnoreCase("-rendicion");
                    if (!centra && !conci && !rendi)
                        await _logrepository._print("ARGUMENTOS NO RECONOCIDOS\n");
                    else
                        await this.CrearReporteria(_configRepository, centra, conci, rendi);
                }
                if (!Program.auto)
                    this.EndConsole();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.Application.Run", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
        }

        public async Task CrearReporteria(IConfigurationRepository _configRepository, bool centralizador = false, bool conciliador = false, bool rendicion = false)
        {
            Program.clock.Start();
            try
            {
                Program._configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();

                List<Configurations> configurationsList = await _configRepository.GetConfigurations();
                List<Channels> channelsList = await _configRepository.GetChannels();
                List<Products> productsList = await _configRepository.GetProducts();
                List<ServicesConfig> servicesConfigList = await _configRepository.GetServicesConfigurations();
                List<ProductCentralizer> productCentralizerList = await _configRepository.GetProductsCentralizer();

                Program.configurations = configurationsList;
                Program.filenameToDownload = _filerepository.SetFolder();
                Program.channels = channelsList;
                Program.products = productsList;
                Program.serviceConfigurations = servicesConfigList;
                Program.productsCentralizer = productCentralizerList;

                Program.ActivateSPSBatchNumberFixed = (await _configRepository.GetConfiguration("ActivateSPSBatchNumberFixed")).Equals("1");
                Program.SPSBatchNumberFixed = await _configRepository.GetConfiguration("SPSBatchNumberFixed");
                //Program.ExportStatusMail = await _configRepository.GetConfiguration("ExportStatusMail");
                Program.ExportStatusMail = "evgonzalez@provincianet.com.ar";

                if (Program.regen)
                {
                    await _logrepository._print("Creando reporte " + Program.regenid.ToString(), "OK");
                    ReGen? reGen = await _configRepository.GetProcess(Program.regenid);
                    if (reGen == null)
                        return;
                    centralizador = reGen.MonitorFilesReportProcess.Type == 1;
                    conciliador = reGen.MonitorFilesReportProcess.Type == 2;
                    rendicion = reGen.MonitorFilesReportProcess.Type == 3;
                    reGen = null;
                }
                if (centralizador)
                {
                    await _logrepository._print("Creando reporte centralizador...", "OK");
                    if (await _filerepository.CreateCentralizerFile())
                        await _logrepository._print("REPORTE CREADO", "OK");
                    else
                        await _logrepository._print("FALLO EN REPORTE", "NO");
                }
                if (conciliador)
                {
                    await _logrepository._print("Creando reporte conciliador...", "OK");
                    if (await _filerepository.CreateConciliationFile())
                        await _logrepository._print("REPORTE CREADO", "OK");
                    else
                        await _logrepository._print("FALLO EN REPORTE", "NO");
                }
                if (rendicion)
                {
                    await _logrepository._print("Creando reportes de rendición...", "OK");
                    Program.services2rendition = await _configRepository.GetServicesToRendition();

                    bool renditionFiles = await _filerepository.CreateRenditionFiles();
                    if (!renditionFiles)
                        await _logrepository._print("REPORTE CREADO", "OK");
                    else
                        await _logrepository._print("FALLO EN REPORTE", "NO");
                }
                Program.clock.Stop();
                await _logrepository._print("Total de tiempo transcurrido: " + Program.clock.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
                if (Program.log_output)
                    await _logrepository.InsertLog("Debug", "PGExporter.Application", "END S02");
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.Application.CrearReporteria", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
            if (!Program.nomail)
            {
                int num = await _mailerRepository.SendStatus(Program._configuration, Program.ExportStatusMail, Program.StatusMailBody) ? 1 : 0;
            }
            if (Program.log_output)
                await _logrepository.InsertLog("Debug", "PGExporter.Application", "END S00");
            Environment.Exit(0);
        }

        public void PrintAyuda()
        {
            _logrepository._print("Herramienta de exportación de registros de Payment Gateway.");
            _logrepository._print("Considere el trabajo con archivos como LEGACY. UTILICE APIs\n");
            _logrepository._print("MANUAL (1) - Todos los archivos: PGEXPORTER ALL");
            _logrepository._print("MANUAL (2) - Opcionalmente 1 o más archivos: PGEXPORTER [-CENTRALIZADOR] [-CONCILIADOR] [-RENDICION]");
            _logrepository._print("AUTOMÁTICO (3) - PGEXPORTER AUTO ALL");
            _logrepository._print("AUTOMÁTICO (4) - PGEXPORTER AUTO [-CENTRALIZADOR] [-CONCILIADOR] [-RENDICION]");
            this.EndConsole();
        }

        public void InitConsole()
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void EndConsole()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Environment.Exit(0);
        }
    }
}
