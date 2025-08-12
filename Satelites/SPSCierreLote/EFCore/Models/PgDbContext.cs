using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SPSCierreLote.EFCore.models;

public partial class PgDbContext : DbContext
{
    public PgDbContext()
    {
    }

    public PgDbContext(DbContextOptions<PgDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppConfig> AppConfig { get; set; }

    public virtual DbSet<Banks> Banks { get; set; }

    public virtual DbSet<CarpendiProccess> CarpendiProccess { get; set; }

    public virtual DbSet<CarpendiStats> CarpendiStats { get; set; }

    public virtual DbSet<Channels> Channels { get; set; }

    public virtual DbSet<Channels1> Channels1 { get; set; }

    public virtual DbSet<ClaimOperations> ClaimOperations { get; set; }

    public virtual DbSet<ClaimStatus> ClaimStatus { get; set; }

    public virtual DbSet<ClaimTypes> ClaimTypes { get; set; }

    public virtual DbSet<Claims> Claims { get; set; }

    public virtual DbSet<Clients> Clients { get; set; }

    public virtual DbSet<Clients1> Clients1 { get; set; }

    public virtual DbSet<CommerceItems> CommerceItems { get; set; }

    public virtual DbSet<ConfigurationDefaultValues> ConfigurationDefaultValues { get; set; }

    public virtual DbSet<ConfigurationDetails> ConfigurationDetails { get; set; }

    public virtual DbSet<Configurations> Configurations { get; set; }

    public virtual DbSet<Configurations1> Configurations1 { get; set; }

    public virtual DbSet<Configurations_BK_06_06> Configurations_BK_06_06 { get; set; }

    public virtual DbSet<Configurations_BK_26_06> Configurations_BK_26_06 { get; set; }

    public virtual DbSet<Configurations_BK_27_06> Configurations_BK_27_06 { get; set; }

    public virtual DbSet<Countries> Countries { get; set; }

    public virtual DbSet<Currencies> Currencies { get; set; }

    public virtual DbSet<Currency> Currency { get; set; }

    public virtual DbSet<CustomMessages> CustomMessages { get; set; }

    public virtual DbSet<DocType> DocType { get; set; }

    public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }

    public virtual DbSet<ExternalApp> ExternalApp { get; set; }

    public virtual DbSet<GetPrismaCodes> GetPrismaCodes { get; set; }

    public virtual DbSet<JobRunLog> JobRunLog { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<Languages> Languages { get; set; }

    public virtual DbSet<Logs> Logs { get; set; }

    public virtual DbSet<Module> Module { get; set; }

    public virtual DbSet<ModuleCode> ModuleCode { get; set; }

    public virtual DbSet<MonitorFilesReportProcess> MonitorFilesReportProcess { get; set; }

    public virtual DbSet<MonitorFilesReportRecords> MonitorFilesReportRecords { get; set; }

    public virtual DbSet<MonitorPaywayBackOfficeTransactions> MonitorPaywayBackOfficeTransactions { get; set; }

    public virtual DbSet<MonitorSPSBatchProcess> MonitorSPSBatchProcess { get; set; }

    public virtual DbSet<MonitorSPSBatchProcessFiles> MonitorSPSBatchProcessFiles { get; set; }

    public virtual DbSet<MonitorSPSBatchProcessTransactions> MonitorSPSBatchProcessTransactions { get; set; }

    public virtual DbSet<MonitorSyncroProcess> MonitorSyncroProcess { get; set; }

    public virtual DbSet<MonitorSyncroProcessRecords> MonitorSyncroProcessRecords { get; set; }

    public virtual DbSet<NpsServiceRequests> NpsServiceRequests { get; set; }

    public virtual DbSet<NpsServiceResponseQueryTrxs> NpsServiceResponseQueryTrxs { get; set; }

    public virtual DbSet<NpsServiceResponses> NpsServiceResponses { get; set; }

    public virtual DbSet<PaymentValidatorComm> PaymentValidatorComm { get; set; }

    public virtual DbSet<ProductCarpendi> ProductCarpendi { get; set; }

    public virtual DbSet<ProductCentralizer> ProductCentralizer { get; set; }

    public virtual DbSet<ProductSPSBatch> ProductSPSBatch { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Products1> Products1 { get; set; }

    public virtual DbSet<ProductsValidators> ProductsValidators { get; set; }

    public virtual DbSet<Refunds> Refunds { get; set; }

    public virtual DbSet<RefundsRecords> RefundsRecords { get; set; }

    public virtual DbSet<ReportJobLog> ReportJobLog { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<SalePoints> SalePoints { get; set; }

    public virtual DbSet<ServiceChannels> ServiceChannels { get; set; }

    public virtual DbSet<ServiceProducts> ServiceProducts { get; set; }

    public virtual DbSet<Services> Services { get; set; }

    public virtual DbSet<Services1> Services1 { get; set; }

    public virtual DbSet<ServicesConfig> ServicesConfig { get; set; }

    public virtual DbSet<SpsBatch> SpsBatch { get; set; }

    public virtual DbSet<SpsBatchTemporalValidation> SpsBatchTemporalValidation { get; set; }

    public virtual DbSet<SpsServiceRequests> SpsServiceRequests { get; set; }

    public virtual DbSet<SpsServiceResponses> SpsServiceResponses { get; set; }

    public virtual DbSet<StatusSvcLogs> StatusSvcLogs { get; set; }

    public virtual DbSet<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; }

    public virtual DbSet<TransactionResultInfo> TransactionResultInfo { get; set; }

    public virtual DbSet<TransactionStatus> TransactionStatus { get; set; }

    public virtual DbSet<Transactions> Transactions { get; set; }

    public virtual DbSet<ValidatorMessages> ValidatorMessages { get; set; }

    public virtual DbSet<Validators> Validators { get; set; }

    public virtual DbSet<Validators1> Validators1 { get; set; }

    public virtual DbSet<VersionLog> VersionLog { get; set; }

    public virtual DbSet<Versions> Versions { get; set; }

    public virtual DbSet<WebSvcMethodParams> WebSvcMethodParams { get; set; }

    public virtual DbSet<WebSvcMethods> WebSvcMethods { get; set; }

    public virtual DbSet<vwGetPaymentTicketInfo> vwGetPaymentTicketInfo { get; set; }

    public virtual DbSet<vwNpsServiceTransactions> vwNpsServiceTransactions { get; set; }

    public virtual DbSet<vwNpsServiceTransactionsAAA> vwNpsServiceTransactionsAAA { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=PaymentGatewaySQL");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppConfig>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasFillFactor(80);

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

        modelBuilder.Entity<Banks>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NPS)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<CarpendiProccess>(entity =>
        {
            entity.HasKey(e => e.CarpendiProccessId).HasFillFactor(80);

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

        modelBuilder.Entity<CarpendiStats>(entity =>
        {
            entity.HasKey(e => e.CarpendiStatsId).HasFillFactor(80);

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
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Channels>(entity =>
        {
            entity.HasKey(e => e.ChannelId).HasFillFactor(80);

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

        modelBuilder.Entity<Channels1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<ClaimOperations>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<ClaimTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<Claims>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<Clients>(entity =>
        {
            entity.HasKey(e => e.ClientId)
                .HasName("PK_Clients_1")
                .HasFillFactor(80);

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

        modelBuilder.Entity<Clients1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<CommerceItems>(entity =>
        {
            entity.HasKey(e => e.CommerceItemsId).HasFillFactor(80);

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
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.TransactionIdPKNavigation).WithMany(p => p.CommerceItems)
                .HasForeignKey(d => d.TransactionIdPK)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentCommerceItems_Transactions");
        });

        modelBuilder.Entity<ConfigurationDefaultValues>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CastToCLRType)
                .HasMaxLength(20)
                .IsUnicode(false);
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

        modelBuilder.Entity<ConfigurationDetails>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.BckpValidator).WithMany(p => p.ConfigurationDetailsBckpValidator)
                .HasForeignKey(d => d.BckpValidatorId)
                .HasConstraintName("FK_ConfigDetails_BckpValidators");

            entity.HasOne(d => d.BckpValidatorMethod).WithMany(p => p.ConfigurationDetailsBckpValidatorMethod)
                .HasForeignKey(d => d.BckpValidatorMethodId)
                .HasConstraintName("FK_ConfigDetails_BckpWebSvcMethods");

            entity.HasOne(d => d.Configuration).WithMany(p => p.ConfigurationDetails)
                .HasForeignKey(d => d.ConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigurationDetails_Configurations");

            entity.HasOne(d => d.MainValidator).WithMany(p => p.ConfigurationDetailsMainValidator)
                .HasForeignKey(d => d.MainValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_MainValidators");

            entity.HasOne(d => d.MainValidatorMethod).WithMany(p => p.ConfigurationDetailsMainValidatorMethod)
                .HasForeignKey(d => d.MainValidatorMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_MainWebSvcMethods");

            entity.HasOne(d => d.Product).WithMany(p => p.ConfigurationDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConfigDetails_Products");
        });

        modelBuilder.Entity<Configurations>(entity =>
        {
            entity.HasKey(e => e.ConfigurationId).HasFillFactor(80);

            entity.ToTable("Configurations", "common");

            entity.HasIndex(e => new { e.ServiceId, e.ChannelId, e.ProductId, e.ValidatorId }, "IX_Configurations")
                .IsUnique()
                .HasFillFactor(80);

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

        modelBuilder.Entity<Configurations1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

            entity.HasOne(d => d.ServiceChannel).WithMany(p => p.Configurations1)
                .HasForeignKey(d => d.ServiceChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Config_ServiceChannels");
        });

        modelBuilder.Entity<Configurations_BK_06_06>(entity =>
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

        modelBuilder.Entity<Configurations_BK_26_06>(entity =>
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

        modelBuilder.Entity<Configurations_BK_27_06>(entity =>
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

        modelBuilder.Entity<Countries>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NPS)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Currencies>(entity =>
        {
            entity.HasKey(e => e.ISO).HasFillFactor(80);

            entity.Property(e => e.ISO)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NPS)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasFillFactor(80);

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

        modelBuilder.Entity<CustomMessages>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EN)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.ES).HasMaxLength(4000);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<DocType>(entity =>
        {
            entity.HasKey(e => e.DocTypeId).HasFillFactor(80);

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

        modelBuilder.Entity<DocumentTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NPS)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<ExternalApp>(entity =>
        {
            entity.HasKey(e => e.ExternalAppId).HasFillFactor(80);

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

        modelBuilder.Entity<GetPrismaCodes>(entity =>
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
            entity.HasKey(e => e.JobRunLogId)
                .HasName("PK_AnnulmentJobs")
                .HasFillFactor(80);

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
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.ToTable("Language", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ISO3166)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ISO6391)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ISO6392)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NativeName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Languages>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ISO3166)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ISO639)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.NPS)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Logs>(entity =>
        {
            entity.HasKey(e => e.LogId).HasFillFactor(80);

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

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
        });

        modelBuilder.Entity<ModuleCode>(entity =>
        {
            entity.HasKey(e => e.ModuleCodeId).HasFillFactor(80);

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

            entity.HasOne(d => d.Module).WithMany(p => p.ModuleCode)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModuleCode_Module");
        });

        modelBuilder.Entity<MonitorFilesReportProcess>(entity =>
        {
            entity.HasKey(e => e.MonitorFilesReportProcessId).HasFillFactor(80);

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

        modelBuilder.Entity<MonitorFilesReportRecords>(entity =>
        {
            entity.HasKey(e => e.MonitorFilesReportRecordsId).HasFillFactor(80);

            entity.ToTable("MonitorFilesReportRecords", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsIncomplete).HasDefaultValue(true);
            entity.Property(e => e.IsTotalizer).HasDefaultValue(true);
        });

        modelBuilder.Entity<MonitorPaywayBackOfficeTransactions>(entity =>
        {
            entity.HasKey(e => e.MonitorSPSBatchProcessTransactionsId)
                .HasName("PK_MonitorSPSBatchProcessTransactionsId")
                .HasFillFactor(80);

            entity.ToTable("MonitorPaywayBackOfficeTransactions", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PaywayStatus)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MonitorSPSBatchProcess>(entity =>
        {
            entity.HasKey(e => e.MonitorSPSBatchProcessId).HasFillFactor(80);

            entity.ToTable("MonitorSPSBatchProcess", "common");

            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.IsFTP).HasDefaultValue(false);
        });

        modelBuilder.Entity<MonitorSPSBatchProcessFiles>(entity =>
        {
            entity.HasKey(e => e.MonitorSPSBatchProcessFilesId).HasFillFactor(80);

            entity.ToTable("MonitorSPSBatchProcessFiles", "common");

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
            entity.Property(e => e.IDSITE)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TicketNumberOnSupport)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UnknownRecords).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ValidationError).IsUnicode(false);
        });

        modelBuilder.Entity<MonitorSPSBatchProcessTransactions>(entity =>
        {
            entity.HasKey(e => e.MonitorSPSBatchProcessTransactionsId).HasFillFactor(80);

            entity.ToTable("MonitorSPSBatchProcessTransactions", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.HasInconsistenceError).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceCARD).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceCOST).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceDATE).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceDAYS).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceDUPS).HasDefaultValue(false);
            entity.Property(e => e.InconsistenceError).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.InconsistenceUNIQ).HasDefaultValue(false);
        });

        modelBuilder.Entity<MonitorSyncroProcess>(entity =>
        {
            entity.HasKey(e => e.MonitorSyncroProcessId).HasFillFactor(80);

            entity.ToTable("MonitorSyncroProcess", "common");

            entity.Property(e => e.BeginOn).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<MonitorSyncroProcessRecords>(entity =>
        {
            entity.HasKey(e => e.MonitorSyncroProcessRecordsId).HasFillFactor(80);

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
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<NpsServiceRequests>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<NpsServiceResponseQueryTrxs>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

            entity.HasOne(d => d.SvcRes).WithMany(p => p.NpsServiceResponseQueryTrxs)
                .HasForeignKey(d => d.SvcResId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NpsServiceResponseQueryTrxs_NpsServiceResponses");
        });

        modelBuilder.Entity<NpsServiceResponses>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.Property(e => e.FrontPSPUrl)
                .HasMaxLength(255)
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

        modelBuilder.Entity<PaymentValidatorComm>(entity =>
        {
            entity.HasKey(e => e.PaymentValidatorCommId).HasFillFactor(80);

            entity.ToTable("PaymentValidatorComm", "payment");

            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.RequestMessage).IsUnicode(false);
            entity.Property(e => e.ResponseDate).HasColumnType("datetime");
            entity.Property(e => e.ResponseMessage).IsUnicode(false);

            entity.HasOne(d => d.TransactionIdPKNavigation).WithMany(p => p.PaymentValidatorComm)
                .HasForeignKey(d => d.TransactionIdPK)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentValidatorComm_Transactions");
        });

        modelBuilder.Entity<ProductCarpendi>(entity =>
        {
            entity.HasKey(e => e.ProductCarpendiId).HasFillFactor(80);

            entity.ToTable("ProductCarpendi", "common");

            entity.Property(e => e.CarpendiProductId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductCentralizer>(entity =>
        {
            entity.HasKey(e => e.ProductCentralizerId).HasFillFactor(80);

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

        modelBuilder.Entity<ProductSPSBatch>(entity =>
        {
            entity.HasKey(e => e.ProductSPSBatchId).HasFillFactor(80);

            entity.ToTable("ProductSPSBatch", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.ProductId)
                .HasName("PK_Products_1")
                .HasFillFactor(80);

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

        modelBuilder.Entity<Products1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.ToTable("Products");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.HSR)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SPS)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductsValidators>(entity =>
        {
            entity.HasKey(e => e.ProductValidatorId).HasFillFactor(80);

            entity.ToTable("ProductsValidators", "common");

            entity.HasIndex(e => new { e.ProductId, e.ValidatorId }, "IX_ProductsValidators")
                .IsUnique()
                .HasFillFactor(80);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsValidators)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsValidators_Products");

            entity.HasOne(d => d.Validator).WithMany(p => p.ProductsValidators)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductsValidators_Validators");
        });

        modelBuilder.Entity<Refunds>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_RefundRequests")
                .HasFillFactor(80);

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

        modelBuilder.Entity<RefundsRecords>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.HasKey(e => e.ReportJobLogId).HasFillFactor(80);

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

            entity.HasOne(d => d.CommerceItem).WithMany(p => p.ReportJobLog)
                .HasForeignKey(d => d.CommerceItemId)
                .HasConstraintName("FK_ReportJobLog_CommerceItems");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.ToTable("Rol", "common");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.shortName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SalePoints>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

            entity.HasMany(d => d.Service).WithMany(p => p.SalePoint)
                .UsingEntity<Dictionary<string, object>>(
                    "SalePointServices",
                    r => r.HasOne<Services1>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SalePointServices_Services"),
                    l => l.HasOne<SalePoints>().WithMany()
                        .HasForeignKey("SalePointId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_SalePointServices_SalePoints"),
                    j =>
                    {
                        j.HasKey("SalePointId", "ServiceId").HasFillFactor(80);
                    });
        });

        modelBuilder.Entity<ServiceChannels>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.HasOne(d => d.Channel).WithMany(p => p.ServiceChannels)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceChannels_Channels");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceChannels)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceChannels_Services");
        });

        modelBuilder.Entity<ServiceProducts>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

            entity.HasOne(d => d.Product).WithMany(p => p.ServiceProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceProducts_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceProducts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceProducts_Services");
        });

        modelBuilder.Entity<Services>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasFillFactor(80);

            entity.ToTable("Services", "common");

            entity.HasIndex(e => e.MerchantId, "IX_Services_MerchantId")
                .IsUnique()
                .HasFillFactor(80);

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

        modelBuilder.Entity<Services1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

            entity.HasOne(d => d.Client).WithMany(p => p.Services1)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Clients");
        });

        modelBuilder.Entity<ServicesConfig>(entity =>
        {
            entity.HasKey(e => e.ServiceConfigId).HasFillFactor(80);

            entity.ToTable("ServicesConfig", "common");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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
            entity.Property(e => e.SenderURL)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicesConfig)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicesConfig_Services");
        });

        modelBuilder.Entity<SpsBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.HasNoKey();

            entity.Property(e => e.FechaDeOperacion).HasColumnType("datetime");
        });

        modelBuilder.Entity<SpsServiceRequests>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<SpsServiceResponses>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.Property(e => e.chargebackReason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.chargebackSite)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.idMotivo)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.SvcReq).WithMany(p => p.SpsServiceResponses)
                .HasForeignKey(d => d.SvcReqId)
                .HasConstraintName("FK_SpsServiceResponses_SpsServiceRequests");
        });

        modelBuilder.Entity<StatusSvcLogs>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<TransactionAdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.TransactionAdditionalInfoId).HasFillFactor(80);

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
            entity.Property(e => e.BatchSPSDate).HasColumnType("datetime");
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
            entity.Property(e => e.EPCValidateURL)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
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

            entity.HasOne(d => d.Channel).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Channels");

            entity.HasOne(d => d.Client).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Clients");

            entity.HasOne(d => d.Currency).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Currencies");

            entity.HasOne(d => d.Language).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Language");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Products");

            entity.HasOne(d => d.Service).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Services");

            entity.HasOne(d => d.TransactionIdPKNavigation).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.TransactionIdPK)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Transactions");

            entity.HasOne(d => d.Validator).WithMany(p => p.TransactionAdditionalInfo)
                .HasForeignKey(d => d.ValidatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionAdditionalInfo_Validators");
        });

        modelBuilder.Entity<TransactionResultInfo>(entity =>
        {
            entity.HasKey(e => e.TransactionResultInfoId).HasFillFactor(80);

            entity.ToTable("TransactionResultInfo", "payment");

            entity.Property(e => e.AuthorizationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchNbr)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BatchSPSDate).HasColumnType("datetime");
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

            entity.HasOne(d => d.TransactionIdPKNavigation).WithMany(p => p.TransactionResultInfo)
                .HasForeignKey(d => d.TransactionIdPK)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionResultInfo_Transactions");
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.TransactionStatusId).HasFillFactor(80);

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

            entity.HasOne(d => d.Transactions).WithMany(p => p.TransactionStatus)
                .HasForeignKey(d => d.TransactionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionStatus_Transactions");
        });

        modelBuilder.Entity<Transactions>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.Property(e => e.JSonObject).IsUnicode(false);
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

        modelBuilder.Entity<ValidatorMessages>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<Validators>(entity =>
        {
            entity.HasKey(e => e.ValidatorId)
                .HasName("PK_Validators_1")
                .HasFillFactor(80);

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

        modelBuilder.Entity<Validators1>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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
            entity.Property(e => e.PluginServiceCallbackURL).HasMaxLength(250);
            entity.Property(e => e.PluginServiceClaimURL)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PluginServiceMethod).HasMaxLength(250);
            entity.Property(e => e.PluginServiceMethodType).HasMaxLength(250);
            entity.Property(e => e.PluginServiceRequestURL).HasMaxLength(250);
            entity.Property(e => e.PluginServiceResponseType).HasMaxLength(250);
            entity.Property(e => e.PluginServiceTransactionInfoURL).HasMaxLength(250);
        });

        modelBuilder.Entity<VersionLog>(entity =>
        {
            entity.HasKey(e => e.VersionLogId).HasFillFactor(80);

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

        modelBuilder.Entity<Versions>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<WebSvcMethodParams>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<WebSvcMethods>(entity =>
        {
            entity.HasKey(e => e.Id).HasFillFactor(80);

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

        modelBuilder.Entity<vwGetPaymentTicketInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwGetPaymentTicketInfo");

            entity.Property(e => e.TAICreatedOn).HasColumnType("datetime");
            entity.Property(e => e.amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.authcode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.cardmask)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.currencyIsoCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.customerMail)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.moduleDescription)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.productName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.responseCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.serviceDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<vwNpsServiceTransactions>(entity =>
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
            entity.Property(e => e.ISO)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
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

        modelBuilder.Entity<vwNpsServiceTransactionsAAA>(entity =>
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
            entity.Property(e => e.ISO)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
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
        modelBuilder.HasSequence("TicketNumberSequence", "common").StartsAt(10618935L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
