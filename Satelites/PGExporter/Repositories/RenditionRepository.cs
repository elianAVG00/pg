using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PGExporter.EF;
using PGExporter.Interfaces;
using PGExporter.Models;
using System.Data;

namespace PGExporter.Repositories
{
    public class RenditionRepository(
  PaymentContext _context,
  ILogRepository _logrepository,
  IProcessRepository _processRepository) : IRenditionRepository
    {
        public async Task<long> GetRenditionReportId(long ServiceID)
        {
            try
            {
                _context.Database.SetCommandTimeout(new int?(Program.DatabaseTimeOut));
                SqlParameter sqlParameter1 = new SqlParameter("@ProcMonitorId", SqlDbType.BigInt);
                sqlParameter1.Direction = ParameterDirection.Output;
                SqlParameter outProcMonitorId = sqlParameter1;
                SqlParameter sqlParameter2 = new SqlParameter("@username", SqlDbType.VarChar);
                sqlParameter2.Direction = ParameterDirection.Input;
                sqlParameter2.Value = (object)Program.userName;
                SqlParameter uname = sqlParameter2;
                SqlParameter sqlParameter3 = new SqlParameter("@ServiceId", SqlDbType.BigInt);
                sqlParameter3.Direction = ParameterDirection.Input;
                sqlParameter3.Value = (object)ServiceID;
                SqlParameter ServiceId = sqlParameter3;
                int num = await _context.Database.ExecuteSqlRawAsync("EXEC [dbo].[RenditionReport] @username, @ServiceId, @ProcMonitorId OUTPUT", (object)uname, (object)ServiceId, (object)outProcMonitorId);
                long pid = Convert.ToInt64(outProcMonitorId.Value);
                await _logrepository._print("PROCESO DE RENDICION CONSULTADO # " + pid.ToString(), "OK");
                return pid;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.RenditionRepository.GetRenditionReportId", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                throw ex;
            }
            long renditionReportId;
            return renditionReportId;
        }

        public async Task<List<RenditionReportModel>> GetRenditionData(long id)
        {
            List<RenditionReportModel> retorno = new List<RenditionReportModel>();
            try
            {
                return retorno;
            }
            catch (Exception ex)
            {
                await _logrepository.InsertLog("Error", "PGExporter.RenditionRepository.GetRenditionData", ex.Message.ToString(), ex.InnerException != null ? ex.InnerException.Message : "Null Inner Exception Message");
                return retorno;
            }
        }

        public RenditionReportModel MapRenditionDataToReport(RenditionDataModel model)
        {
            return new RenditionReportModel()
            {
                MonitorFilesReportRecordsId = model.MonitorFilesReportRecordsId,
                Amount = model.Amount,
                BatchNbr = model.BatchNbr,
                CardHolderName = model.CardHolder,
                CardMask = model.CardMask,
                Code = model.Code,
                Country = "AR",
                Currency = 32,
                CustomerId = "",
                CustomerMail = model.Email,
                ElectronicPaymentCode = model.EPC,
                Description = model.Description,
                MerchantId = model.MerchantId,
                MerchOrderId = model.InternalNbr,
                MerchTrxRef = model.InternalNbr,
                NumPayments = model.Payments,
                Operation = "TC - Compra OnLine",
                PosDateTime = model.CreatedOn,
                Price = model.CIAmount,
                Producto = model.ProductName,
                TrxID = model.TransctionNumber,
                TrxSource = model.ChannelName,
                IsIncomplete = model.IsIncomplete
            };
        }
    }

}
