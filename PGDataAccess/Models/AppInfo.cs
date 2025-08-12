using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace PGDataAccess.Models
{
    [DataContract]
    public class AppInfo
    {
        [DataMember]
        public string DataBaseName { get; set; }
        [DataMember]
        public string DataBaseSource { get; set; }

        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public string DBVersion { get; set; }
        [DataMember]
        public string DASServerName { get; set; }
        [DataMember]
        public string DASPhysicalPath { get; set; }
        [DataMember]
        public string DASVirtualPath { get; set; }

        [DataMember]
        public string logInfo { get; set; }
        [DataMember]
        public string logError { get; set; }
        [DataMember]
        public string logDebug { get; set; }
        [DataMember]
        public string logWarning { get; set; }
        [DataMember]
        public string logOnDataBase { get; set; }



    }
}