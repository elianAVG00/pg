using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SPSCierreLote.Models;
using SPSCierreLote.Tools;

namespace SPSCierreLote.Repositories;

/// <summary>
/// Acceso a archivos de lote SPS (local o FTP) + utilidades de parseo y validación.
/// </summary>
internal static class SPSFileRepository
{
    // ==========================================================
    // 1) I/O BÁSICO  ────────────────────────────────────────────
    // ==========================================================

    /// <summary>
    /// Mueve un archivo al destino indicado (histórico o inválidos).
    /// Maneja rutas locales o FTP según configuración.
    /// </summary>
    internal static Generic MoveFile(string origin, string destination, string filename)
    {
        var result = new Generic();

        try
        {
            // Excepción opcional para debugging o ejecución single-transaction.
            if (origin.Contains("NOMOVE", StringComparison.OrdinalIgnoreCase) ||
                Program.ProcessSingleTxn)
            {
                result.Cadena = "Archivo marcado con NOMOVE o ejecución single-txn";
                result.Booleano = true;
                return result;
            }

            // ----------------- LOCAL FILESYSTEM -----------------
            if (!Program.IsFtpMethod)
            {
                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                File.Move(origin, Path.Combine(destination, filename));

                result.Booleano = false;
                return result;
            }

            // ------------------------- FTP ----------------------
            if (!FTPConnector.CreateDirectoryIfNotExists(destination))
            {
                result.Booleano = true;
                result.Cadena = "No se pudo crear el directorio FTP";
                return result;
            }

            // Mover en FTP
            result.Booleano = FTPConnector.MoveFile(origin, destination + filename);
            result.Cadena = "Moviendo archivo vía FTP";
            return result;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F03");
            Program.Print($"ERROR AL MOVER {origin} → {destination}", "ERROR");

            result.Booleano = true;
            result.Cadena = ex.Message;
            return result;
        }
    }

    /// <summary>
    /// Devuelve la lista de archivos disponibles.
    /// Si <paramref name="filterDate"/> tiene valor, sólo se devuelven los del día indicado.
    /// </summary>
    internal static List<FileInfoModel> GetAllFiles(DateTime? filterDate = null)
    {
        try
        {
            IEnumerable<FileInfoModel> files;

            if (!Program.IsFtpMethod)
            {
                var localDir = Program.FolderOrigin;
                var localFiles = Directory
                                .EnumerateFiles(localDir)
                                .Select(f => new FileInfo(f))
                                .Select(Extensions.GetFileInfo);

                files = localFiles;
            }
            else
            {
                files = FTPConnector.GetAllFTPFiles(Program.FolderOrigin);
            }

            // Filtra por fecha (AAAAMMDD del nombre) si corresponde
            if (filterDate is not null)
            {
                var target = filterDate.Value.Date;
                files = files.Where(f => f.DateCreated.Date == target);
            }

            return files.ToList();
        }
        catch (Exception ex)
        {
            Program.Print("ERROR AL LEER ORIGEN DE ARCHIVOS", "ERROR");
            Program.Logger.LogError(ex, "Error F04");
            return new List<FileInfoModel>();
        }
    }

    // ==========================================================
    // 2) METADATOS Y VALIDACIÓN BÁSICA DE NOMBRE  ──────────────
    // ==========================================================

    /// <summary>
    /// Extrae metadatos (idSite, productCode, fecha) desde el nombre del archivo.
    /// </summary>
    internal static FileInformation GetFileInformationFromName(string fileName)
    {
        try
        {
            // ej. "lote00120852_030625.1.txt"
            var parts = Path.GetFileNameWithoutExtension(fileName)[4..].Split('_');
            var dateAndProd = parts[1].Split('.');

            return new FileInformation
            {
                FileDatetime = DateTime.ParseExact(dateAndProd[0], "ddMMyy", null),
                FileDATE = dateAndProd[0],
                FileIDSITE = parts[0],
                FileName = fileName,
                FileProductCode = dateAndProd[1],
                IsValidFilename = true
            };
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F05");

            return new FileInformation
            {
                FileName = fileName,
                IsValidFilename = false
            };
        }
    }

    // ==========================================================
    // 3) VALIDACIÓN DE CONSISTENCIA DEL ARCHIVO  ───────────────
    // ==========================================================

    internal static List<SPSErrores> ValidateFile(SPSFileModel file)
    {
        var errors = new List<SPSErrores>();
        var trailer = file.Trailer;
        var productId = Convert.ToInt32(file.FileInfoSPS.FileProductCode);

        // --- Validaciones de trailer ---
        if (trailer.Filler != 0)
            errors.Add(MkError("El trailer tiene un filler incorrecto", trailer));

        if (file.Records.Count != trailer.CantidadDeRegistros)
            errors.Add(MkError("La cantidad de registros no coincide con la especificada en el trailer", trailer));

        if (trailer.IDMedioDePago != productId)
            errors.Add(MkError("El código de producto del trailer no corresponde con el del nombre del archivo", trailer));

        // --- Recuento por tipo de operación ---
        var counters = file.Records
                           .GroupBy(r => r.TipoOperacion)
                           .Select(g => new SPSCounter
                           {
                               TipoOperacion = g.Key,
                               CantidadTotal = g.Count(),
                               MontoTotal = g.Sum(c => c.Monto)
                           })
                           .ToDictionary(c => c.TipoOperacion);

        ValidateTrailerCounters(trailer, counters, errors);

        // --- Validaciones por registro ---
        foreach (var record in file.Records)
        {
            if (record.Filler != 0)
                errors.Add(MkError("El registro tiene un filler incorrecto", record));

            if (record.MedioDePago != productId)
                errors.Add(MkError("El código de producto del registro no corresponde con el del nombre del archivo", record));

            if (record.MedioDePago != trailer.IDMedioDePago)
                errors.Add(MkError("El código de producto del registro no corresponde con el del trailer", record));

            if (record.IDLOTE != trailer.IDLote)
                errors.Add(MkError("El número de lote del registro no corresponde con el del trailer", record));
        }

        // --- Duplicados ---
        var duplicatedTxnIds = file.Records
                                   .GroupBy(r => new { r.TransctionId, r.TipoOperacion })
                                   .Where(g => g.Count() > 1)
                                   .Select(g => g.Key.TransctionId)
                                   .ToArray();

        if (duplicatedTxnIds.Any())
        {
            errors.Add(new SPSErrores
            {
                ErrorMessage = "Existen transacciones duplicadas",
                ErrorLine = $"TNs: {string.Join(',', duplicatedTxnIds)}"
            });
        }

        return errors;

        // ---------- helpers locales ----------
        static SPSErrores MkError(string msg, object offendingObj) => new()
        {
            ErrorMessage = msg,
            ErrorLine = JsonConvert.SerializeObject(offendingObj)
        };

        static void ValidateTrailerCounters(
            SPSFileTrailer tr,
            IReadOnlyDictionary<string, SPSCounter> dict,
            ICollection<SPSErrores> errs)
        {
            long GetQty(string op) => dict.TryGetValue(op, out var c) ? c.CantidadTotal : 0;
            long GetAmount(string op) => dict.TryGetValue(op, out var c) ? c.MontoTotal : 0;

            if (tr.CantidadCompras != GetQty("C"))
                errs.Add(new SPSErrores { ErrorMessage = "La cantidad de transacciones aprobadas no coincide con el trailer" });

            if (tr.CantidadDevueltas != GetQty("D"))
                errs.Add(new SPSErrores { ErrorMessage = "La cantidad de transacciones devueltas no coincide con el trailer" });

            if (tr.CantidadAnuladas != GetQty("A"))
                errs.Add(new SPSErrores { ErrorMessage = "La cantidad de transacciones anuladas no coincide con el trailer" });

            if (tr.MontoCompras != GetAmount("C"))
                errs.Add(new SPSErrores { ErrorMessage = "El monto de transacciones aprobadas no coincide con el trailer" });

            if (tr.MontoDevueltas != GetAmount("D"))
                errs.Add(new SPSErrores { ErrorMessage = "El monto de transacciones devueltas no coincide con el trailer" });

            if (tr.MontoAnuladas != GetAmount("A"))
                errs.Add(new SPSErrores { ErrorMessage = "El monto de transacciones anuladas no coincide con el trailer" });
        }
    }

    // ==========================================================
    // 4) PARSEO COMPLETO DEL ARCHIVO  ──────────────────────────
    // ==========================================================

    internal static SPSFileModel ParseFile(string content)
    {
        var file = new SPSFileModel();
        var errors = new List<SPSErrores>();
        var recordLines = SplitLines(content);
        var lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

        var recordList = new List<SPSFileRecord>();
        var trailer = new SPSFileTrailer();

        try
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var prefix = line[..1];

                try
                {
                    switch (prefix)
                    {
                        case "T":
                            trailer = ParseTrailer(line);
                            break;

                        case "D":
                            recordList.Add(ParseRecord(line));
                            file.RecordsRead++;
                            break;

                        default:
                            errors.Add(new SPSErrores
                            {
                                ErrorMessage = "Se encontró un registro que no es ni trailer (T) ni default (D)",
                                ErrorLine = line
                            });
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Diferenciamos entre trailer y record para un mensaje más claro
                    var type = prefix == "T" ? "TRAILER" : "DEFAULT";
                    Program.Logger.LogError(ex, $"Error F0{(prefix == "T" ? "6" : "7")}");
                    errors.Add(new SPSErrores
                    {
                        ErrorMessage = $"Error al parsear un registro {type}",
                        ErrorExceptionMessage = ex.Message,
                        ErrorLine = line
                    });

                    if (prefix == "D")
                        file.RecordsNotRead++;
                }
            }

            if (!lines.Any())
                errors.Add(new SPSErrores { ErrorMessage = "El archivo se encuentra vacío" });

            file.Trailer = trailer;
            file.Records = recordList;
            file.FileErrorList = errors;
            file.HasFileError = errors.Any();
            return file;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F08");
            Program.Print("ERROR GENÉRICO DE PARSEO", "ERROR");
            return new SPSFileModel { HasFileError = true };
        }

        // -------------- helpers locales --------------
        static IEnumerable<string> SplitLines(string txt) => txt
            .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

        static SPSFileTrailer ParseTrailer(string line) => new()
        {
            CantidadDeRegistros = int.Parse(line.Substring(1, 10)),
            IDMedioDePago = int.Parse(line.Substring(11, 3)),
            IDLote = line.Substring(14, 3),
            CantidadCompras = int.Parse(line.Substring(17, 4)),
            MontoCompras = long.Parse(line.Substring(21, 12)),
            CantidadDevueltas = int.Parse(line.Substring(33, 4)),
            MontoDevueltas = long.Parse(line.Substring(37, 12)),
            CantidadAnuladas = int.Parse(line.Substring(49, 4)),
            MontoAnuladas = long.Parse(line.Substring(53, 12)),
            Filler = int.Parse(line.Substring(65, 35))
        };
    }

    // ==========================================================
    // 5) PARSEO DE LÍNEA INDIVIDUAL  ───────────────────────────
    // ==========================================================

    internal static SPSFileRecord ParseRecord(string line)
    {
        try
        {
            var record = new SPSFileRecord
            {
                TransctionId = long.Parse(line.Substring(1, 10)),
                OriginalTID = line.Substring(1, 10),
                MedioDePago = int.Parse(line.Substring(11, 3)),
                CardMask = line.Substring(14, 20),
                TipoOperacion = line.Substring(34, 1),
                EsCargo = line[34] == 'C',
                Fecha = DateTime.ParseExact(line.Substring(35, 8), "ddMMyyyy", null),
                Monto = long.Parse(line.Substring(43, 12)),
                OriginalOperationAmount = line.Substring(43, 12),
                AuthorizationCode = line.Substring(55, 6),
                NumeroDeCupon = line.Substring(61, 6),
                IDSITE = line.Substring(67, 15),
                IDLOTE = line.Substring(82, 3),
                Cuotas = int.Parse(line.Substring(85, 2)),
                FechaCierre = DateTime.ParseExact(line.Substring(87, 8), "ddMMyyyy", null),
                NumeroDeEstablecimiento = line.Substring(95, 30),
                Filler = 0
            };

            return record;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F09");
            throw;
        }
    }

    // ==========================================================
    // 6) UTILIDADES MENORES  ───────────────────────────────────
    // ==========================================================

    internal static Generic IsTextFileEmpty(string fileName)
    {
        var result = new Generic { Booleano = true };

        if (!Program.IsFtpMethod)
        {
            var fi = new FileInfo(fileName);
            if (fi.Length <= 6) return result;

            var txt = File.ReadAllText(fileName);
            result.Booleano = txt.Length == 0;
            result.Cadena = txt;
            return result;
        }

        var content = FTPConnector.ReadFile(fileName);
        if (content.Length <= 6) return result;

        result.Booleano = false;
        result.Cadena = content;
        return result;
    }

    // ==========================================================
    // 7) HELPERS PARA LINQ  ─────────────────────────────────────
    // ==========================================================

    internal static long CantidadCeroSiNulo(SPSCounter? source) =>
        source?.CantidadTotal ?? 0;

    internal static long MontoCeroSiNulo(SPSCounter? source) =>
        source?.MontoTotal ?? 0;
}
