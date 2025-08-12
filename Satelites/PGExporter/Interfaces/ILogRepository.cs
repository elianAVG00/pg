namespace PGExporter.Interfaces
{
    public interface ILogRepository
    {
        Task _print(string message, string result = null);

        void _printManual(string text, int speed);

        Task InsertException(string thread, Exception ex);

        Task InsertLog(string type, string thread, string message, string exception = "");
    }
}
