using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using System.Data;

namespace PGExporter.Repositories
{
    public class ProcessRepository(PaymentContext _context, ILogRepository _logrepository) : IProcessRepository
    {
        public string? CheckBatchNbr(string? fromTAI, string? fromTRI)
        {
            if (!string.IsNullOrEmpty(fromTAI))
                return fromTAI;
            if (!string.IsNullOrEmpty(fromTRI))
                return fromTRI;
            return Program.ActivateSPSBatchNumberFixed ? Program.SPSBatchNumberFixed : (string)null;
        }

        public async Task<bool> RollBackProcess()
        {
            try
            {
                _context.Database.SetCommandTimeout(new int?(Program.DatabaseTimeOut));
                SqlParameter outProcMonitorId = new SqlParameter("@ResultRollback", SqlDbType.Bit);
                outProcMonitorId.Direction = ParameterDirection.Output;

                SqlParameter uname = new SqlParameter("@FilesReportProcessId", SqlDbType.VarChar);
                uname.Direction = ParameterDirection.Input;
                uname.Value = (object)Program.userName;

                int num = await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[RollBackFileReportProcess] @FilesReportProcessId, @ResultRollback OUTPUT", uname, outProcMonitorId);
                bool pid = Convert.ToBoolean(outProcMonitorId.Value);
                await _logrepository._print("PROCESO DE ROLLBACK: " + (pid ? "OK" : "NO"), "OK");
                return pid;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.RollBackProcess", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task RepairTAITransactions(List<TransactionAdditionalInfo> transactionsToRepair)
        {
            try
            {
                int counterChunk = 0;
                for (int i = 0; i < transactionsToRepair.Count(); i += Program.ChunkSize)
                {
                    IEnumerable<TransactionAdditionalInfo> chunk = transactionsToRepair.Skip(i).Take(Program.ChunkSize).ToList();
                    counterChunk += chunk.Count();
                    List<long> chunkTransactionIds = chunk.Select(c => c.TransactionIdPk).ToList();
                    List<TransactionAdditionalInfo> entitiesToUpdate = _context.TransactionAdditionalInfo.Where(e => chunkTransactionIds.Contains(e.TransactionIdPk)).ToList();

                    foreach (TransactionAdditionalInfo transactionAdditionalInfo in entitiesToUpdate)
                    {
                        TransactionAdditionalInfo transtofix = transactionAdditionalInfo;
                        TransactionAdditionalInfo record = transactionsToRepair.Where(r => r.TransactionIdPk == transtofix.TransactionIdPk).FirstOrDefault();
                        transtofix.AuthorizationCode = record.AuthorizationCode;
                        transtofix.CardMask = record.CardMask;
                        transtofix.TicketNumber = record.TicketNumber;
                        transtofix.CardHolder = record.CardHolder;
                        transtofix.BatchNbr = record.BatchNbr;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.RepairTAITransactions", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
        }

        public async Task MarkAsInformed(IEnumerable<long> data)
        {
            try
            {
                int counterChunk = 0;
                for (int i = 0; i < data.Count(); i += Program.ChunkSize)
                {
                    IEnumerable<long> chunk = data.Skip(i).Take(Program.ChunkSize).ToList();
                    counterChunk += chunk.Count();
                    List<MonitorFilesReportRecords> entitiesToUpdate = _context.MonitorFilesReportRecords.Where(e => chunk.Contains<long>(e.MonitorFilesReportRecordsId)).ToList();
                    entitiesToUpdate.ForEach(e => e.Informed = true);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.MarkAsInformed", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
        }

        public async Task RemoveOriginalMark(int type, IEnumerable<long> ids)
        {
            try
            {
                int counterChunk = 0;
                for (int i = 0; i < ids.Count(); i += Program.ChunkSize)
                {
                    IEnumerable<long> chunk = ids.Skip(i).Take(Program.ChunkSize).ToList();
                    counterChunk += chunk.Count();
                    List<TransactionAdditionalInfo> entitiesToUpdate = _context.TransactionAdditionalInfo.Where(e => chunk.Contains<long>(e.TransactionIdPk)).ToList();

                    switch (type)
                    {
                        case 1:
                            entitiesToUpdate.ForEach(e => e.ReportDateCentralizer = new DateTime?());
                            break;
                        case 2:
                            entitiesToUpdate.ForEach(e => e.ReportDateConciliation = new DateTime?());
                            break;
                        case 3:
                            entitiesToUpdate.ForEach(e => e.ReportDateRendition = new DateTime?());
                            break;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.RemoveOriginalMark", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task MarkAsComplete(IEnumerable<long> data)
        {
            try
            {
                int counterChunk = 0;
                for (int i = 0; i < data.Count(); i += Program.ChunkSize)
                {
                    IEnumerable<long> chunk = data.Skip(i).Take(Program.ChunkSize).ToList();
                    counterChunk += chunk.Count();
                    List<MonitorFilesReportRecords> entitiesToUpdate = _context.MonitorFilesReportRecords.Where(e => chunk.Contains<long>(e.MonitorFilesReportRecordsId)).ToList();
                    entitiesToUpdate.ForEach(e => e.IsIncomplete = false);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.MarkAsComplete", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw;
            }
        }

        public async Task CloseProcess(long monitorid, string filename, List<Records> registros, bool withError = false, List<string>? errors = null)
        {
            if (withError)
            {
                for (int i = 0; i < registros.Count; ++i)
                    registros[i].Informed = false;
            }
            try
            {
                MonitorFilesReportProcess cp = await _context.MonitorFilesReportProcess.Where(mp => mp.MonitorFilesReportProcessId == monitorid).FirstOrDefaultAsync();

                cp.EndProcessOn = new DateTime?(DateTime.Now);
                cp.RemoteFile = filename;
                cp.HasError = withError;
                if (withError)
                    cp.Error = string.Join("|", errors.ToArray());
                cp.TotalRecords = registros.Count();
                cp.TotalInformed = registros.Where((r => r.Informed)).Count();
                cp.InformedAmount = Convert.ToInt64(registros.Where((r => r.Informed)).Sum((c => c.Amount)) * 100M);
                cp.IncompleteAmount = Convert.ToInt64(registros.Where((r => r.Incomplete)).Sum((c => c.Amount)) * 100M);
                await _context.SaveChangesAsync();

                await _logrepository._print("Cerrando Proceso....", "OK");
                await this.MarkAsInformed(registros.Where((r => r.Informed)).Select((e => e.MonitorFilesReportRecordsId)));

                await _logrepository._print("Registros Informados", "OK");
                await this.MarkAsComplete(registros.Where((r => !r.Incomplete)).Select((e => e.MonitorFilesReportRecordsId)));

                await _logrepository._print("Registros completados", "OK");
                await this.RemoveOriginalMark(cp.Type, registros.Where((r => !r.Informed)).Select((e => e.TransctionIDPK)));

                await _logrepository._print("Checkout de registros incompletos realizado", "OK");
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.ProcessRepository.CloseProcess", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
            }
        }
    }

}
