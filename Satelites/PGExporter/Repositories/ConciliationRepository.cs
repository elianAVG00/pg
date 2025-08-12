using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using System.Data;

namespace PGExporter.Repositories
{
    public class ConciliationRepository(PaymentContext _context, ILogRepository _logrepository, IProcessRepository _processRepository) : IConciliationRepository
    {
        public async Task<long> GetConciliationReportId()
        {
            try
            {
                _context.Database.SetCommandTimeout(new int?(Program.DatabaseTimeOut));

                var usernameParam = new SqlParameter("@username", SqlDbType.VarChar)
                {
                    Direction = ParameterDirection.Input,
                    Value = Program.userName
                };

                var outputParam = new SqlParameter("@ProcMonitorId", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[ConciliationReport] @username, @ProcMonitorId OUTPUT", usernameParam, outputParam);
                var idproc = Convert.ToInt64(outputParam.Value);
                await _logrepository._print("PROCESO DE CONCILIADOR CONSULTADO # " + idproc.ToString(), "OK");
                return idproc;
            }
            catch (Exception ex)
            {
                await _logrepository._print("ERROR GetConciliationReportId " + ex.Message, "ERROR");
                await _logrepository.InsertLog("Error", "PGExporter.ConciliationRepository.GetConciliationReportId", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task<List<ConciliationReportModel>> GetConciliationData(long processId)
        {
            string step = "00";
            string jsonerror = "ERR";
            List<ConciliationReportModel> result = new List<ConciliationReportModel>();
            try
            {
                List<ConciliationDataModel> records = new List<ConciliationDataModel>();

                var queryNonTotalizer = from tai in _context.TransactionAdditionalInfo
                                        join cis in _context.CommerceItems
                                            on tai.TransactionIdPk equals cis.TransactionIdPk
                                        join mr in _context.MonitorFilesReportRecords
                                            on tai.TransactionIdPk equals mr.TransactionIdPk
                                        where mr.MonitorFilesReportProcessId == processId
                                              && !mr.IsTotalizer
                                        join tri in _context.TransactionResultInfo
                                            on tai.TransactionIdPk equals tri.TransactionIdPk into triGroup
                                        from tri in triGroup.DefaultIfEmpty()
                                        select new ConciliationDataModel
                                        {
                                            IdPK = tai.TransactionIdPk,
                                            MonitorFilesReportRecordsId = mr.MonitorFilesReportRecordsId,
                                            CommerceIdPK = cis.CommerceItemsId,
                                            ProductId = tai.ProductId,
                                            ChannelId = tai.ChannelId,
                                            ServiceId = tai.ServiceId,
                                            BatchNbr = tai.BatchNbr,
                                            TRI_BatchNbr = tri.BatchNbr,
                                            CardNumber = tai.CardMask,
                                            AuthCode = tai.AuthorizationCode,
                                            TicketNumber = tai.TicketNumber,
                                            TRI_CardNumber = tri != null ? tri.CardMask : null,
                                            TRI_AuthCode = tri != null ? tri.AuthorizationCode : null,
                                            TRI_TicketNumber = tri != null ? tri.TicketNumber : null,
                                            TRI_CardHolder = tri.CardHolder,
                                            TRI_IdPK = tri.TransactionResultInfoId,
                                            Amount = tai.CurrentAmount,
                                            CreatedOn = tai.CreatedOn
                                        };

                var queryTotalizer = from tai in _context.TransactionAdditionalInfo
                                     join mr in _context.MonitorFilesReportRecords
                                         on tai.TransactionIdPk equals mr.TransactionIdPk
                                     where mr.MonitorFilesReportProcessId == processId
                                           && mr.IsTotalizer
                                     join tri in _context.TransactionResultInfo
                                         on tai.TransactionIdPk equals tri.TransactionIdPk into triGroup
                                     from tri in triGroup.DefaultIfEmpty()
                                     select new ConciliationDataModel
                                     {
                                         IdPK = tai.TransactionIdPk,
                                         MonitorFilesReportRecordsId = mr.MonitorFilesReportRecordsId,
                                         CommerceIdPK = tai.TransactionIdPk,
                                         ProductId = tai.ProductId,
                                         ChannelId = tai.ChannelId,
                                         ServiceId = tai.ServiceId,
                                         BatchNbr = tai.BatchNbr,
                                         TRI_BatchNbr = tri != null ? tri.BatchNbr : null,
                                         CardNumber = tai.CardMask,
                                         AuthCode = tai.AuthorizationCode,
                                         TicketNumber = tai.TicketNumber,
                                         TRI_CardNumber = tri != null ? tri.CardMask : null,
                                         TRI_AuthCode = tri != null ? tri.AuthorizationCode : null,
                                         TRI_TicketNumber = tri != null ? tri.TicketNumber : null,
                                         TRI_CardHolder = tri.CardHolder,
                                         TRI_IdPK = tri.TransactionIdPk,
                                         Amount = tai.CurrentAmount,
                                         CreatedOn = tai.CreatedOn
                                     };

                records.AddRange(await queryNonTotalizer.AsNoTracking().ToListAsync());
                records.AddRange(await queryTotalizer.AsNoTracking().ToListAsync());

                var toRepair = new List<TransactionAdditionalInfo>();

                foreach (var record in records)
                {
                    var cent = Program.productsCentralizer.FirstOrDefault(p => p.IsActive && p.ProductId == record.ProductId);

                    if (cent == null)
                    {
                        record.IsIncomplete = true;
                    }
                    else
                    {
                        record.CentralizerCode = cent.CentralizerCode;
                        record.IsDebit = cent.IsDebit ?? false;
                        record.ExternalId = Program.serviceConfigurations.FirstOrDefault(s => s.IsActive && s.ServiceId == record.ServiceId)?.ExternalId ?? 0;
                    }

                    record.CommerceNumber = Program.configurations.FirstOrDefault(c => c.ChannelId == record.ChannelId && c.ProductId == record.ProductId
                                                && c.ValidatorId == record.ValidatorId && c.ServiceId == record.ServiceId)?.CommerceNumber;
                    if (string.IsNullOrWhiteSpace(record.CommerceNumber))
                        record.IsIncomplete = true;

                    record.BatchNbr = _processRepository.CheckBatchNbr(record.BatchNbr, record.TRI_BatchNbr);
                    if (string.IsNullOrWhiteSpace(record.BatchNbr))
                        record.IsIncomplete = true;

                    if (string.IsNullOrEmpty(record.CardNumber) || string.IsNullOrEmpty(record.AuthCode) || string.IsNullOrEmpty(record.TicketNumber))
                    {
                        if (record.TRI_IdPK.HasValue)
                        {
                            record.CardNumber = record.TRI_CardNumber;
                            record.AuthCode = record.TRI_AuthCode;
                            record.TicketNumber = record.TRI_TicketNumber;

                            toRepair.Add(new TransactionAdditionalInfo
                            {
                                TransactionIdPk = record.IdPK,
                                AuthorizationCode = record.TRI_AuthCode,
                                CardMask = record.TRI_CardNumber,
                                TicketNumber = record.TRI_TicketNumber,
                                CardHolder = record.TRI_CardHolder,
                                BatchNbr = record.BatchNbr
                            });
                        }
                        else
                        {
                            record.IsIncomplete = true;
                        }
                    }

                    result.Add(MapConciliationDataToReport(record));
                }

                if (!Program.regen)
                    await _processRepository.RepairTAITransactions(toRepair);
                return result;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ConciliationRepository.GetConciliationData" + step, "MESSAGE:" + jsonerror + " - EMSG:" + ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public ConciliationReportModel MapConciliationDataToReport(ConciliationDataModel model)
        {
            return new ConciliationReportModel()
            {
                MonitorFilesReportRecordsId = model.MonitorFilesReportRecordsId,
                IdPK = model.IdPK,
                CodigoAutorizacion = model.AuthCode,
                CodigoBarra = model.Code,
                CommerceItemId = model.CommerceIdPK,
                FechaPago = model.CreatedOn,
                HoraPago = model.CreatedOn,
                Importe = model.Amount,
                IsDebit = model.IsDebit,
                IsIncomplete = model.IsIncomplete,
                MerchantId = model.MerchantId,
                NroCommercio = model.CommerceNumber,
                NroEnte = model.ExternalId.ToString(),
                NroLote = model.BatchNbr,
                NroTicket = model.TicketNumber,
                TarjetaFin = model.CardNumber,
                TarjetaInicio = model.CardNumber,
                TipoTarjeta = model.CentralizerCode,
                TransactionId = model.IdPK.ToString(),
                TransactionNumber = model.IdPK.ToString()
            };
        }
    }
}
