using Microsoft.EntityFrameworkCore.ChangeTracking;
using PGExporter.EF;
using PGExporter.Interfaces;

namespace PGExporter.Repositories
{
    public class LogRepository(PaymentContext _context) : ILogRepository
    {
        public async Task InsertException(string thread, Exception ex)
        {
            await InsertLog("Error", thread, ex.Message, ex.InnerException?.Message);
        }

        public async Task InsertLog(string type, string thread, string message, string exception = "")
        {
            DateTime date = DateTime.Now;
            Logs log = new Logs();
            log.Date = date;
            log.Type = type;
            log.Thread = thread;
            log.Message = message;
            log.Exception = exception;
            log.Transaction = "";
            log.CreatedBy = Program.appversion;
            log.CreatedOn = DateTime.Now;
            EntityEntry<Logs> entityEntry = await _context.Logs.AddAsync(log);

            await _context.SaveChangesAsync();
        }

        public async Task _print(string message, string? result = null)
        {
            string? html = "";
            string? htmlcolor = "";
            if (result != null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[");
                switch (result)
                {
                    case "OK":
                        htmlcolor = "339966";
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "NO":
                        htmlcolor = "ff0000";
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "WARNING":
                        htmlcolor = "ff9900";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "ERROR":
                        htmlcolor = "ff0000";
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                Console.Write(result.ToUpper());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] ");
                html = string.Format("<tr><td><strong>[<span style=\"color: #{0};\">{1}</span>]</strong></td><td>{2}</tr>", htmlcolor, result.ToUpper(), message.ToUpper());
            }
            else
                html = string.Format("<tr><td>&nbsp;</td><td>{0}</tr>", message.ToUpper());

            string? information = string.Format("[{1}] {0}", message.ToUpper(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if (Program.auto)
            {
                Console.WriteLine(information);
                await this.InsertLog("Info", "PGExporter.LogRepository.Print", "Output", information);
            }
            else
                this._printManual(message.ToUpper(), 10);
            if (string.IsNullOrWhiteSpace(Program.ExportStatusMail))
            {
                html = null;
                htmlcolor = null;
                information = null;
            }
            else
            {
                Program.StatusMailBody += html;
                html = null;
                htmlcolor = null;
                information = null;
            }
        }

        public void _printManual(string text, int speed)
        {
            if (speed == 0)
            {
                Console.WriteLine(text);
            }
            else
            {
                int charcount = 0;
                string[] words = text.Split(' ');
                Task.Run(() =>
                {
                    foreach (string str in words)
                    {
                        foreach (char ch in str)
                        {
                            ++charcount;
                            Console.Write(ch);
                            Thread.Sleep(speed);
                        }
                        ++charcount;
                        if (charcount > 120)
                        {
                            charcount = 0;
                            Console.WriteLine(" ");
                        }
                        else
                            Console.Write(" ");
                        Thread.Sleep(30);
                    }
                }).Wait();
                Console.WriteLine("");
            }
        }
    }
}
