namespace PGSyncro.Models
{
    public class SyncroSaveStatus
    {
        public int StatusCodeId { get; set; }

        public bool ItWasClose { get; set; }

        public bool StatusSaved { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
