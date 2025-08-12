using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SPSCierreLote.EFCore.models;
using SPSCierreLote.Models;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace SPSCierreLote.Repositories;

/// <summary>
/// Operaciones contra BD para auditoría, validación y persistencia del proceso
/// “SPS Cierre de Lote”.
/// </summary>
internal static class SPSMonitorDatabase
{
    // ==========================================================
    // 1) INICIO / FIN DEL PROCESO GLOBAL ───────────────────────
    // ==========================================================

    internal static long InitNewProcess()
    {
        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            var entry = new MonitorSPSBatchProcess
            {
                CreatedBy = Program.AppVersion,
                CreatedOn = DateTime.Now,
                IsActive = true,
                BeginOn = DateTime.Now,
                WithValidation = Program.IsValidationActive,
                IsFTP = Program.IsFtpMethod
            };

            ctx.MonitorSPSBatchProcess.Add(entry);
            ctx.SaveChanges();

            return entry.MonitorSPSBatchProcessId;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F20 – InitNewProcess");
            Program.Print("ERROR DE ESCRITURA EN MonitorSPSBatchProcess", "ERROR");
            return 0;
        }
    }

    internal static Generic CloseNewProcess(
        int filesFound,
        int filesRead,
        int totalRecordsRead,
        int totalRecordsNotRead,
        bool withErrors)
    {
        var result = new Generic();

        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            var proc = ctx.MonitorSPSBatchProcess
                          .First(p => p.MonitorSPSBatchProcessId == Program.MonitorProcessId);

            proc.FilesFound = filesFound;
            proc.FilesRead = filesRead;
            proc.TotalRecordsRead = totalRecordsRead;
            proc.TotalRecordsNotRead = totalRecordsNotRead;
            proc.WithError = withErrors;
            proc.EndOn = DateTime.Now;

            ctx.SaveChanges();
            result.Booleano = false;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F21 – CloseNewProcess");
            Program.Print("ERROR DE ESCRITURA EN MonitorSPSBatchProcess", "ERROR");
            result.Booleano = true;
            result.Cadena = ex.Message;
        }

        return result;
    }

    // ==========================================================
    // 2) INICIO / FIN DE ARCHIVO INDIVIDUAL ────────────────────
    // ==========================================================

    internal static GenericWithFileInfo InitNewFile(FileInfoModel fileMeta)
    {
        var generic = new Generic();
        var composite = new GenericWithFileInfo { skipThisFile = false };

        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            var entry = new MonitorSPSBatchProcessFiles
            {
                MonitorSPSBatchProcessId = Program.MonitorProcessId,
                CreatedBy = Program.AppVersion,
                CreatedOn = DateTime.Now,
                IsActive = true,
                BeginOn = DateTime.Now
            };

            // --------- validación de nombre ---------
            FileInformation info = SPSFileRepository.GetFileInformationFromName(fileMeta.Name);

            // filtro por parámetro de fecha (CLI: d:AAAAmmdd)
            if (Worker.ParameterDateFlag && info.FileDatetime.Date != Worker.ParameterDate.Date)
            {
                composite.skipThisFile = true;
                return composite;
            }

            int? productId = null;

            if (Program.IsFilenameValidationActive)
            {
                if (!info.IsValidFilename)
                {
                    generic.Booleano = true;
                    generic.Cadena = "El nombre del archivo no se puede validar";
                }
                else
                {
                    // Verificamos que IDSITE y ProductCode existan en la tabla cacheada
                    string idsiteClean = info.FileIDSITE.TrimStart('0');
                    int productCodeClean = Convert.ToInt32(info.FileProductCode.TrimStart('0'));

                    var match = Program.SiteProducts.FirstOrDefault(sp =>
                        sp.IDSite.TrimStart('0') == idsiteClean &&
                       (sp.SPSPrisma == productCodeClean || sp.SPSNormal == productCodeClean));

                    if (match is null)
                    {
                        generic.Booleano = true;
                        generic.Cadena = "IDSITE y/o Código de producto inválido";
                    }
                    else
                    {
                        productId = match.ProductId;
                    }
                }
            }

            entry.FileDate = info.FileDATE;
            entry.RealFileDate = DateOnly.ParseExact(info.FileDATE, "ddMMyy", null);
            entry.Filename = fileMeta.FullName;
            entry.IDSITE = info.FileIDSITE;
            entry.ProductCode = info.FileProductCode;
            entry.ProductId = productId;
            entry.WithValidation = Program.IsValidationActive;
            entry.TicketNumberOnSupport = string.Empty;

            // ¿El archivo está vacío?
            var emptyCheck = SPSFileRepository.IsTextFileEmpty(fileMeta.FullName);
            if (emptyCheck.Booleano)
            {
                generic.Booleano = true;
                generic.Cadena = "El archivo está vacío o es inválido";
            }
            else
            {
                composite.file_content = emptyCheck.Cadena;
            }

            if (generic.Booleano)
            {
                entry.EndOn = DateTime.Now;
                entry.WithError = true;
                entry.ValidationError = generic.Cadena;
            }

            ctx.MonitorSPSBatchProcessFiles.Add(entry);
            ctx.SaveChanges();

            generic.Id = entry.MonitorSPSBatchProcessFilesId;
            composite.generico = generic;
            composite.FileInformation = info;

            return composite;
        }
        catch (DbEntityValidationException ex)
        {
            Program.Logger.LogError(ex, "Error F22 – InitNewFile (EF validation)");
            generic.Booleano = true;
            generic.Cadena = string.Join(" | ", ex.EntityValidationErrors
                                                .SelectMany(e => e.ValidationErrors)
                                                .Select(v => $"Property:{v.PropertyName} Error:{v.ErrorMessage}"));
            composite.generico = generic;
            return composite;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F23 – InitNewFile");
            generic.Booleano = true;
            generic.Cadena = ex.Message;
            composite.generico = generic;
            return composite;
        }
    }

    internal static Generic CloseNewFile(
        FileInfoModel fileMeta,
        SPSFileModel processedFile,
        long fileId)
    {
        var result = new Generic();

        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            var fileRow = ctx.MonitorSPSBatchProcessFiles
                             .First(f => f.MonitorSPSBatchProcessFilesId == fileId);

            // Trailer ↔ objeto
            fileRow.TrailerRecords = processedFile.Trailer.CantidadDeRegistros;
            fileRow.TrailerIdLote = int.Parse(processedFile.Trailer.IDLote);
            fileRow.TrailerAutorizadas = processedFile.Trailer.CantidadCompras;
            fileRow.TrailerAnuladas = processedFile.Trailer.CantidadAnuladas;
            fileRow.TrailerDevueltas = processedFile.Trailer.CantidadDevueltas;
            fileRow.TrailerMontoAutorizadas = processedFile.Trailer.MontoCompras;
            fileRow.TrailerMontoAnuladas = processedFile.Trailer.MontoAnuladas;
            fileRow.TrailerMontoDevueltas = processedFile.Trailer.MontoDevueltas;
            fileRow.HasInconsistenceError = processedFile.HasPGInconsistence;

            if (processedFile.UnknownTransactionNumbers.Any())
            {
                fileRow.HasUnknownRecords = true;
                fileRow.TotalUnknownRecords = processedFile.UnknownTransactionNumbers.Count;
                fileRow.UnknownRecords = JsonConvert.SerializeObject(processedFile.UnknownTransactionNumbers);
            }
            else
            {
                fileRow.HasUnknownRecords = false;
            }

            fileRow.TotalRecordsRead = processedFile.RecordsRead;
            fileRow.TotalRecordsNotRead = processedFile.RecordsNotRead;

            // ----- mover archivo -----
            string slash = Program.IsFtpMethod ? "/" : "\\";
            DateTime archiveDate = Program.SaveHistoricByActualDate
                ? DateTime.Now
                : processedFile.FileInfoSPS.FileDatetime;

            string destination =
                $"{Program.FolderDestiny}{archiveDate:yyyy}{slash}{archiveDate:MM}{slash}{archiveDate:dd}{slash}";

            var moveRes = SPSFileRepository.MoveFile(fileMeta.FullName, destination, fileMeta.Name);
            fileRow.MovedToHistory = !moveRes.Booleano;

            if (moveRes.Booleano)
            {
                processedFile.FileErrorList.Add(new SPSErrores
                {
                    ErrorMessage = $"No se pudo mover el archivo {fileMeta.FullName}",
                    ErrorExceptionMessage = moveRes.Cadena
                });
            }

            if (processedFile.FileErrorList.Any())
            {
                fileRow.WithError = true;
                fileRow.ValidationError = JsonConvert.SerializeObject(processedFile.FileErrorList);
            }

            fileRow.EndOn = DateTime.Now;

            ctx.SaveChanges();
            result.Booleano = false;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F24 – CloseNewFile");
            Program.Print("ERROR DE ESCRITURA EN MonitorSPSBatchProcess", "ERROR");
            result.Booleano = true;
            result.Cadena = ex.Message;
        }

        return result;
    }

    // ==========================================================
    // 3) HOMOLOGACIÓN: CARGA + SP + CRUCE ──────────────────────
    // ==========================================================

    internal static void InsertBulkDatatable(DataTable data)
    {
        string providerConn = Program.Configuration.GetConnectionString("PaymentGatewaySQL");

        using var sqlConn = new SqlConnection(providerConn);
        sqlConn.Open();

        using var tran = sqlConn.BeginTransaction();

        try
        {
            using var bulk = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.TableLock, tran)
            {
                DestinationTableName = data.TableName
            };

            foreach (DataColumn col in data.Columns)
                bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);

            bulk.WriteToServer(data);
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            Program.Logger.LogError(ex, "Error F25 – InsertBulkDatatable");
            Program.Print("ERROR BULK INSERT", "ERROR");//esto se agrego 
            throw;
        }
    }

    internal static List<GetInformationForSPSBatchValidation_Result> GetDataValidations()
    {
        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            var validations = ctx.Database
                                 .SqlQueryRaw<GetInformationForSPSBatchValidation_Result>("GetInformationForSPSBatchValidation")
                                 .ToList();

            Program.Print("EVALUACIÓN DE HOMOLOGACIÓN EJECUTADA", "OK");

            ctx.Database.ExecuteSqlRaw("DELETE FROM [dbo].[SpsBatchTemporalValidation]");
            Program.Print("DATOS TEMPORALES DE HOMOLOGACIÓN BORRADOS", "OK");

            return validations;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F26 – GetDataValidations");
            Program.Print("EVALUACIÓN DE HOMOLOGACIÓN", "ERROR");
            return new List<GetInformationForSPSBatchValidation_Result>();
        }
    }

    internal static SPSFileModel ValidateDataInDatabase(SPSFileModel file)
    {
        // ---------- 1) Prepara DataTable para bulk ----------
        var dt = new DataTable("SpsBatchTemporalValidation");
        dt.Columns.Add("TransactionNumber", typeof(long));
        dt.Columns.Add("CurrentAmount", typeof(long));
        dt.Columns.Add("IDSITE", typeof(long));
        dt.Columns.Add("MedioDePago", typeof(int));
        dt.Columns.Add("FechaDeOperacion", typeof(DateTime));
        dt.Columns.Add("EsCargo", typeof(bool));

        foreach (var r in file.Records)
            dt.Rows.Add(r.TransctionId, r.Monto, r.IDSITE, r.MedioDePago, r.Fecha, r.EsCargo);

        InsertBulkDatatable(dt);
        Program.Print("DATOS TEMPORALES DE HOMOLOGACIÓN GUARDADOS", "OK");

        // ---------- 2) Llama SP y procesa resultado ----------
        var spResults = GetDataValidations();

        if (spResults.IsNullOrEmpty())
        {
            Program.Print($"NO SE PUDIERON VALIDAR LAS TRANSACCIONES CONTRA LA DB", "WARNING");//esto lo agregue 
        }

        file.HasPGInconsistence = false;

        foreach (var val in spResults)
        {
            // Ubicar registro correspondiente por PK de transacción
            var rec = file.Records
                          .FirstOrDefault(r => r.TransactionIDPK == val.TransactionIdPK);

            if (rec == null)
            {
                // Mapear TransactionNumber si PK todavía es 0
                rec = file.Records.FirstOrDefault(r =>
                    r.TransctionId == val.TransactionNumber);

                if (rec != null)
                    rec.TransactionIDPK = val.TransactionIdPK.GetValueOrDefault();
            }

            if (rec == null) continue;

            // ----- Evaluar inconsistencias -----
            var inconsistencias = new List<SPSErrorValidacionDato>();

            void AddErr(string msg, string valSps = "", string valPg = "") =>
                inconsistencias.Add(new SPSErrorValidacionDato
                {
                    ErrorMessage = msg,
                    Valor_DECIDIR = valSps,
                    Valor_PG = valPg
                });

            if (!val.IsAmountOK)
                AddErr("El monto informado en el archivo no coincide con el de la transacción",
                       val.SPSMonto.ToString(), val.PGMonto.ToString());

            if (!val.IsCardCodeOK)
                AddErr("El código de tarjeta informado en el archivo no coincide con el de la transacción",
                       val.SPSMedioDePago.ToString(), val.PGMedioDePago.ToString());

            if (!val.IsDateOK)
                AddErr("La fecha informada en el archivo no coincide con la de la transacción",
                       val.SPSFechaOperacion.ToString("yyyy-MM-dd"),
                       val.PGFechaOperacion?.ToString("yyyy-MM-dd") ?? "N/A");

            if (!val.IsUniqueCodeOK)
                AddErr("El IDSITE informado en el archivo no coincide con el de la transacción",
                       val.SPSIDSITE.ToString(), val.PGUniqueCode.ToString());

            if (!val.IsDateGAPOK)
                AddErr("La transacción informada tiene demasiados días",
                       val.SPSFechaOperacion.ToString("yyyy-MM-dd"),
                       val.PGFechaOperacion?.ToString("yyyy-MM-dd") ?? "N/A");

            if (!val.IsDuplicatedOK)
                AddErr("La transacción ya había sido informada");

            // Marcar flags
            rec.HasPGInconsistence = inconsistencias.Any();
            rec.inconsistence_cost = !val.IsAmountOK;
            rec.inconsistence_card = !val.IsCardCodeOK;
            rec.inconsistence_date = !val.IsDateOK;
            rec.inconsistence_uniq = !val.IsUniqueCodeOK;
            rec.inconsistence_days = !val.IsDateGAPOK;
            rec.inconsistence_dups = !val.IsDuplicatedOK;
            rec.PGDataInconsistenceList = inconsistencias;

            if (inconsistencias.Any())
            {
                file.HasPGInconsistence = true;
                Program.Print($"SE ENCONTRARON INCONSISTENCIAS DE HOMOLOGACIÓN EN TRANSACCIÓN:{rec.TransactionIDPK}", "WARNING");
            }
        }

        // Identificar transacciones desconocidas
        file.UnknownTransactionNumbers = file.Records
                                             .Where(r => r.TransactionIDPK == 0)
                                             .Select(r => r.TransctionId)
                                             .Distinct()
                                             .ToList();

        // Quitar las desconocidas del set válido
        file.Records.RemoveAll(r => r.TransactionIDPK == 0);

        return file;
    }

    // ==========================================================
    // 4) PERSISTENCIA DE RESULTADOS POR TRANSACCIÓN ────────────
    // ==========================================================

    internal static Generic SaveSPSBatchMainData(SPSFileModel file, long fileId)
    {
        var result = new Generic();

        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            foreach (var rec in file.Records)
            {
                if (Program.ProcessSingleTxn &&
                    rec.TransactionIDPK != Program.SingleTxnIdPk)
                    continue;

                string cardMaskClean = rec.CardMask.TrimStart('0');
                string ticketClean = rec.NumeroDeCupon.TrimStart('0');
                string last4 = cardMaskClean[^4..];

                // ----- Auditoría por transacción -----
                ctx.MonitorSPSBatchProcessTransactions.Add(
                    new MonitorSPSBatchProcessTransactions
                    {
                        MonitorSPSBatchProcessFilesId = fileId,
                        TransactionIDPK = rec.TransactionIDPK,
                        CreatedOn = DateTime.Now,
                        CreatedBy = Program.AppVersion,
                        HasInconsistenceError = rec.HasPGInconsistence,
                        InconsistenceCARD = rec.inconsistence_card,
                        InconsistenceDATE = rec.inconsistence_date,
                        InconsistenceDUPS = rec.inconsistence_dups,
                        InconsistenceDAYS = rec.inconsistence_days,
                        InconsistenceUNIQ = rec.inconsistence_uniq,
                        InconsistenceCOST = rec.inconsistence_cost,
                        InconsistenceError = JsonConvert.SerializeObject(rec.PGDataInconsistenceList)
                    });

                var tai = ctx.TransactionAdditionalInfo
                             .First(t => t.TransactionIdPK == rec.TransactionIDPK);

                tai.AuthorizationCode = rec.AuthorizationCode;
                tai.CardMask = cardMaskClean;
                tai.TicketNumber = ticketClean;
                tai.BatchNbr = rec.IDLOTE;
                tai.BatchSPSDate = DateTime.Now;

                // ----- set status -----
                bool changeStatus = false;
                int newStatus = 5; // completado

                var ts = ctx.TransactionStatus
                            .FirstOrDefault(s => s.TransactionsId == rec.TransactionIDPK);

                if (rec.EsCargo)
                {
                    if (ts?.StatusCodeId != 5)
                        changeStatus = true;
                }
                else
                {
                    newStatus = 36; // devuelto/anulado
                    changeStatus = true;
                }

                //if (changeStatus) ARREGLAR ESTE LLAMADO AL SP setStatus
                //    ctx.SetStatus(rec.TransactionIDPK, newStatus, Program.AppVersion);

                // ----- TransactionResultInfo (insert/update) -----
                var tri = ctx.TransactionResultInfo
                              .Where(t => t.TransactionIdPK == rec.TransactionIDPK)
                              .ToList();

                if (!tri.Any())
                {
                    ctx.TransactionResultInfo.Add(new TransactionResultInfo
                    {
                        AuthorizationCode = rec.AuthorizationCode,
                        CardMask = cardMaskClean,
                        CardNbrLfd = last4,
                        TicketNumber = ticketClean,
                        BatchNbr = rec.IDLOTE,
                        BatchSPSDate = DateTime.Now,
                        TransactionIdPK = rec.TransactionIDPK,
                        CreatedBy = Program.AppVersion,
                        CreatedOn = DateTime.Now
                    });
                }
                else
                {
                    tri.ForEach(t =>
                    {
                        t.AuthorizationCode = rec.AuthorizationCode;
                        t.CardMask = cardMaskClean;
                        t.CardNbrLfd = last4;
                        t.TicketNumber = ticketClean;
                        t.BatchNbr = rec.IDLOTE;
                        t.BatchSPSDate = DateTime.Now;
                    });
                }

                ctx.SaveChanges();
            }

            result.Booleano = false;
        }
        catch (DbEntityValidationException ex)
        {
            Program.Logger.LogError(ex, "Error F27 – SaveSPSBatchMainData (EF validation)");
            result.Booleano = true;
            result.Cadena = string.Join(" | ", ex.EntityValidationErrors
                                             .SelectMany(e => e.ValidationErrors)
                                             .Select(v => $"Property:{v.PropertyName} Error:{v.ErrorMessage}"));
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F28 – SaveSPSBatchMainData");
            result.Booleano = true;
            result.Cadena = ex.Message;
        }

        return result;
    }

    // ==========================================================
    // 5) FORZAR `SetSPSBatch` ──────────────────────────────────
    // ==========================================================

    internal static void ForceSetSPSBatch()
    {
        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();
            ctx.Database.SetCommandTimeout(3600); // 1 h
            ctx.Database.ExecuteSqlRaw("[dbo].[SetSPSBatch]");
            Program.Print("FORCE SPS BATCH EJECUTADO", "OK");
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F29 – ForceSetSPSBatch");
            Program.Print("FORCE SPS BATCH EJECUTADO", "ERROR");
        }
    }

    // ==========================================================
    // 6) Nuevo Updatear MonitorSPSBatchProcessFiles ────────────
    // ==========================================================

    internal static void UpdateFileErrors(long monitorFileId, string errorsJson)
    {
        var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

        ctx.MonitorSPSBatchProcessFiles
           .Where(f => f.MonitorSPSBatchProcessFilesId == monitorFileId)
           .ExecuteUpdate(s => s
                .SetProperty(p => p.ValidationError, errorsJson)
                .SetProperty(p => p.WithError, true)
                .SetProperty(p => p.EndOn, DateTime.Now));

        ctx.SaveChanges();
    }
}
