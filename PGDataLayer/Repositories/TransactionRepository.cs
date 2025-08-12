using PGDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PGDataLayer.EF;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace PGDataLayer.Repositories
{
    internal static class TransactionRepository
    {
        internal static bool IsEPCValid(IsEPCValidModel input)
        {
            try
            {
                using (PaymentGatewayEntities _context = new PaymentGatewayEntities())
                {
                    SqlParameter epc = new SqlParameter("@EPC", input.EPC);
                    SqlParameter service = new SqlParameter("@ServiceId", input.serviceId);
                    return _context.Database.SqlQuery<List<long>>("[dbo].[IsEPCExists] @EPC, @ServiceId", epc, service).Any();
                
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel
                {
                    exception = ex,
                    message = "Error revisando EPC",
                    module = "PGDataLayer/TransactionRepository/IsEPCValid",
                    Type = LogType.Exception
                });
                return false;
                
            }
        }

        internal static bool UpdateTransactionIdFromTransactionNumber(long transactionIdPk) 
        {
            try
            {
                using (var _context = new PaymentGatewayEntities()) 
                {
                    var transactionToUpdate = _context.Transactions.FirstOrDefault(t => t.Id == transactionIdPk);

                    var additionalInfo = _context.TransactionAdditionalInfo.FirstOrDefault(tr => tr.TransactionIdPK == transactionIdPk);

                    if (transactionToUpdate != null && additionalInfo != null)
                    {
                        transactionToUpdate.TransactionId = additionalInfo.TransactionNumber.ToString();
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error actualizacion TransactionId en dbo.Transactions",
                    module = "PGDataLayer/TransactionRepository/UpdateTransactionIdFromTransactionNumber",
                    Type = LogType.Exception
                });
            }
            return false;
        }

        internal static long GetTransactionNumberByTransactionId(long transactionIdPk) 
        {
            try
            {
                using (var _context = new PaymentGatewayEntities()) 
                {
                    var additionalInfo = _context.TransactionAdditionalInfo.FirstOrDefault(c => c.TransactionIdPK == transactionIdPk);
                    if (additionalInfo != null)
                    {
                        return additionalInfo.TransactionNumber;
                    }

                    LogRepository.InsertLog(new LogModel()
                    {
                        exception = new NullReferenceException($"TransactionAdditionalInfo no encontrada para TransactionIdPK: {transactionIdPk}"),
                        message = "Error obteniendo TransactionNumber: TAI no encontrado.", 
                        module = "PGDataLayer/TransactionRepository/GetTransactionNumberByTransactionId",
                        Type = LogType.Warning
                    });
                    return -1;
                }
            }
            catch (Exception ex) // Captura la NRE si additionalInfo es null y no se maneja explícitamente arriba.
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error obteniendo TransactionNumber por TransactionIdPK", // Mensaje ligeramente mejorado. Original "Error revisando EPC" era incorrecto.
                    module = "PGDataLayer/TransactionRepository/GetTransactionNumberByTransactionId",
                    Type = LogType.Exception
                });
                return -1;
            }
        }

        internal static Transaction GetTAI(long transactionIdPk) 
        {
            try
            {
                using (var _context = new PaymentGatewayEntities()) 
                {
                    var transactionData = (from tai in _context.TransactionAdditionalInfo
                                           join t in _context.Transactions on tai.TransactionIdPK equals t.Id
                                           join pv in _context.ProductsValidators on tai.ProductId equals pv.ProductId
                                           where tai.TransactionIdPK == transactionIdPk && tai.IsActive == true 
                                           select new Transaction 
                                           {
                                               IdPK = tai.TransactionIdPK,
                                               TransactionNumber = tai.TransactionNumber,
                                               Amount = tai.CurrentAmount,
                                               ServiceId = tai.ServiceId, 
                                               ProductId = tai.ProductId, 
                                               CustomerMail = tai.CustomerMail,
                                               TransactionIdFromValidator = t.TransactionId,
                                               ValidatorId = tai.ValidatorId,
                                               payments = tai.Payments,
                                               UniqueCode = tai.UniqueCode,
                                               ValidatorCardCode = pv.CardCode
                                           }).FirstOrDefault();

                    if (transactionData != null)
                    {
                        var productValidator = _context.ProductsValidators.FirstOrDefault(pv => pv.ValidatorId == transactionData.ValidatorId &&
                                                                              pv.ProductId == transactionData.ProductId);
                        if (productValidator != null)
                        {
                            transactionData.ValidatorCardCode = productValidator.CardCode;
                        }
                    }

                    return transactionData;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error obteniendo Transaction (TAI)", 
                    module = "PGDataLayer/TransactionRepository/GetTAI",
                    Type = LogType.Exception
                });
                return null; 
            }
        }

        public static List<RenditionData> GetRenditionReportOnTheFly(RenditionFileInput input)
        {
            var renditionList = new List<RenditionData>(); 
            try
            {
                using (var _context = new PaymentGatewayEntities()) 
                {
                    var serviceIdParam = new SqlParameter("@ServiceId", input.serviceid);
                    var dayParam = new SqlParameter("@Dia", input.day);
                    var monthParam = new SqlParameter("@Mes", input.month);
                    var yearParam = new SqlParameter("@Anio", input.year);

                    var resultsFromSp = _context.Database.SqlQuery<RenditionReportOnline_Result>(
                        "RenditionReportOnline @ServiceId, @Dia, @Mes, @Anio",serviceIdParam, dayParam, monthParam, yearParam).ToList();

                    foreach (var reportItem in resultsFromSp) 
                    {
                        renditionList.Add(new RenditionData()
                        {
                            EPC = reportItem.ElectronicPaymentCode,
                            GenericCode = reportItem.GenericCode,
                            StatusCode = reportItem.StatusCode,
                            TransactionNumber = reportItem.TrxID
                        });
                    }
                    return renditionList;
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLog(new LogModel()
                {
                    exception = ex,
                    message = "Error generando reporte on the fly",
                    module = "PGDataLayer/TransactionRepository/GetRenditionReportOnTheFly",
                    Type = LogType.Exception
                });
                return null; 
            }
        }

    }
}