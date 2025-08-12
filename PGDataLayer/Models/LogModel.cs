using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PGDataLayer.Models
{
    public class LogModel
    {
        public LogType Type { get; set; }
        public string module { get; set; }
        public string message { get; set; }

public Exception exception { get; set; }
        public long? transaction { get; set; }

    }
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error,
        Exception,
        Security
    }
}