namespace PGExporter.Models
{
    public class RenditionDataModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public DateTime? SPSBatch { get; set; }

        public int ValidatorId { get; set; }

        public long IDPK { get; set; }

        public bool IsIncomplete { get; set; }

        public string ChannelName { get; set; }

        public string EPC { get; set; }

        public long TransctionNumber { get; set; }

        public int ChannelId { get; set; }

        public string BatchNbr { get; set; }

        public string Email { get; set; }

        public int ProductId { get; set; }

        public Decimal Amount { get; set; }

        public string CardHolder { get; set; }

        public string CardMask { get; set; }

        public string InternalNbr { get; set; }

        public int Payments { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Code { get; set; }

        public Decimal CIAmount { get; set; }

        public string Description { get; set; }

        public int ServiceId { get; set; }

        public string MerchantId { get; set; }

        public string ProductName { get; set; }

        public string? TRI_AuthCode { get; set; }

        public string? TRI_CardNumber { get; set; }

        public string? TRI_TicketNumber { get; set; }

        public string TRI_BatchNbr { get; set; }

        public string? TRI_CardHolder { get; set; }

        public long? TRI_IdPK { get; set; }
    }
}
