using PGSyncro.Models;
using System.Collections.Generic;

namespace PGSyncro
{
    public static class AppConfig
    {
        public static string appVersion { get; set; }
        public static List<StatusModuleCode> codes { get; set; }
        public static List<int> servicesidWithConciliation { get; set; }
        public static bool FixHistoric { get; set; }
        public static long SincroId { get; set; }
        public static long TotalSincroFromUSP { get; set; }
        public static bool witherror { get; set; }
        public static SyncroStats stats { get; set; } = new SyncroStats();
        public static string SPSBatchNumberFixed { get; set; }
        public static bool ActivateSPSBatchNumberFixed { get; set; }
        public static DateTime FixedSPSBatchBeginDate { get; set; }
    }
}