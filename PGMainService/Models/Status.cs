namespace PGMainService.Models
{
    public class Status
    {
        public Status()
        {
            this.IsPaymentServiceOn = false;
            this.IsServiceOn = false;
            this.ErrorCode = 0;
            this.StateDescription = "";
            this.Version = 0L;
            this.VersionHash = "";
        }

        public string VersionHash { get; set; }

        public long Version { get; set; }

        public bool IsServiceOn { get; set; }

        public bool IsPaymentServiceOn { get; set; }

        public int ErrorCode { get; set; }

        public string StateDescription { get; set; }
    }

}