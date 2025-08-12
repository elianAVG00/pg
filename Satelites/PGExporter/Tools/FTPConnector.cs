using FluentFTP;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using System.Text;

namespace PGExporter.Tools
{
    public static class FTPConnector
    {
        private static FtpConfig GetConfig()
        {
            return new FtpConfig()
            {
                EncryptionMode = FtpEncryptionMode.Explicit,
                ValidateAnyCertificate = true,
                SslProtocols = SslProtocols.Tls12,
                SocketKeepAlive = true
            };
        }

        public static List<FileInfoModel> GetAllFTPFiles(string folder)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Program._configuration.GetSection("FTP")["Host"], Program._configuration.GetSection("FTP")["User"], Program._configuration.GetSection("FTP")["Pass"], Program._configuration.GetSection("FTP").GetValue<int>("Port"), FTPConnector.GetConfig()))
                {
                    ftpClient.AutoConnect();
                    List<FileInfoModel> allFtpFiles = new List<FileInfoModel>();
                    foreach (FtpListItem ftpListItem in ftpClient.GetListing(folder))
                    {
                        if (ftpListItem.Type == FtpObjectType.File)
                            allFtpFiles.Add(new FileInfoModel()
                            {
                                FullName = ftpListItem.FullName,
                                Extension = PGExporter.Tools.Extensions.GetFileExtension(ftpListItem.Name),
                                Name = ftpListItem.Name
                            });
                    }
                    ftpClient.Disconnect();
                    return allFtpFiles;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UploadFile(string destination, string filename, string content)
        {
            try
            {
                bool directoryIfNotExists = FTPConnector.CreateDirectoryIfNotExists(destination);
                if (!directoryIfNotExists)
                    return directoryIfNotExists;
                using (FtpClient ftpClient = new FtpClient(Program._configuration.GetSection("FTP")["Host"], Program._configuration.GetSection("FTP")["User"], Program._configuration.GetSection("FTP")["Pass"], Program._configuration.GetSection("FTP").GetValue<int>("Port"), FTPConnector.GetConfig()))
                {
                    ftpClient.AutoConnect();
                    byte[] bytes = Encoding.ASCII.GetBytes(content);
                    FtpStatus ftpStatus = ftpClient.UploadBytes(bytes, destination + filename, FtpRemoteExists.Overwrite, false, (Action<FtpProgress>)null);
                    ftpClient.Disconnect();
                    return ftpStatus == FtpStatus.Success;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool MoveFile(string origin, string destination)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Program._configuration.GetSection("FTP")["Host"], Program._configuration.GetSection("FTP")["User"], Program._configuration.GetSection("FTP")["Pass"], Program._configuration.GetSection("FTP").GetValue<int>("Port"), FTPConnector.GetConfig()))
                {
                    ftpClient.AutoConnect();
                    bool flag = ftpClient.MoveFile(origin, destination, FtpRemoteExists.Overwrite);
                    ftpClient.Disconnect();
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ReadFile(string filename)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Program._configuration.GetSection("FTP")["Host"], Program._configuration.GetSection("FTP")["User"], Program._configuration.GetSection("FTP")["Pass"], Program._configuration.GetSection("FTP").GetValue<int>("Port"), FTPConnector.GetConfig()))
                {
                    ftpClient.AutoConnect();
                    byte[] outBytes;
                    string str = !ftpClient.DownloadBytes(out outBytes, filename, 0L, (Action<FtpProgress>)null, 0L) ? (string)null : Encoding.UTF8.GetString(outBytes);
                    ftpClient.Disconnect();
                    return str;
                }
            }
            catch (Exception ex)
            {
                return (string)null;
            }
        }

        public static bool CreateDirectoryIfNotExists(string directory)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Program._configuration.GetSection("FTP")["Host"], Program._configuration.GetSection("FTP")["User"], Program._configuration.GetSection("FTP")["Pass"], Program._configuration.GetSection("FTP").GetValue<int>("Port"), FTPConnector.GetConfig()))
                {
                    ftpClient.AutoConnect();
                    return ftpClient.DirectoryExists(directory) || ftpClient.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
