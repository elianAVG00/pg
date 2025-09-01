namespace PGSyncro.Models
{
    public class StatusModuleCode : Status
    {
        public int validatorId { get; set; }

        public string originalcode { get; set; }

        public string type { get; set; }
    }
}