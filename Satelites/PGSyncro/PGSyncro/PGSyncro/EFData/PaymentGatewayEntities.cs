using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PGSyncro.EFData;

public partial class PaymentGatewayEntities : DbContext
{
    public PaymentGatewayEntities()
    {
    }

    public PaymentGatewayEntities(DbContextOptions<PaymentGatewayEntities> options)
        : base(options)
    {
    }

    public virtual DbSet<AnnulmentJobLog> AnnulmentJobLogs { get; set; }

    public virtual DbSet<AnnulmentRequest> AnnulmentRequests { get; set; }

    public virtual DbSet<AnnulmentResultInfo> AnnulmentResultInfos { get; set; }

    public virtual DbSet<AnnulmentValidatorComm> AnnulmentValidatorComms { get; set; }

    public virtual DbSet<AppConfig> AppConfigs { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<CarpendiProccess> CarpendiProccesses { get; set; }

    public virtual DbSet<CarpendiStat> CarpendiStats { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<Channel1> Channels1 { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<ClaimOperation> ClaimOperations { get; set; }

    public virtual DbSet<ClaimStatus> ClaimStatuses { get; set; }

    public virtual DbSet<ClaimType> ClaimTypes { get; set; }

    public virtual DbSet<Claimer> Claimers { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Client1> Clients1 { get; set; }

    public virtual DbSet<CodeMapping> CodeMappings { get; set; }

    public virtual DbSet<CommerceItem> CommerceItems { get; set; }

    public virtual DbSet<CommerceItemClaim> CommerceItemClaims { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Configuration1> Configurations1 { get; set; }

    public virtual DbSet<ConfigurationDefaultValue> ConfigurationDefaultValues { get; set; }

    public virtual DbSet<ConfigurationDetail> ConfigurationDetails { get; set; }

    public virtual DbSet<ConfigurationsBk0606> ConfigurationsBk0606s { get; set; }

    public virtual DbSet<ConfigurationsBk2606> ConfigurationsBk2606s { get; set; }

    public virtual DbSet<ConfigurationsBk2706> ConfigurationsBk2706s { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Currency1> Currencies1 { get; set; }

    public virtual DbSet<CustomMessage> CustomMessages { get; set; }

    public virtual DbSet<DocType> DocTypes { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<ExternalApp> ExternalApps { get; set; }

    public virtual DbSet<GenericCode> GenericCodes { get; set; }

    public virtual DbSet<GetPrismaCode> GetPrismaCodes { get; set; }

    public virtual DbSet<JobRunLog> JobRunLogs { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Language1> Languages1 { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Log1> Logs1 { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ModuleCode> ModuleCodes { get; set; }

    public virtual DbSet<MonitorFilesReportProcess> MonitorFilesReportProcesses { get; set; }

    public virtual DbSet<MonitorFilesReportRecord> MonitorFilesReportRecords { get; set; }

    public virtual DbSet<MonitorPaywayBackOfficeTransaction> MonitorPaywayBackOfficeTransactions { get; set; }

    public virtual DbSet<MonitorSpsbatchProcess> MonitorSpsbatchProcesses { get; set; }

    public virtual DbSet<MonitorSpsbatchProcessFile> MonitorSpsbatchProcessFiles { get; set; }

    public virtual DbSet<MonitorSpsbatchProcessTransaction> MonitorSpsbatchProcessTransactions { get; set; }

    public virtual DbSet<MonitorSyncroProcess> MonitorSyncroProcesses { get; set; }

    public virtual DbSet<MonitorSyncroProcessRecord> MonitorSyncroProcessRecords { get; set; }

    public virtual DbSet<NotificationConfig> NotificationConfigs { get; set; }

    public virtual DbSet<NotificationLog> NotificationLogs { get; set; }

    public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    public virtual DbSet<NpsServiceRequest> NpsServiceRequests { get; set; }

    public virtual DbSet<NpsServiceResponse> NpsServiceResponses { get; set; }

    public virtual DbSet<NpsServiceResponseQueryTrx> NpsServiceResponseQueryTrxs { get; set; }

    public virtual DbSet<PaymentClaim> PaymentClaims { get; set; }

    public virtual DbSet<PaymentClaimStatus> PaymentClaimStatuses { get; set; }

    public virtual DbSet<PaymentValidatorComm> PaymentValidatorComms { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Product1> Products1 { get; set; }

    public virtual DbSet<ProductCarpendi> ProductCarpendis { get; set; }

    public virtual DbSet<ProductCentralizer> ProductCentralizers { get; set; }

    public virtual DbSet<ProductSpsbatch> ProductSpsbatches { get; set; }

    public virtual DbSet<ProductsValidator> ProductsValidators { get; set; }

    public virtual DbSet<Pswd> Pswds { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<RefundsRecord> RefundsRecords { get; set; }

    public virtual DbSet<ReportJobLog> ReportJobLogs { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SalePoint> SalePoints { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Service1> Services1 { get; set; }

    public virtual DbSet<ServiceChannel> ServiceChannels { get; set; }

    public virtual DbSet<ServiceProduct> ServiceProducts { get; set; }

    public virtual DbSet<ServicesConfig> ServicesConfigs { get; set; }

    public virtual DbSet<SpsBatch> SpsBatches { get; set; }

    public virtual DbSet<SpsBatchTemporalValidation> SpsBatchTemporalValidations { get; set; }

    public virtual DbSet<SpsServiceRequest> SpsServiceRequests { get; set; }

    public virtual DbSet<SpsServiceResponse> SpsServiceResponses { get; set; }

    public virtual DbSet<StatusCode> StatusCodes { get; set; }

    public virtual DbSet<StatusMessage> StatusMessages { get; set; }

    public virtual DbSet<StatusSvcLog> StatusSvcLogs { get; set; }

    public virtual DbSet<StatusTemplate> StatusTemplates { get; set; }

    public virtual DbSet<TicketLog> TicketLogs { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionAdditionalInfo> TransactionAdditionalInfos { get; set; }

    public virtual DbSet<TransactionResultInfo> TransactionResultInfos { get; set; }

    public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

    public virtual DbSet<TransactionTicket> TransactionTickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivity> UserActivities { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    public virtual DbSet<UserService> UserServices { get; set; }

    public virtual DbSet<Validator> Validators { get; set; }

    public virtual DbSet<Validator1> Validators1 { get; set; }

    public virtual DbSet<ValidatorMessage> ValidatorMessages { get; set; }

    public virtual DbSet<ValidatorServiceConfig> ValidatorServiceConfigs { get; set; }

    public virtual DbSet<Version> Versions { get; set; }

    public virtual DbSet<VersionLog> VersionLogs { get; set; }

    public virtual DbSet<VwGetPaymentTicketInfo> VwGetPaymentTicketInfos { get; set; }

    public virtual DbSet<VwNpsServiceTransaction> VwNpsServiceTransactions { get; set; }

    public virtual DbSet<VwNpsServiceTransactionsAaa> VwNpsServiceTransactionsAaas { get; set; }

    public virtual DbSet<WebSvcMethod> WebSvcMethods { get; set; }

    public virtual DbSet<WebSvcMethodParam> WebSvcMethodParams { get; set; }

    public virtual DbSet<GetTransactionsToSync_Result> GetTransactionsToSyncResults { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GetTransactionsToSync_Result>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<AnnulmentJobLog>(entity =>
        {
            entity.ToTable("AnnulmentJobLog", "claim");

            entity.HasOne(d => d.AnnulmentRequest).WithMany(p => p.AnnulmentJobLogs)
                .HasForeignKey(d => d.AnnulmentRequestId)
                .HasConstraintName("FK_AnnulmentJobLog_AnnulmentRequest");

            entity.HasOne(d => d.JobRunLog).WithMany(p => p.AnnulmentJobLogs)
                .HasForeignKey(d => d.JobRunLogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnulmentJobLog_JobRunLog");

            entity.HasOne(d => d.PaymentClaim).WithMany(p => p.AnnulmentJobLogs)
                .HasForeignKey(d => d.PaymentClaimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnulmentJobLog_PaymentClaim");
        });

        modelBuilder.Entity<AnnulmentRequest>(entity =>
        {
            entity.ToTable("AnnulmentRequest", "claim");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ResponseModuleCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Result)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentClaim).WithMany(p => p.AnnulmentRequests)
                .HasForeignKey(d => d.PaymentClaimId)
                .HasConstraintName("FK_AnnulmentRequest_PaymentClaim");
        });

        modelBuilder.Entity<AnnulmentResultInfo>(entity =>
        {
            entity.ToTable("AnnulmentResultInfo", "claim");

            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OperationNumber)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OriginalDateTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentClaim).WithMany(p => p.AnnulmentResultInfos)
                .HasForeignKey(d => d.PaymentClaimId)
                .HasConstraintName("FK_AnnulmentResultInfo_PaymentClaim");

            entity.HasOne(d => d.Transaction).WithMany(p => p.AnnulmentResultInfos)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnulmentResultInfo_Transactions");

            entity.HasOne(d => d.Validator).WithMany(p => p.AnnulmentResultInfos)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnnulmentResultInfo_Validators");
        });

        modelBuilder.Entity<AnnulmentValidatorComm>(entity =>
        {
            entity.ToTable("AnnulmentValidatorComm", "claim");

            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.RequestMessage)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ResponseDate).HasColumnType("datetime");
            entity.Property(e => e.ResponseMessage)
                .HasMaxLength(8000)
                .IsUnicode(false);

            entity.HasOne(d => d.AnnulmentRequest).WithMany(p => p.AnnulmentValidatorComms)
                .HasForeignKey(d => d.AnnulmentRequestId)
                .HasConstraintName("FK_AnnulmentValidatorComm_AnnulmentRequest");
        });

        modelBuilder.Entity<AppConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId);

            entity.ToTable("AppConfig", "common");

            entity.Property(e => e.ConfigType)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DeleteTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Setting)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.Value).IsUnicode(false);
            entity.Property(e => e.ViewSpecialBackgroundColor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("white");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
        });

        modelBuilder.Entity<CarpendiProccess>(entity =>
        {
            entity.ToTable("CarpendiProccess", "common");

            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.Filename)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<CarpendiStat>(entity =>
        {
            entity.HasKey(e => e.CarpendiStatsId);

            entity.ToTable("CarpendiStats", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DataRelated).IsUnicode(false);
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.ExceptionMessage).IsUnicode(false);
            entity.Property(e => e.InnerExceptionMessage).IsUnicode(false);
            entity.Property(e => e.Method)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TransactionIdPkrelated).HasColumnName("TransactionIdPKRelated");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.ToTable("Channels", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Channel1>(entity =>
        {
            entity.ToTable("Channels");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.Property(e => e.ClaimExternalApp)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ClosedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClosedOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CrmCaseId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DocumentNbr)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Mobile)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdCrmcase).HasColumnName("UpdCRMCase");
            entity.Property(e => e.UpdCrminvoice).HasColumnName("UpdCRMInvoice");

            entity.HasOne(d => d.ClaimStatus).WithMany(p => p.Claims)
                .HasForeignKey(d => d.ClaimStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Claims_ClaimStatus");

            entity.HasOne(d => d.ClaimType).WithMany(p => p.Claims)
                .HasForeignKey(d => d.ClaimTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Claims_ClaimTypes");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Claims)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Claims_DocumentTypes");
        });

        modelBuilder.Entity<ClaimOperation>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Claim).WithMany(p => p.ClaimOperations)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClaimOperations_Claims");

            entity.HasOne(d => d.Product).WithMany(p => p.ClaimOperations)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClaimOperations_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.ClaimOperations)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClaimOperations_Services");
        });

        modelBuilder.Entity<ClaimStatus>(entity =>
        {
            entity.ToTable("ClaimStatus");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ClaimType>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Claimer>(entity =>
        {
            entity.ToTable("Claimer", "claim");

            entity.Property(e => e.Cellphone)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.DocType).WithMany(p => p.Claimers)
                .HasForeignKey(d => d.DocTypeId)
                .HasConstraintName("FK_Claimer_DocType");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Clients_1");

            entity.ToTable("Clients", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LegalName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupportMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TributaryCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Client1>(entity =>
        {
            entity.ToTable("Clients");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LegalName).HasMaxLength(250);
            entity.Property(e => e.MailAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MailPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MailUserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ReturnUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SettingId).HasDefaultValue(1);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.TributaryCode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<CodeMapping>(entity =>
        {
            entity.ToTable("CodeMapping", "status");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.ModuleCode).WithMany(p => p.CodeMappings)
                .HasForeignKey(d => d.ModuleCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeMapping_ModuleCode");

            entity.HasOne(d => d.StatusCode).WithMany(p => p.CodeMappings)
                .HasForeignKey(d => d.StatusCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CodeMapping_StatusCode");
        });

        modelBuilder.Entity<CommerceItem>(entity =>
        {
            entity.HasKey(e => e.CommerceItemsId);

            entity.ToTable("CommerceItems", "payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OriginalCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReportDateCentralizer).HasColumnType("datetime");
            entity.Property(e => e.ReportDateConciliation).HasColumnType("datetime");
            entity.Property(e => e.ReportDateRendition).HasColumnType("datetime");
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.TransactionIdPkNavigation).WithMany(p => p.CommerceItems)
                .HasForeignKey(d => d.TransactionIdPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentCommerceItems_Transactions");
        });

        modelBuilder.Entity<CommerceItemClaim>(entity =>
        {
            entity.ToTable("CommerceItemClaim", "claim");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CommerceItem).WithMany(p => p.CommerceItemClaims)
                .HasForeignKey(d => d.CommerceItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommerceItemClaim_CommerceItems");

            entity.HasOne(d => d.PaymentClaim).WithMany(p => p.CommerceItemClaims)
                .HasForeignKey(d => d.PaymentClaimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CommerceItemClaim_PaymentClaim");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.ToTable("Configurations", "common");

            entity.HasIndex(e => new { e.ServiceId, e.ChannelId, e.ProductId, e.ValidatorId }, "IX_Configurations").IsUnique();

            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Channel).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Configurations_Channels");

            entity.HasOne(d => d.Product).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Configurations_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Configurations_Services");

            entity.HasOne(d => d.Validator).WithMany(p => p.Configurations)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Configurations_Validators");
        });

        modelBuilder.Entity<Configuration1>(entity =>
        {
            entity.ToTable("Configurations");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FmDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OperatorEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");

            entity.HasOne(d => d.ServiceChannel).WithMany(p => p.Configuration1s)
                .HasForeignKey(d => d.ServiceChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Config_ServiceChannels");
        });

        modelBuilder.Entity<ConfigurationDefaultValue>(entity =>
        {
            entity.Property(e => e.CastToClrtype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CastToCLRType");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DefaultValue).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Configuration).WithMany(p => p.ConfigurationDefaultValues)
                .HasForeignKey(d => d.ConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDV_Configurations");

            entity.HasOne(d => d.WebSvcMethod).WithMany(p => p.ConfigurationDefaultValues)
                .HasForeignKey(d => d.WebSvcMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDV_WebSvcMethods");

            entity.HasOne(d => d.WebSvcMethodParam).WithMany(p => p.ConfigurationDefaultValues)
                .HasForeignKey(d => d.WebSvcMethodParamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDV_WebSvcMethodParams");
        });

        modelBuilder.Entity<ConfigurationDetail>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.BckpValidator).WithMany(p => p.ConfigurationDetailBckpValidators)
                .HasForeignKey(d => d.BckpValidatorId)
                .HasConstraintName("FK_ConfigDetails_BckpValidators");

            entity.HasOne(d => d.BckpValidatorMethod).WithMany(p => p.ConfigurationDetailBckpValidatorMethods)
                .HasForeignKey(d => d.BckpValidatorMethodId)
                .HasConstraintName("FK_ConfigDetails_BckpWebSvcMethods");

            entity.HasOne(d => d.Configuration).WithMany(p => p.ConfigurationDetails)
                .HasForeignKey(d => d.ConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigurationDetails_Configurations");

            entity.HasOne(d => d.MainValidator).WithMany(p => p.ConfigurationDetailMainValidators)
                .HasForeignKey(d => d.MainValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_MainValidators");

            entity.HasOne(d => d.MainValidatorMethod).WithMany(p => p.ConfigurationDetailMainValidatorMethods)
                .HasForeignKey(d => d.MainValidatorMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_MainWebSvcMethods");

            entity.HasOne(d => d.Product).WithMany(p => p.ConfigurationDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_Products");
        });

        modelBuilder.Entity<ConfigurationsBk0606>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configurations-BK-06-06", "common");

            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ConfigurationsBk2606>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configurations-BK-26-06", "common");

            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<ConfigurationsBk2706>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configurations-BK-27-06", "common");

            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("Currency", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsoCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Currency1>(entity =>
        {
            entity.HasKey(e => e.Iso);

            entity.ToTable("Currencies");

            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
        });

        modelBuilder.Entity<CustomMessage>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.En)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("EN");
            entity.Property(e => e.Es)
                .HasMaxLength(4000)
                .HasColumnName("ES");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<DocType>(entity =>
        {
            entity.ToTable("DocType", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
        });

        modelBuilder.Entity<ExternalApp>(entity =>
        {
            entity.ToTable("ExternalApp", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<GenericCode>(entity =>
        {
            entity.ToTable("GenericCode", "status");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<GetPrismaCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GetPrismaCodes");

            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JobRunLog>(entity =>
        {
            entity.HasKey(e => e.JobRunLogId).HasName("PK_AnnulmentJobs");

            entity.ToTable("JobRunLog", "common");

            entity.Property(e => e.DateFinish).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Parameters)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SendNotificationTo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("Language", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Iso3166)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISO3166");
            entity.Property(e => e.Iso6391)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("ISO6391");
            entity.Property(e => e.Iso6392)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISO6392");
            entity.Property(e => e.NativeName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Language1>(entity =>
        {
            entity.ToTable("Languages");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.Iso3166)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO3166");
            entity.Property(e => e.Iso639)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO639");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Exception).IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.Thread)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log1>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("Logs", "security");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdOn");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Exception).IsUnicode(false);
            entity.Property(e => e.InnerException).IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.Thread)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Transaction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.ToTable("Module", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.ValidatorId).HasColumnName("validatorId");
        });

        modelBuilder.Entity<ModuleCode>(entity =>
        {
            entity.ToTable("ModuleCode", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OriginalCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TechnicalInfo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Module).WithMany(p => p.ModuleCodes)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModuleCode_Module");
        });

        modelBuilder.Entity<MonitorFilesReportProcess>(entity =>
        {
            entity.ToTable("MonitorFilesReportProcess", "common");

            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndDataOn).HasColumnType("datetime");
            entity.Property(e => e.EndProcessOn).HasColumnType("datetime");
            entity.Property(e => e.Error)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.RemoteFile)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MonitorFilesReportRecord>(entity =>
        {
            entity.HasKey(e => e.MonitorFilesReportRecordsId);

            entity.ToTable("MonitorFilesReportRecords", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsIncomplete).HasDefaultValue(true);
            entity.Property(e => e.IsTotalizer).HasDefaultValue(true);
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
        });

        modelBuilder.Entity<MonitorPaywayBackOfficeTransaction>(entity =>
        {
            entity.HasKey(e => e.MonitorSpsbatchProcessTransactionsId).HasName("PK_MonitorSPSBatchProcessTransactionsId");

            entity.ToTable("MonitorPaywayBackOfficeTransactions", "common");

            entity.Property(e => e.MonitorSpsbatchProcessTransactionsId).HasColumnName("MonitorSPSBatchProcessTransactionsId");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PaywayStatus)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Pgamount).HasColumnName("PGAmount");
            entity.Property(e => e.PgnewStatusId).HasColumnName("PGNewStatusId");
            entity.Property(e => e.PgoldStatusId).HasColumnName("PGOldStatusId");
            entity.Property(e => e.TransactionIdpk).HasColumnName("TransactionIDPK");
        });

        modelBuilder.Entity<MonitorSpsbatchProcess>(entity =>
        {
            entity.ToTable("MonitorSPSBatchProcess", "common");

            entity.Property(e => e.MonitorSpsbatchProcessId).HasColumnName("MonitorSPSBatchProcessId");
            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.IsFtp)
                .HasDefaultValue(false)
                .HasColumnName("IsFTP");
        });

        modelBuilder.Entity<MonitorSpsbatchProcessFile>(entity =>
        {
            entity.HasKey(e => e.MonitorSpsbatchProcessFilesId);

            entity.ToTable("MonitorSPSBatchProcessFiles", "common");

            entity.Property(e => e.MonitorSpsbatchProcessFilesId).HasColumnName("MonitorSPSBatchProcessFilesId");
            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.FileDate)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Filename)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.HasInconsistenceError).HasDefaultValue(false);
            entity.Property(e => e.HasUnknownRecords).HasDefaultValue(false);
            entity.Property(e => e.Idsite)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IDSITE");
            entity.Property(e => e.MonitorSpsbatchProcessId).HasColumnName("MonitorSPSBatchProcessId");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TicketNumberOnSupport)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UnknownRecords).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ValidationError).IsUnicode(false);
        });

        modelBuilder.Entity<MonitorSpsbatchProcessTransaction>(entity =>
        {
            entity.HasKey(e => e.MonitorSpsbatchProcessTransactionsId);

            entity.ToTable("MonitorSPSBatchProcessTransactions", "common");

            entity.Property(e => e.MonitorSpsbatchProcessTransactionsId).HasColumnName("MonitorSPSBatchProcessTransactionsId");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.HasInconsistenceError).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceCard)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceCARD");
            entity.Property(e => e.InconsistenceCost)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceCOST");
            entity.Property(e => e.InconsistenceDate)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceDATE");
            entity.Property(e => e.InconsistenceDays)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceDAYS");
            entity.Property(e => e.InconsistenceDups)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceDUPS");
            entity.Property(e => e.InconsistenceError).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.InconsistenceUniq)
                .HasDefaultValue(false)
                .HasColumnName("InconsistenceUNIQ");
            entity.Property(e => e.MonitorSpsbatchProcessFilesId).HasColumnName("MonitorSPSBatchProcessFilesId");
            entity.Property(e => e.TransactionIdpk).HasColumnName("TransactionIDPK");
        });

        modelBuilder.Entity<MonitorSyncroProcess>(entity =>
        {
            entity.ToTable("MonitorSyncroProcess", "common");

            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.TotalTransactionsClosedError).HasColumnName("TotalTransactionsClosedERROR");
            entity.Property(e => e.TotalTransactionsClosedNa).HasColumnName("TotalTransactionsClosedNA");
            entity.Property(e => e.TotalTransactionsClosedNabyTimeout).HasColumnName("TotalTransactionsClosedNAByTIMEOUT");
            entity.Property(e => e.TotalTransactionsClosedNo).HasColumnName("TotalTransactionsClosedNO");
            entity.Property(e => e.TotalTransactionsClosedOk).HasColumnName("TotalTransactionsClosedOK");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<MonitorSyncroProcessRecord>(entity =>
        {
            entity.HasKey(e => e.MonitorSyncroProcessRecordsId);

            entity.ToTable("MonitorSyncroProcessRecords", "common");

            entity.Property(e => e.BeginProcess).HasColumnType("datetime");
            entity.Property(e => e.ClosedByProcess).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndProcess).HasColumnType("datetime");
            entity.Property(e => e.Error)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.SpsbatchForced).HasColumnName("SPSBatchForced");
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<NotificationConfig>(entity =>
        {
            entity.ToTable("NotificationConfig", "notification");

            entity.Property(e => e.AdditionalFooter)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.AdditionalHeader)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.NotificationConfigs)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_NotificationConfig_Services");

            entity.HasOne(d => d.StatusTemplate).WithMany(p => p.NotificationConfigs)
                .HasForeignKey(d => d.StatusTemplateId)
                .HasConstraintName("FK_NotificationConfig_StatusTemplate");
        });

        modelBuilder.Entity<NotificationLog>(entity =>
        {
            entity.ToTable("NotificationLog", "notification");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.HtmlNotification)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.TypeFormat)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.NotificationLogs)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NotificationLog_Module");

            entity.HasOne(d => d.TicketLog).WithMany(p => p.NotificationLogs)
                .HasForeignKey(d => d.TicketLogId)
                .HasConstraintName("FK_NotificationLog_TicketLog");
        });

        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.ToTable("NotificationTemplate", "notification");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.TemplateBody)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.TemplateSubject)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TemplateTicket)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<NpsServiceRequest>(entity =>
        {
            entity.Property(e => e.CardExpDate)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.CustomerCountry)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocNbr)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CustomerFirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.CustomerLastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerLocality)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.CustomerMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerProvince)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FrmBackButtonUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FrmLanguage)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.MerchOrderId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchTrxRef)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.MerchantMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Plan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PosDateTime).HasColumnType("datetime");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.QueryCriteria)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.QueryCriteriaId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.ReturnUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ScreenDescription)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SoftDescriptor)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TicketDescription)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.TrxSource)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Version)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NpsServiceResponse>(entity =>
        {
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.BarCode)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.ClResponseMessage).HasMaxLength(255);
            entity.Property(e => e.Country)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocNbr)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocType)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.CustomerMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Extended).HasMaxLength(255);
            entity.Property(e => e.FrontPspurl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FrontPSPUrl");
            entity.Property(e => e.MerchOrderId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchTrxRef)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.MerchantMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Plan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PosDateTime).HasColumnType("datetime");
            entity.Property(e => e.QueryCriteria)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.QueryCriteriaId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.ScreenDescription)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Session)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.TicketDescription)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.SvcReq).WithMany(p => p.NpsServiceResponses)
                .HasForeignKey(d => d.SvcReqId)
                .HasConstraintName("FK_NpsServiceResponses_NpsServiceRequests");
        });

        modelBuilder.Entity<NpsServiceResponseQueryTrx>(entity =>
        {
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CardMask)
                .HasMaxLength(19)
                .IsUnicode(false);
            entity.Property(e => e.CardNbrFsd)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CardNbrLfd)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ClResponseMessage).HasMaxLength(255);
            entity.Property(e => e.Country)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.CustomerMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Extended).HasMaxLength(255);
            entity.Property(e => e.MerchOrderId)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchTrxRef)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.MerchantMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Operation)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Plan)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PosDateTime).HasColumnType("datetime");
            entity.Property(e => e.PurchaseDescription).HasMaxLength(255);
            entity.Property(e => e.TrxSource)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.SvcRes).WithMany(p => p.NpsServiceResponseQueryTrxes)
                .HasForeignKey(d => d.SvcResId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NpsServiceResponseQueryTrxs_NpsServiceResponses");
        });

        modelBuilder.Entity<PaymentClaim>(entity =>
        {
            entity.ToTable("PaymentClaim", "claim");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsAutomatic).HasDefaultValue(true);
            entity.Property(e => e.Observation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PaymentClaimNumber).HasDefaultValueSql("(NEXT VALUE FOR [common].[PaymentClaimNumberSequence])");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Claimer).WithMany(p => p.PaymentClaims)
                .HasForeignKey(d => d.ClaimerId)
                .HasConstraintName("FK_PaymentClaim_Claimer");

            entity.HasOne(d => d.Currency).WithMany(p => p.PaymentClaims)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_PaymentClaim_Currency");

            entity.HasOne(d => d.Transaction).WithMany(p => p.PaymentClaims)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_PaymentClaim_Transactions");
        });

        modelBuilder.Entity<PaymentClaimStatus>(entity =>
        {
            entity.ToTable("PaymentClaimStatus", "claim");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.Observation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentClaim).WithMany(p => p.PaymentClaimStatuses)
                .HasForeignKey(d => d.PaymentClaimId)
                .HasConstraintName("FK_PaymentClaimStatus_PaymentClaim");

            entity.HasOne(d => d.StatusCode).WithMany(p => p.PaymentClaimStatuses)
                .HasForeignKey(d => d.StatusCodeId)
                .HasConstraintName("FK_PaymentClaimStatus_StatusCode");
        });

        modelBuilder.Entity<PaymentValidatorComm>(entity =>
        {
            entity.ToTable("PaymentValidatorComm", "payment");

            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.RequestMessage).IsUnicode(false);
            entity.Property(e => e.ResponseDate).HasColumnType("datetime");
            entity.Property(e => e.ResponseMessage).IsUnicode(false);
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");

            entity.HasOne(d => d.TransactionIdPkNavigation).WithMany(p => p.PaymentValidatorComms)
                .HasForeignKey(d => d.TransactionIdPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentValidatorComm_Transactions");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_Products_1");

            entity.ToTable("Products", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("system");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Product1>(entity =>
        {
            entity.ToTable("Products");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Hsr)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("HSR");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Nps).HasColumnName("NPS");
            entity.Property(e => e.Sps)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SPS");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductCarpendi>(entity =>
        {
            entity.ToTable("ProductCarpendi", "common");

            entity.Property(e => e.CarpendiProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Deleted).HasColumnName("deleted");
        });

        modelBuilder.Entity<ProductCentralizer>(entity =>
        {
            entity.ToTable("ProductCentralizer", "common");

            entity.Property(e => e.CentralizerCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDebit).HasDefaultValue(false);
        });

        modelBuilder.Entity<ProductSpsbatch>(entity =>
        {
            entity.ToTable("ProductSPSBatch", "common");

            entity.Property(e => e.ProductSpsbatchId).HasColumnName("ProductSPSBatchId");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductIdNormal).HasColumnName("ProductIdNORMAL");
            entity.Property(e => e.ProductIdPrisma).HasColumnName("ProductIdPRISMA");
        });

        modelBuilder.Entity<ProductsValidator>(entity =>
        {
            entity.HasKey(e => e.ProductValidatorId);

            entity.ToTable("ProductsValidators", "common");

            entity.HasIndex(e => new { e.ProductId, e.ValidatorId }, "IX_ProductsValidators").IsUnique();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsValidators)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsValidators_Products");

            entity.HasOne(d => d.Validator).WithMany(p => p.ProductsValidators)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsValidators_Validators");
        });

        modelBuilder.Entity<Pswd>(entity =>
        {
            entity.ToTable("Pswd", "security");

            entity.Property(e => e.Pswd1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("pswd");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RefundRequests");

            entity.Property(e => e.CallCenterMessage).HasMaxLength(4000);
            entity.Property(e => e.CallCenterValidationDate).HasColumnType("datetime");
            entity.Property(e => e.ClientMessage).HasMaxLength(4000);
            entity.Property(e => e.ClientValidationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.MailSendedToCustomerDate).HasColumnType("datetime");
            entity.Property(e => e.RobotRunDate).HasColumnType("datetime");
            entity.Property(e => e.ValidatorMessage).HasMaxLength(4000);
            entity.Property(e => e.ValidatorValidationDate).HasColumnType("datetime");

            entity.HasOne(d => d.ClaimOperation).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.ClaimOperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Refunds_ClaimOperations");
        });

        modelBuilder.Entity<RefundsRecord>(entity =>
        {
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.RefundsRecords)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefundsRecords_Products");

            entity.HasOne(d => d.Refund).WithMany(p => p.RefundsRecords)
                .HasForeignKey(d => d.RefundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefundsRecords_Refunds");

            entity.HasOne(d => d.Service).WithMany(p => p.RefundsRecords)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefundsRecords_Services");

            entity.HasOne(d => d.Validator).WithMany(p => p.RefundsRecords)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefundsRecords_Validators");
        });

        modelBuilder.Entity<ReportJobLog>(entity =>
        {
            entity.ToTable("ReportJobLog", "payment");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.CommerceItem).WithMany(p => p.ReportJobLogs)
                .HasForeignKey(d => d.CommerceItemId)
                .HasConstraintName("FK_ReportJobLog_CommerceItems");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol", "common");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shortName");
        });

        modelBuilder.Entity<SalePoint>(entity =>
        {
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.SalePoints)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalePoints_Clients");

            entity.HasMany(d => d.Services).WithMany(p => p.SalePoints)
                .UsingEntity<Dictionary<string, object>>(
                    "SalePointService",
                    r => r.HasOne<Service1>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SalePointServices_Services"),
                    l => l.HasOne<SalePoint>().WithMany()
                        .HasForeignKey("SalePointId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SalePointServices_SalePoints"),
                    j =>
                    {
                        j.HasKey("SalePointId", "ServiceId");
                        j.ToTable("SalePointServices");
                    });
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Services", "common");

            entity.HasIndex(e => e.MerchantId, "IX_Services_MerchantId").IsUnique();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Services)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Clients");
        });

        modelBuilder.Entity<Service1>(entity =>
        {
            entity.ToTable("Services");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MerchantEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SenderDisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SenderEmailAddress)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.Service1s)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Clients");
        });

        modelBuilder.Entity<ServiceChannel>(entity =>
        {
            entity.HasOne(d => d.Channel).WithMany(p => p.ServiceChannels)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceChannels_Channels");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceChannels)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceChannels_Services");
        });

        modelBuilder.Entity<ServiceProduct>(entity =>
        {
            entity.HasOne(d => d.Product).WithMany(p => p.ServiceProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceProducts_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceProducts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceProducts_Services");
        });

        modelBuilder.Entity<ServicesConfig>(entity =>
        {
            entity.HasKey(e => e.ServiceConfigId);

            entity.ToTable("ServicesConfig", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExcludeFromSpsbatchCloseInPaywayBo).HasColumnName("ExcludeFromSPSBatchCloseInPaywayBO");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ReportBeginCentralizer)
                .HasDefaultValueSql("('20990121')")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportBeginConciliation)
                .HasDefaultValueSql("('20990121')")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportBeginRendition)
                .HasDefaultValueSql("('20990121')")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportsPath)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.SenderMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SenderURL");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicesConfigs)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicesConfig_Services");
        });

        modelBuilder.Entity<SpsBatch>(entity =>
        {
            entity.ToTable("SpsBatch");

            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.BatchCard).HasMaxLength(4);
            entity.Property(e => e.BatchCommerceNbr).HasMaxLength(12);
            entity.Property(e => e.BatchDate).HasMaxLength(8);
            entity.Property(e => e.BatchNumber)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CardCode)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CloseDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CouponNumber)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EstablishmentNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.OperationAmount)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.OperationDate)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SpsBatchTemporalValidation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SpsBatchTemporalValidation");

            entity.Property(e => e.FechaDeOperacion).HasColumnType("datetime");
            entity.Property(e => e.Idsite).HasColumnName("IDSITE");
        });

        modelBuilder.Entity<SpsServiceRequest>(entity =>
        {
            entity.Property(e => e.Amount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CallbackUrl)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CommerceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MailAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.OperationNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParamSitio)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Payments)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReturnUrl)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SpsServiceResponse>(entity =>
        {
            entity.Property(e => e.AddressValidation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AditionalReason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AuthenticationResultVbv)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Card)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ChargebackDate).HasColumnType("datetime");
            entity.Property(e => e.ChargebackReason)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("chargebackReason");
            entity.Property(e => e.ChargebackSite)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("chargebackSite");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DeliveryAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryCity)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryCountry)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDistrict)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryMessage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryState)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.DocTypeDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Holder)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdMotivo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("idMotivo");
            entity.Property(e => e.IsDateOfBirthValidated)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.IsDocNumValidated)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.IsDocTypeValidated)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.IsGateNumValidated)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.OperationNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Order)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OrderCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PurchaserMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PurchaserTelephone)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Result)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SiteParameter)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TicketNum)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VisibleCardNum)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ZipNumDelivery)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.SvcReq).WithMany(p => p.SpsServiceResponses)
                .HasForeignKey(d => d.SvcReqId)
                .HasConstraintName("FK_SpsServiceResponses_SpsServiceRequests");
        });

        modelBuilder.Entity<StatusCode>(entity =>
        {
            entity.ToTable("StatusCode", "status");

            entity.HasIndex(e => e.Code, "UQ__StatusCo__A25C5AA7887AD120").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.GenericCode).WithMany(p => p.StatusCodes)
                .HasForeignKey(d => d.GenericCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusCode_GenericCode");
        });

        modelBuilder.Entity<StatusMessage>(entity =>
        {
            entity.ToTable("StatusMessage", "status");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Message)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.IdLanguageNavigation).WithMany(p => p.StatusMessages)
                .HasForeignKey(d => d.IdLanguage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusMessage_Language");

            entity.HasOne(d => d.StatusCode).WithMany(p => p.StatusMessages)
                .HasForeignKey(d => d.StatusCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusMessage_StatusCode");
        });

        modelBuilder.Entity<StatusSvcLog>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ElectronicPaymentCode)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusTemplate>(entity =>
        {
            entity.ToTable("StatusTemplate", "notification");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.StatusTemplates)
                .HasForeignKey(d => d.NotificationTemplateId)
                .HasConstraintName("FK_StatusTemplate_NotificationTemplate");

            entity.HasOne(d => d.StatusCode).WithMany(p => p.StatusTemplates)
                .HasForeignKey(d => d.StatusCodeId)
                .HasConstraintName("FK_StatusTemplate_StatusCode");
        });

        modelBuilder.Entity<TicketLog>(entity =>
        {
            entity.ToTable("TicketLog", "notification");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.HtmlTicket)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.TypeFormat)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Channel).HasMaxLength(50);
            entity.Property(e => e.Client).HasMaxLength(50);
            entity.Property(e => e.ConvertionRate).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("")
                .IsFixedLength();
            entity.Property(e => e.ElectronicPaymentCode)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.InternalNbr)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.JsonObject)
                .IsUnicode(false)
                .HasColumnName("JSonObject");
            entity.Property(e => e.MerchantId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Product).HasMaxLength(50);
            entity.Property(e => e.SalePoint)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Service).HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrxCurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("")
                .IsFixedLength();
            entity.Property(e => e.Validator).HasMaxLength(50);
            entity.Property(e => e.WebSvcMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransactionAdditionalInfo>(entity =>
        {
            entity.ToTable("TransactionAdditionalInfo", "payment");

            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BarCode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BatchNbr)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BatchSpsdate)
                .HasColumnType("datetime")
                .HasColumnName("BatchSPSDate");
            entity.Property(e => e.CallbackUrl)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CardHolder)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CardMask)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChannelId).HasDefaultValue(1);
            entity.Property(e => e.ConfirmedByAutoBackOffice).HasColumnType("datetime");
            entity.Property(e => e.ConfirmedByManualSupportBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ConfirmedByManualSupportOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyId).HasDefaultValue(1);
            entity.Property(e => e.CurrentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerMail)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EpcvalidateUrl)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EPCValidateURL");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEpcvalidated).HasColumnName("IsEPCValidated");
            entity.Property(e => e.LanguageId).HasDefaultValue(1);
            entity.Property(e => e.LastStatus).HasDefaultValue(-1);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.OriginalReason)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Payments).HasDefaultValue(1);
            entity.Property(e => e.ProdVersionUsed)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReportDateCentralizer).HasColumnType("datetime");
            entity.Property(e => e.ReportDateConciliation).HasColumnType("datetime");
            entity.Property(e => e.ReportDateRendition).HasColumnType("datetime");
            entity.Property(e => e.SynchronizationDate).HasColumnType("datetime");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
            entity.Property(e => e.UniqueCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.VersionUsed)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.Channel).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Channels");

            entity.HasOne(d => d.Client).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Clients");

            entity.HasOne(d => d.Currency).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Currencies");

            entity.HasOne(d => d.Language).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Language");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Services");

            entity.HasOne(d => d.TransactionIdPkNavigation).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.TransactionIdPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Transactions");

            entity.HasOne(d => d.Validator).WithMany(p => p.TransactionAdditionalInfos)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Validators");
        });

        modelBuilder.Entity<TransactionResultInfo>(entity =>
        {
            entity.ToTable("TransactionResultInfo", "payment");

            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchNbr)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BatchSpsdate)
                .HasColumnType("datetime")
                .HasColumnName("BatchSPSDate");
            entity.Property(e => e.CardHolder)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CardMask)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CardNbrLfd)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerDocType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MailSynchronizationDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StateExtendedMessage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StateMessage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SynchronizationDate).HasColumnType("datetime");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");

            entity.HasOne(d => d.TransactionIdPkNavigation).WithMany(p => p.TransactionResultInfos)
                .HasForeignKey(d => d.TransactionIdPk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionResultInfo_Transactions");
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.ToTable("TransactionStatus", "payment");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsActual).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.StatusCode).WithMany(p => p.TransactionStatuses)
                .HasForeignKey(d => d.StatusCodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionStatus_StatusCode");

            entity.HasOne(d => d.Transactions).WithMany(p => p.TransactionStatuses)
                .HasForeignKey(d => d.TransactionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionStatus_Transactions");
        });

        modelBuilder.Entity<TransactionTicket>(entity =>
        {
            entity.ToTable("TransactionTicket", "notification");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EmailSentDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TicketNumber).HasDefaultValueSql("(NEXT VALUE FOR [common].[TicketNumberSequence])");
            entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "security");

            entity.HasIndex(e => e.Username, "UQ__User__F3DBC572651B2910").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("email");
            entity.Property(e => e.PswdId).HasColumnName("pswdId");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Pswd).WithMany(p => p.Users)
                .HasForeignKey(d => d.PswdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Pswd");
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.ToTable("UserActivity", "security");

            entity.Property(e => e.A)
                .HasMaxLength(8000)
                .HasColumnName("a");
            entity.Property(e => e.R).HasColumnName("r");
            entity.Property(e => e.Ut).HasColumnName("ut");
        });

        modelBuilder.Entity<UserRol>(entity =>
        {
            entity.ToTable("UserRol", "security");

            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Rol).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRol_Rol");

            entity.HasOne(d => d.User).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRol_User");
        });

        modelBuilder.Entity<UserService>(entity =>
        {
            entity.ToTable("UserService", "security");

            entity.HasOne(d => d.Service).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserService_Service");

            entity.HasOne(d => d.User).WithMany(p => p.UserServices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserService_User");
        });

        modelBuilder.Entity<Validator>(entity =>
        {
            entity.HasKey(e => e.ValidatorId).HasName("PK_Validators_1");

            entity.ToTable("Validators", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SendMail).HasDefaultValue(true);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Validator1>(entity =>
        {
            entity.ToTable("Validators");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PluginServiceCallbackUrl)
                .HasMaxLength(250)
                .HasColumnName("PluginServiceCallbackURL");
            entity.Property(e => e.PluginServiceClaimUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PluginServiceClaimURL");
            entity.Property(e => e.PluginServiceMethod).HasMaxLength(250);
            entity.Property(e => e.PluginServiceMethodType).HasMaxLength(250);
            entity.Property(e => e.PluginServiceRequestUrl)
                .HasMaxLength(250)
                .HasColumnName("PluginServiceRequestURL");
            entity.Property(e => e.PluginServiceResponseType).HasMaxLength(250);
            entity.Property(e => e.PluginServiceTransactionInfoUrl)
                .HasMaxLength(250)
                .HasColumnName("PluginServiceTransactionInfoURL");
        });

        modelBuilder.Entity<ValidatorMessage>(entity =>
        {
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.CustomMessage).WithMany(p => p.ValidatorMessages)
                .HasForeignKey(d => d.CustomMessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ValidatorMessages_CustomMessages");

            entity.HasOne(d => d.Validator).WithMany(p => p.ValidatorMessages)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ValidatorMessages_Validators");
        });

        modelBuilder.Entity<ValidatorServiceConfig>(entity =>
        {
            entity.ToTable("ValidatorServiceConfig", "security");

            entity.Property(e => e.HashKey)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ValidatorPass)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ValidatorUser)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Service).WithMany(p => p.ValidatorServiceConfigs)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ValidatorServiceConfig_Services");

            entity.HasOne(d => d.Validator).WithMany(p => p.ValidatorServiceConfigs)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ValidatorServiceConfig_Validators");
        });

        modelBuilder.Entity<Version>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<VersionLog>(entity =>
        {
            entity.ToTable("VersionLog", "common");

            entity.Property(e => e.DeployDate).HasColumnType("datetime");
            entity.Property(e => e.RequestBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Revision)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Support)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwGetPaymentTicketInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGetPaymentTicketInfo");

            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Authcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("authcode");
            entity.Property(e => e.Cardmask)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cardmask");
            entity.Property(e => e.CurrencyIsoCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("currencyIsoCode");
            entity.Property(e => e.CustomerMail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("customerMail");
            entity.Property(e => e.ModuleDescription)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("moduleDescription");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("productName");
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("responseCode");
            entity.Property(e => e.ServiceDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("serviceDescription");
            entity.Property(e => e.ServiceId).HasColumnName("serviceId");
            entity.Property(e => e.TaicreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("TAICreatedOn");
            entity.Property(e => e.TransactionId).HasColumnName("transactionId");
            entity.Property(e => e.Triid).HasColumnName("TRIId");
        });

        modelBuilder.Entity<VwNpsServiceTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwNpsServiceTransactions");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CardMask)
                .HasMaxLength(19)
                .IsUnicode(false);
            entity.Property(e => e.Client).HasMaxLength(50);
            entity.Property(e => e.Country)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.MerchTrxRef)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Operation)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Product).HasMaxLength(50);
            entity.Property(e => e.PurchaseDescription).HasMaxLength(255);
            entity.Property(e => e.Service).HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrxSource)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Validator).HasMaxLength(50);
        });

        modelBuilder.Entity<VwNpsServiceTransactionsAaa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwNpsServiceTransactionsAAA");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CardMask)
                .HasMaxLength(19)
                .IsUnicode(false);
            entity.Property(e => e.Client).HasMaxLength(50);
            entity.Property(e => e.Country)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.MerchTrxRef)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Operation)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Product).HasMaxLength(50);
            entity.Property(e => e.PurchaseDescription).HasMaxLength(255);
            entity.Property(e => e.Service).HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrxSource)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Validator).HasMaxLength(50);
        });

        modelBuilder.Entity<WebSvcMethod>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TrxCost).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Validator).WithMany(p => p.WebSvcMethods)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WebSvcMethods_Validators");
        });

        modelBuilder.Entity<WebSvcMethodParam>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProposedValue).HasMaxLength(500);

            entity.HasOne(d => d.WebSvcMethod).WithMany(p => p.WebSvcMethodParams)
                .HasForeignKey(d => d.WebSvcMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WebSvcMethodParams_WebSvcMethods");
        });
        modelBuilder.HasSequence("PaymentClaimNumberSequence", "common").HasMin(1L);
        modelBuilder.HasSequence("TicketNumberSequence", "common").StartsAt(10618935L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
