using PGDataAccess.EF;
using PGDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace PGDataAccess.Repository
{
    public class JobRepository
    {
        public static void InsertRefundJobRunLog(List<long> CommerceItemId, int JobRunLog) { 
            //TODO
        }

        public static List<ReporteRenditionExportModel> GetRenditionInformation() {

            List<ReporteRenditionExportModel> servicesToReturn = new List<ReporteRenditionExportModel>();
            try
            {
                using (var context = new PGDataEntities())
                {
                    servicesToReturn = (from sconf in context.ServicesConfig
                                        join serv in context.Services
                                        on sconf.ServiceId equals serv.ServiceId
                                        where sconf.RptToRendition
                                        select new ReporteRenditionExportModel
                                        {
                                            merchantId = serv.MerchantId,
                                            RenditionFolder = sconf.ReportsPath,
                                            serviceName = serv.Name
                                        }).ToList();
                }
            }
            catch (Exception ex) {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
            }
            return servicesToReturn;
        }

        public static void InsertReportJobRunLog(List<long> CommerceItemId, int JobRunLog, string ReportType)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    foreach (long CommerceItem in CommerceItemId)
                    {
                        ReportJobLog reportToUpdate = new ReportJobLog();

                        reportToUpdate = (from rep in context.ReportJobLog
                                          where rep.CommerceItemId == CommerceItem
                                          select rep).FirstOrDefault();

                        switch (ReportType)
                        {
                            case "centralizerRefunds":
                                reportToUpdate.CentralizerRefundJobRun = JobRunLog;
                                break;
                            case "renditionRefunds":
                                reportToUpdate.RenditionRefundJobRun = JobRunLog;
                                break;
                            case "centralizer":
                                reportToUpdate.CentralizerJobRun = JobRunLog;
                                break;
                            case "conciliation":
                                reportToUpdate.ConciliationJobRun = JobRunLog;
                                break;
                            case "rendition":
                                reportToUpdate.RenditionJobRun = JobRunLog;
                                break;
                            default:
                                return;
                        }

                        if (reportToUpdate == null)
                        {
                            reportToUpdate.IsActive = true;
                            reportToUpdate.CommerceItemId = CommerceItem;
                            reportToUpdate.CreatedOn = DateTime.Now;
                            reportToUpdate.CreatedBy = "system";
                            context.ReportJobLog.Add(reportToUpdate);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
            }
        }

        public static int InsertJobRunLog(JobLogModel jobToInsert)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    var newJobRunLog = new JobRunLog();
                    newJobRunLog.IsActive = true;
                    newJobRunLog.DateStart = DateTime.Now;
                    if (jobToInsert.IsFinish)
                    {
                        newJobRunLog.DateFinish = DateTime.Now;
                    }
                    else {
                        DateTime? nulldate = null;
                        newJobRunLog.DateFinish = nulldate;
                    }
                    newJobRunLog.Parameters = jobToInsert.Parameters;
                    newJobRunLog.SendNotificationTo = jobToInsert.SendNotificationTo;
                    newJobRunLog.State = jobToInsert.State;
                    newJobRunLog.Type = jobToInsert.Type;

                    context.JobRunLog.Add(newJobRunLog);
                    context.SaveChanges();
                    return newJobRunLog.JobRunLogId;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        throw new Exception("Ocurrió un error interno - ERR-" + LogRepository.InsertLogCommon(LogTypeModel.Error,"Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:","- Property: " + ve.PropertyName + " | Error: " + ve.ErrorMessage).ToString());
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
                return 0;
    
            }
        }

        public static void UpdateJobRunLog(JobLogModel jobToUpdate)
        {
            try
            {
                using (var context = new PGDataEntities())
                {
                    JobRunLog jobrunToUpdate = new JobRunLog();

                    jobrunToUpdate = (from jobr in context.JobRunLog
                                      where jobr.JobRunLogId == jobToUpdate.JobRunLogId
                                      select jobr).FirstOrDefault();

                    if (jobToUpdate.IsFinish) { jobrunToUpdate.DateFinish = DateTime.Now; }
                    jobrunToUpdate.SendNotificationTo += jobToUpdate.SendNotificationTo;
                    jobrunToUpdate.State = jobToUpdate.State;
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                LogRepository.InsertLogException(LogTypeModel.Error, ex).ToString();
            }
        }
    }
}