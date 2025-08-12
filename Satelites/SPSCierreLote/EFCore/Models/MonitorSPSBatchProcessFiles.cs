using System;
using System.Collections.Generic;

namespace SPSCierreLote.EFCore.models;

public partial class MonitorSPSBatchProcessFiles
{
    public long MonitorSPSBatchProcessFilesId { get; set; }

    public long MonitorSPSBatchProcessId { get; set; }

    public DateTime BeginOn { get; set; }

    public DateTime? EndOn { get; set; }

    public string Filename { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public string IDSITE { get; set; } = null!;

    public string FileDate { get; set; } = null!;

    public int TrailerRecords { get; set; }

    public int TrailerIdLote { get; set; }

    public int TrailerAutorizadas { get; set; }

    public int TrailerAnuladas { get; set; }

    public int TrailerDevueltas { get; set; }

    public long TrailerMontoAutorizadas { get; set; }

    public long TrailerMontoDevueltas { get; set; }

    public long TrailerMontoAnuladas { get; set; }

    public int TotalRecordsRead { get; set; }

    public int TotalRecordsNotRead { get; set; }

    public bool WithValidation { get; set; }

    public bool WithError { get; set; }

    public string? ValidationError { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public bool IsActive { get; set; }

    public string TicketNumberOnSupport { get; set; } = null!;

    public bool MovedToHistory { get; set; }

    public bool? HasInconsistenceError { get; set; }

    public bool? HasUnknownRecords { get; set; }

    public string? UnknownRecords { get; set; }

    public int TotalUnknownRecords { get; set; }

    public DateOnly? RealFileDate { get; set; }

    public int? ProductId { get; set; }
}
