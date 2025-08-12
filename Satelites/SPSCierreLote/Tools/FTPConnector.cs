using FluentFTP;
using Microsoft.Extensions.Logging;
using SPSCierreLote.Models;
using System.Security.Authentication;
using System.Text;

namespace SPSCierreLote.Tools;

/// <summary>
/// Conector FTP usado por el batch de cierre de lote para listar, leer y mover
/// los archivos de SPS.  Envuelve FluentFTP con la configuración unificada del
/// proyecto y expone helpers sincronizados (el proceso es 100 % batch).
/// </summary>
internal static class FTPConnector
{
    // Configuración compartida para todas las conexiones
    private static readonly FtpConfig _cfg = new()
    {
        EncryptionMode = FtpEncryptionMode.Explicit,
        ValidateAnyCertificate = true,           // SPS usa certificados autofirmados
        SslProtocols = SslProtocols.Tls12,
        SocketKeepAlive = true,
        ConnectTimeout = 10_000,
        ReadTimeout = 30_000
    };

    // ==========================================================
    // 1) LISTAR ARCHIVOS ────────────────────────────────────────
    // ==========================================================
    internal static List<FileInfoModel> GetAllFTPFiles(string remoteFolder)
    {
        var files = new List<FileInfoModel>();

        try
        {
            using var ftp = BuildClient();
            ftp.Connect();

            foreach (var item in ftp.GetListing(remoteFolder))
            {
                if (item.Type != FtpObjectType.File) continue;

                files.Add(new FileInfoModel
                {
                    FullName = item.FullName,
                    Extension = Extensions.GetFileExtension(item.Name),
                    Name = item.Name,
                    // La fecha se rellena luego cuando haga falta
                });
            }

            ftp.Disconnect();
            return files;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F10 – Listando archivos vía FTP");
            Program.Print("ERROR AL LISTAR ARCHIVOS EN FTP", "ERROR");
            return files;                          // lista vacía = sin archivos
        }
    }

    // ==========================================================
    // 2) MOVER ARCHIVO (ORIGIN → DESTINO) ───────────────────────
    // ==========================================================
    internal static bool MoveFile(string origin, string destination)
    {
        try
        {
            using var ftp = BuildClient();
            ftp.Connect();

            // Crea jerarquía destino si no existe
            var destDir = Path.GetDirectoryName(destination) ?? "/";
            if (!ftp.DirectoryExists(destDir))
                ftp.CreateDirectory(destDir, true);

            bool ok = ftp.MoveFile(origin, destination, FtpRemoteExists.Overwrite);
            ftp.Disconnect();
            return ok;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F11 – Moviendo archivo FTP");
            Program.Print($"ERROR AL MOVER {origin} → {destination}", "ERROR");
            return false;
        }
    }

    // ==========================================================
    // 3) LEER CONTENIDO DE UN ARCHIVO ───────────────────────────
    // ==========================================================
    internal static string? ReadFile(string remoteFile)
    {
        try
        {
            using var ftp = BuildClient();
            ftp.Connect();

            byte[] bytes;
            bool ok = ftp.DownloadBytes(out bytes, remoteFile);
            ftp.Disconnect();

            return ok && bytes.Length > 0
                ? Encoding.UTF8.GetString(bytes)
                : null;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F12 – Leyendo archivo FTP");
            Program.Print($"ERROR AL LEER {remoteFile}", "ERROR");
            return null;
        }
    }

    // ==========================================================
    // 4) CREAR DIRECTORIO SI NO EXISTE ─────────────────────────
    // ==========================================================
    internal static bool CreateDirectoryIfNotExists(string remoteDir)
    {
        try
        {
            using var ftp = BuildClient();
            ftp.Connect();

            if (ftp.DirectoryExists(remoteDir))
                return true;

            bool ok = ftp.CreateDirectory(remoteDir, true);
            ftp.Disconnect();
            return ok;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F13 – Creando directorio FTP");
            Program.Print($"ERROR AL CREAR DIRECTORIO {remoteDir}", "ERROR");
            return false;
        }
    }

    // ==========================================================
    // 5) FACTORÍA DE CLIENTE ───────────────────────────────────
    // ==========================================================
    private static FtpClient BuildClient() =>
        new(Program.FtpUrl, Program.FtpUser, Program.FtpPassword, Program.FtpPort, _cfg)
        {
            Encoding = Encoding.UTF8
        };
}
