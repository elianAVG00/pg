using Newtonsoft.Json;
using SPSCierreLote.Models;
using SPSCierreLote.Repositories;

namespace SPSCierreLote;

/// <summary>
/// Motor que recorre, valida y persiste los archivos de cierre de lote SPS.
/// </summary>
internal static class Worker
{
    /// <summary>
    /// Cuando se activa por línea de comandos (`d:AAAAmmdd`) procesa sólo los
    /// archivos correspondientes a esa fecha.
    /// </summary>
    internal static bool ParameterDateFlag { get; set; }

    /// <summary>
    /// Fecha indicada en CLI; se ignora si <see cref="ParameterDateFlag"/> es false.
    /// </summary>
    internal static DateTime ParameterDate { get; set; }

    /// <summary>
    /// Loop principal: toma cada archivo pendiente y ejecuta todo el flujo
    /// (parseo → validación sintáctica → homologación BD → persistencia → archivado).
    /// </summary>
    internal static void ProcessFiles()
    {
        // 1) Obtiene la lista de archivos según fecha (o todos si no se filtró por parámetro).
        IEnumerable<FileInfoModel> candidateFiles = SPSFileRepository.GetAllFiles(ParameterDateFlag ? ParameterDate : (DateTime?)null);

        foreach (var fileInfo in candidateFiles)
        {
            Program.FilesFound++;

            // Sólo procesa la extensión configurada (ej. "1.txt")
            if (!string.Equals(fileInfo.Extension.TrimStart('.'),
                               Program.BatchFileExtension,
                               StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // 2) Alta en Monitor + validación del nombre (IDSite–ProductCode)
            var initResult = SPSMonitorDatabase.InitNewFile(fileInfo);

            if (initResult.skipThisFile)
            {
                MoveToInvalid(fileInfo);               // comercio no vigente, etc.
                continue;
            }

            Program.Print("===============================================", "OK");

            if (initResult.generico.Booleano)           // nombre inválido
            {
                Program.Print($"ARCHIVO INVÁLIDO: {initResult.FileInformation.FileName}", "ERROR");
                Program.ExecutionWithErrors = true;
                MoveToInvalid(fileInfo);
                continue;
            }

            Program.Print($"ARCHIVO VALIDADO: {initResult.FileInformation.FileName}", "OK");

            // 3) Parseo del archivo
            var parsedFile = SPSFileRepository.ParseFile(initResult.file_content);
            Program.FilesRead++;

            if (parsedFile.HasFileError)
            {
                Program.Print($"ARCHIVO CON ERRORES DE PARSEO: {initResult.FileInformation.FileName}", "ERROR");
                Program.ExecutionWithErrors = true;
                MoveToInvalid(fileInfo);
                continue;
            }

            Program.Print($"ARCHIVO PARSEADO: {initResult.FileInformation.FileName}", "OK");

            // 4) Validación sintáctica/comercial (opcional)
            if (Program.IsValidationActive)
            {
                Program.Print($"VALIDAR ARCHIVOS ESTÁ ACTIVADO - ARCHIVO: {initResult.FileInformation.FileName}", "OK");

                parsedFile.FileInfoSPS = initResult.FileInformation;
                List<SPSErrores> validationErrors = SPSFileRepository.ValidateFile(parsedFile).ToList();

                Program.Print($"PROCESO DE VALIDACIÓN EJECUTADO - ARCHIVO: {initResult.FileInformation.FileName}", "OK");

                if (initResult.FileInformation.FileName.Contains("NOSAVE", StringComparison.OrdinalIgnoreCase))
                {
                    Program.Print("ARCHIVO CON SUFIJO 'NOSAVE' – se omite homologación y persistencia", "NO");
                    continue;
                }

                if (validationErrors.Any())
                {
                    Program.Print("SE ENCONTRARON FALLOS DE VALIDACIÓN DE ARCHIVO", "ERROR");

                    Program.TotalRecordsNotRead += parsedFile.Records.Count;
                    Program.ExecutionWithErrors = true;
                    SPSMonitorDatabase.UpdateFileErrors(
                                                        monitorFileId: initResult.generico.Id,
                                                        errorsJson: JsonConvert.SerializeObject(validationErrors));
                    Program.Print("ROLLBACK", "OK");
                    MoveToInvalid(fileInfo);
                    continue;
                }
            }
            else
            {
                Program.Print($"VALIDAR ARCHIVOS ESTÁ DESACTIVADO - ARCHIVO: {initResult.FileInformation.FileName}", "NO");
            }

            // 5) Homologación con la base de datos
            Program.Print("INICIO DEL PROCESO DE HOMOLOGACIÓN DE DATOS", "OK");
            var validatedFile = SPSMonitorDatabase.ValidateDataInDatabase(parsedFile);
            Program.Print("PROCESO DE HOMOLOGACIÓN DE DATOS EJECUTADO", "OK");

            // 6) Persistencia de resultados
            Program.Print("GUARDANDO DATOS DE TRANSACCIONES…", "OK");
            var saveResult = SPSMonitorDatabase.SaveSPSBatchMainData(validatedFile, initResult.generico.Id);
            Program.Print("DATOS GUARDADOS - ARCHIVO PROCESADO", "OK");

            Program.TotalRecordsRead += validatedFile.Records.Count;

            if (saveResult.Booleano)
            {
                // Fallo al persistir: se consideran no leídos
                Program.TotalRecordsNotRead += validatedFile.Records.Count;
                Program.ExecutionWithErrors = true;
                MoveToInvalid(fileInfo);
                continue;
            }

            // 7) Cierre y archivado
            bool closeError = SPSMonitorDatabase.CloseNewFile(fileInfo, validatedFile, initResult.generico.Id).Booleano;

            if (closeError)
            {
                Program.Print($"CIERRE DEL PROCESO - ARCHIVO: {initResult.FileInformation.FileName}", "ERROR");
                Program.ExecutionWithErrors = true;
                MoveToInvalid(fileInfo);
            }
            else
            {
                Program.Print($"CIERRE DEL PROCESO - ARCHIVO: {initResult.FileInformation.FileName}", "OK");
                //ArchiveFile(fileInfo); este envia al archivo devuelta cheaquear si hay que borrarlo
            }
        }
    }

    /// <summary>
    /// Mueve el archivo procesado correctamente al histórico AAAA\MM\DD\.
    /// </summary>
    private static void ArchiveFile(FileInfoModel fileInfo)
    {
        string targetFolder = Program.SaveHistoricByActualDate
            ? $"{Program.FolderDestiny}{DateTime.Now:yyyy}\\{DateTime.Now:MM}\\{DateTime.Now:dd}\\"
            : $"{Program.FolderDestiny}{fileInfo.DateCreated:yyyy}\\{fileInfo.DateCreated:MM}\\{fileInfo.DateCreated:dd}\\";

        SPSFileRepository.MoveFile(fileInfo.FullName, targetFolder, fileInfo.Name);
    }

    /// <summary>
    /// Envía el archivo a la carpeta de inválidos con timestamp + Id de proceso.
    /// </summary>
    private static void MoveToInvalid(FileInfoModel fileInfo)
    {
        string targetFolder = $"{Program.FolderInvalid}{DateTime.Now:yyyyMMdd}.{Program.MonitorProcessId}\\";

        SPSFileRepository.MoveFile(fileInfo.FullName, targetFolder, fileInfo.Name);
    }
}
