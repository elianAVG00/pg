namespace PGExporter.Models
{
    public class ConciliationDataModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public bool IsIncomplete { get; set; }

        public long IdPK { get; set; }

        public long CommerceIdPK { get; set; }

        public string CommerceNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ExternalId { get; set; }

        public string CentralizerCode { get; set; }

        public Decimal Amount { get; set; }

        public string MerchantId { get; set; }

        public string Code { get; set; }

        public bool IsDebit { get; set; }

        public DateTime? SPSBatch { get; set; }

        public int ValidatorId { get; set; }

        public int ChannelId { get; set; }

        public int ServiceId { get; set; }

        public int ProductId { get; set; }

        public string AuthCode { get; set; }

        public string CardNumber { get; set; }

        public string TicketNumber { get; set; }

        public string BatchNbr { get; set; }

        public string? TRI_AuthCode { get; set; }

        public string? TRI_CardNumber { get; set; }

        public string? TRI_TicketNumber { get; set; }

        public string TRI_BatchNbr { get; set; }

        public string? TRI_CardHolder { get; set; }

        public long? TRI_IdPK { get; set; }
    }
}
