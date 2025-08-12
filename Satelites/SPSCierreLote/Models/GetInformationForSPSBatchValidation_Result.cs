namespace SPSCierreLote.Models
{
    public class GetInformationForSPSBatchValidation_Result
    {
        public int SPSMedioDePago { get; set; }

        public DateTime SPSFechaOperacion { get; set; }

        public long? SPSMonto { get; set; }

        public long SPSIDSITE { get; set; }

        public bool SPSEsCargo { get; set; }

        public long? PGMonto { get; set; }

        public int? PGMedioDePago { get; set; }

        public DateTime? PGFechaOperacion { get; set; }

        public string PGUniqueCode { get; set; }

        public long? TransactionIdPK { get; set; }

        public long? TransactionNumber { get; set; }

        public int? TransactionResultInfoId { get; set; }

        public bool IsAmountOK { get; set; }

        public bool IsUniqueCodeOK { get; set; }

        public bool IsDateOK { get; set; }

        public bool IsCardCodeOK { get; set; }

        public bool IsDateGAPOK { get; set; }

        public bool IsDuplicatedOK { get; set; }
    }
}
