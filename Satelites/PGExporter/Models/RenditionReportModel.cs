namespace PGExporter.Models
{
    public class RenditionReportModel
    {
        public long MonitorFilesReportRecordsId { get; set; }

        public long IDPK { get; set; }

        public bool IsIncomplete { get; set; }

        public string ElectronicPaymentCode { get; set; }

        public long TrxID { get; set; }

        public string TrxSource { get; set; }

        public string BatchNbr { get; set; }

        public string CustomerId { get; set; }

        public string CustomerMail { get; set; }

        public string Producto { get; set; }

        public string Country { get; set; }

        public int Currency { get; set; }

        public Decimal Amount { get; set; }

        public string CardHolderName { get; set; }

        public string CardMask { get; set; }

        public string MerchantId { get; set; }

        public string MerchOrderId { get; set; }

        public string MerchTrxRef { get; set; }

        public int NumPayments { get; set; }

        public string Operation { get; set; }

        public DateTime PosDateTime { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public Decimal Price { get; set; }
    }
}
