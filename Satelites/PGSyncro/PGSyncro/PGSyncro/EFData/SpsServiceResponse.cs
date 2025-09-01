using System;
using System.Collections.Generic;

namespace PGSyncro.EFData;

public partial class SpsServiceResponse
{
    public long Id { get; set; }

    public long? SvcReqId { get; set; }

    public string? Reason { get; set; }

    public string? Currency { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? AddressValidation { get; set; }

    public string? OrderCode { get; set; }

    public string? DeliveryName { get; set; }

    public DateTime? Date { get; set; }

    public string? PurchaserTelephone { get; set; }

    public string? DeliveryDistrict { get; set; }

    public string? AuthorizationCode { get; set; }

    public string? DeliveryCountry { get; set; }

    public int? Quotes { get; set; }

    public string? IsDateOfBirthValidated { get; set; }

    public string? IsDocNumValidated { get; set; }

    public string? Holder { get; set; }

    public string? Order { get; set; }

    public string? ZipNumDelivery { get; set; }

    public decimal? Amount { get; set; }

    public string? Card { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? PurchaserMail { get; set; }

    public string? IsGateNumValidated { get; set; }

    public string? DeliveryCity { get; set; }

    public string? IsDocTypeValidated { get; set; }

    public string? OperationNumber { get; set; }

    public string? DeliveryState { get; set; }

    public string? Result { get; set; }

    public string? DeliveryMessage { get; set; }

    public string? SiteParameter { get; set; }

    public string? DocType { get; set; }

    public string? DocTypeDescription { get; set; }

    public int? DocNumber { get; set; }

    public DateTime? ChargebackDate { get; set; }

    public string? ChargebackReason { get; set; }

    public string? ChargebackSite { get; set; }

    public string? AuthenticationResultVbv { get; set; }

    public string? VisibleCardNum { get; set; }

    public string? AditionalReason { get; set; }

    public string? TicketNum { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? IdMotivo { get; set; }

    public virtual SpsServiceRequest? SvcReq { get; set; }
}
