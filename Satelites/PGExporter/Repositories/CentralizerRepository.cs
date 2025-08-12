using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using System.Data;

namespace PGExporter.Repositories
{
    public class CentralizerRepository(PaymentContext _context, ILogRepository _logrepository) : ICentralizerRepository
    {
        public async Task<long> GetCentralizerReportId()
        {
            try
            {
                _context.Database.SetCommandTimeout(new int?(3600));

                var usernameParam = new SqlParameter("@username", SqlDbType.VarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = Program.userName
                };

                var outputParam = new SqlParameter("@ProcMonitorId", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[CentralizerReport2] @username, @ProcMonitorId OUTPUT", usernameParam, outputParam);

                var idproc = Convert.ToInt64(outputParam.Value);
                await _logrepository._print("PROCESO DE CENTRALIZADOR CONSULTADO # " + idproc.ToString(), "OK");
                return idproc;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.CentralizerRepository.GetCentralizerReportId", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<List<CentralizerReportModel>> GetCentralizerData(long processId)
        {
            List<CentralizerReportModel> result = new List<CentralizerReportModel>();
            try
            {
                var queryCI = from tai in _context.TransactionAdditionalInfo
                              join cis in _context.CommerceItems
                                  on tai.TransactionIdPk equals cis.TransactionIdPk
                              join mr in _context.MonitorFilesReportRecords
                                  on tai.TransactionIdPk equals mr.TransactionIdPk
                              where mr.MonitorFilesReportProcessId == processId
                                      && !mr.IsTotalizer
                              select new CentralizerDataModel
                              {
                                  IdPK = tai.TransactionIdPk,
                                  MonitorFilesReportRecordsId = mr.MonitorFilesReportRecordsId,
                                  CommerceIdPK = cis.CommerceItemsId,
                                  ServiceId = tai.ServiceId,
                                  Code = tai.BarCode,
                                  Amount = tai.CurrentAmount,
                                  CreatedOn = tai.CreatedOn
                              };

                var queryNOCI = from tai in _context.TransactionAdditionalInfo
                                join mr in _context.MonitorFilesReportRecords
                                    on tai.TransactionIdPk equals mr.TransactionIdPk
                                where mr.MonitorFilesReportProcessId == processId
                                    && mr.IsTotalizer
                                select new CentralizerDataModel
                                {
                                    IdPK = tai.TransactionIdPk,
                                    MonitorFilesReportRecordsId = mr.MonitorFilesReportRecordsId,
                                    CommerceIdPK = tai.TransactionIdPk,
                                    ServiceId = tai.ServiceId,
                                    Code = tai.UniqueCode,
                                    Amount = tai.CurrentAmount,
                                    CreatedOn = tai.CreatedOn
                                };

                var records = new List<CentralizerDataModel>();
                records.AddRange(await queryCI.AsNoTracking().ToListAsync());
                records.AddRange(await queryNOCI.AsNoTracking().ToListAsync());


                foreach (var record in records)
                {
                    //if (record.IdPK == 18565671L) chequear

                    var servConf = Program.serviceConfigurations.FirstOrDefault(s => s.IsActive && s.ServiceId == record.ServiceId);

                    if (servConf != null)
                    {
                        record.BranchID = servConf.BranchId ?? default;
                        record.TerminalId = servConf.TerminalId ?? default;
                        record.ExternalId = servConf.ExternalId ?? default;
                    }

                    result.Add(MapCentralizerDataToReport(record));
                }
                return result;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.CentralizerRepository.GetCentralizerData", ex.Message, ex.InnerException?.Message ?? "Null Inner Exception Message");
                throw;
            }
        }

        public CentralizerReportModel MapCentralizerDataToReport(CentralizerDataModel model)
        {
            return new CentralizerReportModel()
            {
                IdPK = model.IdPK,
                MonitorFilesReportRecordsId = model.MonitorFilesReportRecordsId,
                AnioCuota = new string('0', 7),
                Autorizacion = new string('0', 15),
                BancoCheque = new string('0', 3),
                BancoCodPostal = new string('0', 4),
                BancoSucursal = new string('0', 3),
                CodigoBarra = model.Code,
                CodigoBCRA = "0814",
                CodigoCajero = new string('0', 4),
                CodigoEnte = new int?(model.ExternalId),
                CodigoOperacion = "A3",
                CodigoR = "R",
                CodigoSeguridad = new string('0', 3),
                CodigoServicio = model.IdPK,
                CodigoSucursal = new int?(model.BranchID),
                CodigoTerminal = new int?(model.TerminalId),
                CommerceItemId = model.CommerceIdPK,
                Datos = "Datos",
                Desde = new string('0', 2),
                FechaPago = model.CreatedOn,
                FechaVto1 = new string('0', 6),
                FechaVto2 = new string('0', 6),
                FondoEducativo = new string('0', 3),
                FoProvi = new string('0', 9),
                FormaPago = new string('0', 2),
                Hasta = new string('0', 2),
                Importe = model.Amount,
                Interes = new string('0', 11),
                Jurisdiccion = new string('0', 4),
                Moneda = new string('0', 1),
                NroAnulacion = new string('0', 8),
                NroCheque = new string('0', 8),
                NroCuenta = new string('0', 8),
                NroSecuenciaOn = new string('0', 8),
                NroSecuenciaTrans = model.CommerceIdPK,
                Plazo = new string('0', 3),
                Recargo = new string('0', 11),
                Spacer1 = new string(' ', 3),
                Spacer2 = new string(' ', 10),
                Spacer3 = new string(' ', 2),
                Spacer4 = new string(' ', 1),
                Spacer5 = new string(' ', 3)
            };
        }
    }
}
