using PGExporter.Interfaces;
using PGExporter.Models;
using PGExporter.Tools;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PGExporter.Repositories
{
    public class FileRepository(ILogRepository _logrepository, IConfigurationRepository _configurationRepository, IConciliationRepository _conciliationrepository,
                                ICentralizerRepository _centralizerRepository, IRenditionRepository _renditionrepository, IProcessRepository _processRepository) : IFileRepository
    {
        public string SetFolder()
        {
            return !Program.FTPMethod ? Program._configuration.GetSection("ReportPath")["Path"] + DateTime.Now.ToString("\\\\yyyy\\\\MM\\\\dd\\\\") :
                Program._configuration.GetSection("FTP")["Directory"] + DateTime.Now.ToString("/yyyy/MM/dd/");
        }

        public string AddProcessIdToFolder(long id, string type)
        {
            return !Program.FTPMethod ? Program.filenameToDownload + id.ToString() + " - " + type + "\\\\" : Program.filenameToDownload + id.ToString() + " - " + type + "/";
        }

        public async Task<bool> CreateCentralizerFile()
        {
            Stopwatch centralizerSW = new Stopwatch();
            centralizerSW.Start();
            bool privateError = false;
            bool getRecords = false;
            string filename = await _configurationRepository.GetConfiguration("814FileNameHeader") + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            List<Records> records = new List<Records>();
            long monId = 0;
            try
            {
                List<string> content = new List<string>();
                monId = await _centralizerRepository.GetCentralizerReportId();
                string destinationFile = AddProcessIdToFolder(monId, "CENTRALIZER");
                List<CentralizerReportModel> centralizerData = await _centralizerRepository.GetCentralizerData(monId);
                getRecords = true;
                foreach (CentralizerReportModel lineToReport in centralizerData)
                {
                    content.Add(await this.Model814ToLine(lineToReport));

                    records.Add(new Records()
                    {
                        TransctionIDPK = lineToReport.IdPK,
                        Amount = lineToReport.Importe,
                        Incomplete = false,
                        Informed = true,
                        MonitorFilesReportRecordsId = lineToReport.MonitorFilesReportRecordsId
                    });
                }
                if (!Program.FTPMethod)
                    await CopyFile(content.Distinct().ToArray(), destinationFile, filename);
                else
                    privateError = !FTPConnector.UploadFile(destinationFile, filename, string.Join(Environment.NewLine, content));
            }
            catch (Exception ex)
            {
                privateError = true;
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.CreateCentralizerFile", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
            if (privateError & getRecords)
            {
                await _processRepository.RollBackProcess();
            }
            await _processRepository.CloseProcess(monId, Program.filenameToDownload + filename, records, privateError);
            await _logrepository._print(string.Format("CENTRALIZADOR - Procesados {0} registros en: {1} ", records.Count, centralizerSW.Elapsed.ToString("hh\\:mm\\:ss\\.fff")));
            centralizerSW.Stop();
            bool centralizerFile = !privateError;

            return centralizerFile;
        }

        public async Task<bool> CreateRenditionFiles()
        {
            bool someError = false;
            try
            {
                if (!Program.regen)
                {
                    foreach (ServiceModel service in Program.services2rendition)
                    {
                        if (!await CreateRenditionFile(service.ServiceId, service.Name))
                            someError = true;
                    }
                }
                else
                {
                    ServiceModel serviceRegen = Program.services2rendition.FirstOrDefault(sr => sr.ServiceId == Program.regensid);
                    if (serviceRegen == null)
                        return true;

                    if (!await CreateRenditionFile(serviceRegen.ServiceId, serviceRegen.Name))
                        someError = true;
                }
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.CreateRenditionFiles", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "");
            }
            return someError;
        }

        public async Task<bool> CreateRenditionFile(long serviceId, string serviceName)
        {
            bool privateError = false;
            bool getRecords = false;
            List<Records> records = new List<Records>();
            long monId = 0;
            string filename = string.Format($"{0}_{1}_{2}.csv", await _configurationRepository.GetConfiguration("RenditionFileNameHeader") +
                                    DateTime.Now.ToString("yyyyMMdd") + serviceName);
            try
            {
                List<string> content = new List<string>();
                string destinationFile = Program.filenameToDownload + "RENDITION" + "\\\\";
                if (Program.regen)
                {
                    destinationFile = destinationFile + "_" + DateTime.Now.ToString("yyyyMMdd.hhmmss.fff") + "\\\\";
                    monId = Program.regenid;
                }
                else
                    monId = await _renditionrepository.GetRenditionReportId(serviceId);
                List<RenditionReportModel> ListadoRendicion = await _renditionrepository.GetRenditionData(monId);
                getRecords = true;
                string header = "ElectronicPaymentCode;" + "TrxID;" + "TrxSource;" + "BatchNbr;" + "CustomerId;" + "CustomerMail;" + "Producto;" + "Country;" + "Currency;" + "Amount;" + "CardHolderName;" + "CardMask;" + "MerchantId;" + "MerchOrderId;" + "MerchTrxRef;" + "NumPayments;" + "Operation;" + "PosDateTime;" + "Code;" + "Description;" + "Price;";
                content.Add(header);
                foreach (RenditionReportModel lineToReport in ListadoRendicion)
                {
                    if (lineToReport.IsIncomplete)
                    {
                        records.Add(new Records()
                        {
                            TransctionIDPK = lineToReport.IDPK,
                            Amount = lineToReport.Amount,
                            Incomplete = lineToReport.IsIncomplete,
                            Informed = false,
                            MonitorFilesReportRecordsId = lineToReport.MonitorFilesReportRecordsId
                        });
                    }
                    else
                    {
                        records.Add(new Records()
                        {
                            TransctionIDPK = lineToReport.IDPK,
                            Amount = lineToReport.Amount,
                            Incomplete = lineToReport.IsIncomplete,
                            Informed = true,
                            MonitorFilesReportRecordsId = lineToReport.MonitorFilesReportRecordsId
                        });

                        content.Add(await ModelRenditionToLine(lineToReport));
                    }
                }
                if (!Program.FTPMethod)
                    await CopyFile(content.Distinct().ToArray(), destinationFile, filename);
                else
                    privateError = !FTPConnector.UploadFile(destinationFile, filename, string.Join(Environment.NewLine, content));
                if (!Program.regen)
                    await _processRepository.CloseProcess(monId, Program.filenameToDownload + filename, records, privateError);
                if (Program.log_output)
                    await _logrepository.InsertLog("Debug", "PGExporter.Application", "END S03");
                return !privateError;
            }
            catch (Exception ex)
            {
                privateError = true;
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.CreateRenditionFile", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "");
            }
            if (Program.log_output)
                await _logrepository.InsertLog("Debug", "PGExporter.Application", "END S04");
            if (privateError & getRecords)
            {
                int num = await _processRepository.RollBackProcess() ? 1 : 0;
            }
            if (Program.log_output)
                await _logrepository.InsertLog("Debug", "PGExporter.Application", "END S05");
            return privateError;
        }

        public async Task<bool> CreateConciliationFile()
        {
            Stopwatch conciliadorSW = new Stopwatch();
            conciliadorSW.Start();
            bool privateError = false;
            bool getRecords = false;
            List<Records> records = new List<Records>();
            long monid = 0;
            string filename = await _configurationRepository.GetConfiguration("ConciliationFileNameHeader") + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            try
            {
                List<string> content = new List<string>();
                string regents = "";
                if (Program.regen)
                {
                    monid = Program.regenid;
                    regents = "_" + DateTime.Now.ToString("yyyyMMdd.hhmmss.fff");
                }
                else
                    monid = await _conciliationrepository.GetConciliationReportId();
                string destinationFile = AddProcessIdToFolder(monid, "CONCILIATION" + regents);
                List<ConciliationReportModel> ListadoConciliador = await _conciliationrepository.GetConciliationData(monid);
                getRecords = true;
                foreach (ConciliationReportModel lineToReport in ListadoConciliador)
                {
                    if (lineToReport.IsIncomplete)
                    {
                        records.Add(new Records()
                        {
                            TransctionIDPK = lineToReport.IdPK,
                            Amount = lineToReport.Importe,
                            Incomplete = lineToReport.IsIncomplete,
                            Informed = false,
                            MonitorFilesReportRecordsId = lineToReport.MonitorFilesReportRecordsId
                        });
                    }
                    else
                    {
                        records.Add(new Records()
                        {
                            TransctionIDPK = lineToReport.IdPK,
                            Amount = lineToReport.Importe,
                            Incomplete = lineToReport.IsIncomplete,
                            Informed = true,
                            MonitorFilesReportRecordsId = lineToReport.MonitorFilesReportRecordsId
                        });
                        content.Add(await ModelConciliationToLine(lineToReport));
                    }
                }
                if (!Program.FTPMethod)
                    await this.CopyFile(content.Distinct().ToArray(), destinationFile, filename);
                else
                    privateError = !FTPConnector.UploadFile(destinationFile, filename, string.Join(Environment.NewLine, content));
            }
            catch (Exception ex)
            {
                privateError = true;
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.CreateConciliationFile", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "");
            }
            if (privateError & getRecords)
            {
                int num = await _processRepository.RollBackProcess() ? 1 : 0;
            }
            if (!Program.regen)
                await _processRepository.CloseProcess(monid, Program.filenameToDownload + filename, records, privateError);
            await _logrepository._print(string.Format("CONCILIADOR - Procesados {0} registros en: {1}", (object)records.Count, (object)conciliadorSW.Elapsed.ToString("hh\\:mm\\:ss\\.fff")));
            conciliadorSW.Stop();
            bool conciliationFile = !privateError;
            return conciliationFile;
        }

        public async Task CopyFile(string[] content, string directory, string filename)
        {
            try
            {
                await _logrepository._print("Generando el archivo: " + directory + filename, "OK");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    await _logrepository._print("Directorio '" + directory + "' creado.", "OK");
                }
                if (!File.Exists(directory + filename))
                    File.WriteAllLines(directory + filename, content, Encoding.Default);
                await _logrepository._print("Archivo '" + filename + "' creado.", "OK");
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.CopyFile", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<string> ModelRenditionToLine(RenditionReportModel LineaAEscribir)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                string retorno = string.Empty;
                retorno = retorno + LineaAEscribir.ElectronicPaymentCode + ";";
                retorno = retorno + LineaAEscribir.TrxID.ToString() + ";";
                retorno = retorno + LineaAEscribir.TrxSource + ";";
                retorno = retorno + LineaAEscribir.BatchNbr + ";";
                retorno = retorno + LineaAEscribir.CustomerId + ";";
                retorno = retorno + LineaAEscribir.CustomerMail + ";";
                retorno = retorno + LineaAEscribir.Producto + ";";
                retorno = retorno + LineaAEscribir.Country + ";";
                retorno = retorno + LineaAEscribir.Currency.ToString() + ";";
                retorno = retorno + LineaAEscribir.Amount.ToString("0.00", (IFormatProvider)provider) + ";";
                retorno = !string.IsNullOrWhiteSpace(LineaAEscribir.CardHolderName) ? retorno + string.Concat(LineaAEscribir.CardHolderName.ToUpper().Where((c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))) + ";" : retorno + ";";
                retorno = retorno + LineaAEscribir.CardMask.ToUpper() + ";";
                retorno = retorno + LineaAEscribir.MerchantId + ";";
                retorno = retorno + LineaAEscribir.MerchOrderId + ";";
                retorno = retorno + LineaAEscribir.MerchTrxRef + ";";
                retorno = retorno + LineaAEscribir.NumPayments.ToString() + ";";
                retorno = retorno + LineaAEscribir.Operation + ";";
                retorno = retorno + LineaAEscribir.PosDateTime.ToString("M/d/yyyy hh:mm:ss tt", provider) + ";";
                retorno = retorno + LineaAEscribir.Code + ";";
                retorno = retorno + LineaAEscribir.Description + ";";
                retorno = retorno + LineaAEscribir.Price.ToString("0.00", provider) + ";";
                return retorno;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.ModelRenditionToLine", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<string> Model814ToLine(CentralizerReportModel LineaAEscribir)
        {
            try
            {
                string retorno = string.Empty;
                retorno += LineaAEscribir.Datos;
                retorno += LineaAEscribir.Spacer1;
                retorno += LineaAEscribir.CodigoBCRA;
                retorno += LineaAEscribir.CodigoR;
                string str1 = retorno;
                int? codigoTerminal = LineaAEscribir.CodigoTerminal;
                retorno += await GetFormattedNumber(codigoTerminal.ToString(), '0', 5);
                retorno += LineaAEscribir.Spacer2;
                int? codigoSucursal = LineaAEscribir.CodigoSucursal;
                retorno += await this.GetFormattedNumber(codigoSucursal.ToString(), '0', 4);
                retorno += LineaAEscribir.NroSecuenciaOn;
                retorno += await this.GetFormattedNumber(LineaAEscribir.NroSecuenciaTrans.ToString(), '0', 8);
                retorno += LineaAEscribir.CodigoOperacion;
                retorno += LineaAEscribir.Desde;
                retorno += LineaAEscribir.Hasta;
                retorno += LineaAEscribir.CodigoEnte;
                retorno += await this.GetFormattedString(LineaAEscribir.CodigoServicio.ToString(), ' ', 19);
                retorno += await GetIntegerFromDecimal(LineaAEscribir.Importe, '0', 11);
                retorno += LineaAEscribir.Interes;
                retorno += LineaAEscribir.Recargo;
                retorno += LineaAEscribir.Moneda;
                retorno += LineaAEscribir.CodigoCajero;
                retorno += LineaAEscribir.FondoEducativo;
                retorno += LineaAEscribir.Spacer3;
                retorno += LineaAEscribir.CodigoSeguridad;
                retorno += LineaAEscribir.FechaVto1;
                retorno += LineaAEscribir.FechaVto2;
                retorno += LineaAEscribir.BancoCheque;
                retorno += LineaAEscribir.BancoSucursal;
                retorno += LineaAEscribir.BancoCodPostal;
                retorno += LineaAEscribir.NroCheque;
                retorno += LineaAEscribir.NroCuenta;
                retorno += LineaAEscribir.Plazo;
                retorno += await this.GetFormattedString(LineaAEscribir.CodigoBarra, ' ', 60);
                retorno += LineaAEscribir.FechaPago.ToString("yyMMdd");
                retorno += LineaAEscribir.Spacer4;
                retorno += LineaAEscribir.AnioCuota;
                retorno += LineaAEscribir.FoProvi;
                retorno += LineaAEscribir.FormaPago;
                retorno += LineaAEscribir.Jurisdiccion;
                retorno += LineaAEscribir.Spacer5;
                retorno += LineaAEscribir.Autorizacion;
                retorno += LineaAEscribir.NroAnulacion;
                retorno = retorno ?? "";
                return retorno;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.Model814ToLine", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<string> ModelConciliationToLine(ConciliationReportModel LineaAEscribir)
        {
            try
            {
                string tarjetaFin;
                if (LineaAEscribir.TarjetaFin != "")
                {
                    tarjetaFin = LineaAEscribir.TarjetaFin.Trim();
                    tarjetaFin = tarjetaFin.Substring(tarjetaFin.Length - 4, 4);
                }
                else
                    tarjetaFin = "";
                string tarjetaInicio = LineaAEscribir.TarjetaInicio.Length <= 5 ? "XXXXXX" : LineaAEscribir.TarjetaInicio.Trim().Substring(0, 6);
                string retorno = string.Empty;
                retorno += await this.GetFormattedNumber(LineaAEscribir.CommerceItemId.ToString(), '0', 19);
                retorno += await this.GetFormattedNumber(LineaAEscribir.TransactionId.ToString(), '0', 19);
                retorno += await this.GetFormattedNumber(LineaAEscribir.NroCommercio.ToString(), '0', 19);
                retorno += await this.GetFormattedNumber(LineaAEscribir.NroEnte, '0', 6);
                DateTime fechaPago = LineaAEscribir.FechaPago;
                retorno += fechaPago.ToString("yyyyMMdd");
                DateTime horaPago = LineaAEscribir.HoraPago;
                retorno += horaPago.ToString("yyyyMMdd");
                retorno += await this.GetFormattedNumber(LineaAEscribir.TipoTarjeta.ToString(), '0', 4);
                retorno += await this.GetFormattedNumber(tarjetaInicio, '0', 6);
                retorno += await this.GetFormattedNumber(tarjetaFin, '0', 4);
                retorno += await this.GetFormattedString(LineaAEscribir.CodigoAutorizacion, ' ', 6);
                retorno += await this.GetFormattedAndBoundedNumber(LineaAEscribir.NroTicket, '0', 4);
                retorno += await this.GetIntegerFromDecimal(LineaAEscribir.Importe, '0', 18);
                retorno += await this.GetFormattedNumber(LineaAEscribir.NroLote, '0', 4);
                retorno += await this.GetFormattedString(LineaAEscribir.MerchantId, ' ', 64);
                retorno += await this.GetFormattedString(LineaAEscribir.CodigoBarra, ' ', 68);

                return retorno;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.ModelConciliationToLine", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                return "Error \n";
            }
        }

        private async Task<string> GetFormattedNumber(string dato, char caracter, int repeticiones)
        {
            try
            {
                return string.IsNullOrEmpty(dato) ? new string(caracter, repeticiones) : dato.PadLeft(repeticiones, caracter);
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.GetFormattedNumber", "Dato: " + dato + " - ExceptionMessage: " + ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message"));
                throw;
            }
        }

        private async Task<string> GetFormattedAndBoundedNumber(
          string dato,
          char caracter,
          int repeticiones)
        {
            try
            {
                if (dato.Length > repeticiones)
                    dato = dato.Substring(dato.Length - repeticiones, repeticiones);
                return string.IsNullOrEmpty(dato) ? new string(caracter, repeticiones) : dato.PadLeft(repeticiones, caracter);
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.GetFormattedAndBoundedNumber", "Dato: " + dato + " - ExceptionMessage: " + ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message"));
                throw;
            }
        }

        private async Task<string> GetFormattedString(string dato, char caracter, int repeticiones)
        {
            try
            {
                return string.IsNullOrEmpty(dato) ? new string(caracter, repeticiones) : dato.PadRight(repeticiones, caracter);
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.GetFormattedString", "Dato: " + dato + " - ExceptionMessage: " + ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message"));
                throw;
            }
        }

        private async Task<string> GetIntegerFromDecimal(Decimal value, char caracter, int repeticiones)
        {
            try
            {
                long entero = (long)(value * 100M);
                string strEntero = entero.ToString();
                string asd = strEntero.PadLeft(repeticiones, caracter);
                return asd;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.FileHandler.GetIntegerFromDecimal", "Dato: " + value.ToString() + " - ExceptionMessage: " + ex.Message.ToString() + (ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message"));
                throw;
            };
        }
    }
}
