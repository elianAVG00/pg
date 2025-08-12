using Microsoft.EntityFrameworkCore;

namespace PGExporter.EF
{
    public class PaymentContext : DbContext
    {
        public PaymentContext() { }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppConfig> AppConfig { get; set; }
        public virtual DbSet<Channels> Channels { get; set; }
        public virtual DbSet<CommerceItems> CommerceItems { get; set; }
        public virtual DbSet<Configurations> Configurations { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<MonitorFilesReportProcess> MonitorFilesReportProcess { get; set; }
        public virtual DbSet<MonitorFilesReportRecords> MonitorFilesReportRecords { get; set; }
        public virtual DbSet<ProductCentralizer> ProductCentralizer { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<ServicesConfig> ServicesConfig { get; set; }
        public virtual DbSet<TransactionAdditionalInfo> TransactionAdditionalInfo { get; set; }
        public virtual DbSet<TransactionResultInfo> TransactionResultInfo { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>(entity =>
            {
                entity.HasKey(e => e.ConfigId);
                entity.ToTable("AppConfig", "common");
                entity.Property(e => e.ConfigType).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.DeleteTime).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(byte.MaxValue).IsUnicode(false);
                entity.Property(e => e.Setting).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
                entity.Property(e => e.Value).IsUnicode(false);
                entity.Property(e => e.ViewSpecialBackgroundColor)
                      .HasMaxLength(50)
                      .IsUnicode(false)
                      .HasDefaultValue("white");
            });

            modelBuilder.Entity<Channels>(entity =>
            {
                entity.HasKey(e => e.ChannelId);
                entity.ToTable("Channels", "common");
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedOn)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500).IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<CommerceItems>(entity =>
            {
                entity.ToTable("CommerceItems", "payment");
                entity.HasIndex(e => e.TransactionIdPk, "IDX_CommerceItems_TraId").HasFillFactor(80);
                entity.HasIndex(e => e.ReportDateCentralizer, "NIDX_CentralizerReport1").HasFillFactor(80);
                entity.HasIndex(e => e.ReportDateRendition, "NIDX_RenditionReport1").HasFillFactor(80);
                entity.HasIndex(e => e.ReportDateConciliation, "NIDX_ReporteConciliation1").HasFillFactor(80);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Code).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500).IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.OriginalCode).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.ReportDateCentralizer).HasColumnType("datetime");
                entity.Property(e => e.ReportDateConciliation).HasColumnType("datetime");
                entity.Property(e => e.ReportDateRendition).HasColumnType("datetime");
                entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
                entity.HasOne(d => d.TransactionIdPkNavigation)
                      .WithMany(p => p.CommerceItems)
                      .HasForeignKey(d => d.TransactionIdPk)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_PaymentCommerceItems_Transactions");
            });

            modelBuilder.Entity<Configurations>(entity =>
            {
                entity.HasKey(e => e.ConfigurationId);
                entity.ToTable("Configurations", "common");
                entity.HasIndex(e => new { e.ServiceId, e.ChannelId, e.ProductId, e.ValidatorId }, "IX_Configurations").IsUnique();
                entity.Property(e => e.CommerceNumber).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.UniqueCode).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UpdatedBy).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
                entity.HasOne(d => d.Channel)
                      .WithMany(p => p.Configurations)
                      .HasForeignKey(d => d.ChannelId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Configurations_Channels");
                entity.HasOne(d => d.Product)
                      .WithMany(p => p.Configurations)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Configurations_Products");
                entity.HasOne(e => e.Service)
                      .WithMany(s => s.Configurations)
                      .HasForeignKey(e => e.ServiceId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Configurations_Services");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.ToTable("Logs", "security");
                entity.HasIndex(e => e.Date, "NonClusteredIndex-20240116-120847").HasFillFactor(80);
                entity.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("createdBy");
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("createdOn");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.Deleted).HasColumnName("deleted");
                entity.Property(e => e.Exception).IsUnicode(false);
                entity.Property(e => e.InnerException).IsUnicode(false);
                entity.Property(e => e.Message).IsUnicode(false);
                entity.Property(e => e.Thread).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.Transaction).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.Type).HasMaxLength(250).IsUnicode(false);
            });

            modelBuilder.Entity<MonitorFilesReportProcess>(entity =>
            {
                entity.ToTable("MonitorFilesReportProcess", "common");
                entity.Property(e => e.BeginOn).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.EndDataOn).HasColumnType("datetime");
                entity.Property(e => e.EndProcessOn).HasColumnType("datetime");
                entity.Property(e => e.Error).HasMaxLength(8000).IsUnicode(false);
                entity.Property(e => e.RemoteFile).HasMaxLength(250).IsUnicode(false);
            });

            modelBuilder.Entity<MonitorFilesReportRecords>(entity =>
            {
                entity.ToTable("MonitorFilesReportRecords", "common");
                entity.Property(e => e.CreatedBy).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.IsIncomplete).HasDefaultValue(true);
                entity.Property(e => e.IsTotalizer).HasDefaultValue(true);
                entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
            });

            modelBuilder.Entity<ProductCentralizer>(entity =>
            {
                entity.ToTable("ProductCentralizer", "common");
                entity.Property(e => e.CentralizerCode).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDebit).HasDefaultValue(false);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK_Products_1");
                entity.ToTable("Products", "common");
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false).HasDefaultValue("system");
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Type).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId);
                entity.ToTable("Services", "common");
                entity.HasIndex(e => e.MerchantId, "IX_Services_MerchantId").IsUnique();
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(500).IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.MerchantId).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ServicesConfig>(entity =>
            {
                entity.HasKey(e => e.ServiceConfigId);
                entity.ToTable("ServicesConfig", "common");
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.ExcludeFromSpsbatchCloseInPaywayBo).HasColumnName("ExcludeFromSPSBatchCloseInPaywayBO");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.ReportBeginCentralizer).HasDefaultValueSql("('20990121')").HasColumnType("datetime");
                entity.Property(e => e.ReportBeginConciliation).HasDefaultValueSql("('20990121')").HasColumnType("datetime");
                entity.Property(e => e.ReportBeginRendition).HasDefaultValueSql("('20990121')").HasColumnType("datetime");
                entity.Property(e => e.ReportsPath).HasMaxLength(8000).IsUnicode(false);
                entity.Property(e => e.SenderMail).HasMaxLength(byte.MaxValue).IsUnicode(false);
                entity.Property(e => e.SenderUrl).HasMaxLength(byte.MaxValue).HasColumnName("SenderURL");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
                entity.HasOne(d => d.Service)
                      .WithMany(p => p.ServicesConfig)
                      .HasForeignKey(d => d.ServiceId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ServicesConfig_Services");
            });

            modelBuilder.Entity<TransactionAdditionalInfo>(entity =>
            {
                entity.ToTable("TransactionAdditionalInfo", "payment");
                entity.HasIndex(e => e.TransactionIdPk, "IDX_TransactionAdditionalInfo_TraId").HasFillFactor(80);
                entity.HasIndex(e => new { e.TransactionNumber, e.TransactionIdPk }, "IDX_TransactionAdditionalInfo_TraNumber_TraId").HasFillFactor(80);
                entity.HasIndex(e => e.ValidatorId, "NIDX_ValidartorId").HasFillFactor(80);
                entity.Property(e => e.AuthorizationCode).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.BarCode).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.BatchNbr).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.BatchSpsdate).HasColumnType("datetime").HasColumnName("BatchSPSDate");
                entity.Property(e => e.CallbackUrl).HasMaxLength(1000).IsUnicode(false).HasDefaultValue("");
                entity.Property(e => e.CardHolder).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.CardMask).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.ChannelId).HasDefaultValue(1);
                entity.Property(e => e.ConfirmedByAutoBackOffice).HasColumnType("datetime");
                entity.Property(e => e.ConfirmedByManualSupportBy).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.ConfirmedByManualSupportOn).HasColumnType("datetime");
                entity.Property(e => e.CreatedBy).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.CurrencyId).HasDefaultValue(1);
                entity.Property(e => e.CurrentAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CustomerMail).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.EpcvalidateUrl).HasMaxLength(1000).HasColumnName("EPCValidateURL");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsEpcvalidated).HasColumnName("IsEPCValidated");
                entity.Property(e => e.LanguageId).HasDefaultValue(1);
                entity.Property(e => e.LastStatus).HasDefaultValue(-1);
                entity.Property(e => e.MerchantId).HasMaxLength(50).IsUnicode(false).HasDefaultValue("");
                entity.Property(e => e.OriginalReason).HasMaxLength(8000).IsUnicode(false);
                entity.Property(e => e.Payments).HasDefaultValue(1);
                entity.Property(e => e.ProdVersionUsed).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.ReportDateCentralizer).HasColumnType("datetime");
                entity.Property(e => e.ReportDateConciliation).HasColumnType("datetime");
                entity.Property(e => e.ReportDateRendition).HasColumnType("datetime");
                entity.Property(e => e.SynchronizationDate).HasColumnType("datetime");
                entity.Property(e => e.TicketNumber).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
                entity.Property(e => e.TransactionNumber).HasDefaultValueSql("(NEXT VALUE FOR [common].[TransactionNumberSequence])");
                entity.Property(e => e.UniqueCode).HasMaxLength(50).IsUnicode(false).HasDefaultValue("");
                entity.Property(e => e.UpdatedBy).HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
                entity.Property(e => e.VersionUsed).HasMaxLength(250).IsUnicode(false);
                entity.HasOne(d => d.Channel).WithMany(p => p.TransactionAdditionalInfo).HasForeignKey(d => d.ChannelId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransactionAdditionalInfo_Channels");
                entity.HasOne(d => d.Product).WithMany(p => p.TransactionAdditionalInfo).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransactionAdditionalInfo_Products");
                entity.HasOne(d => d.Service).WithMany(p => p.TransactionAdditionalInfo).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransactionAdditionalInfo_Services");
                entity.HasOne(d => d.TransactionIdPkNavigation).WithMany(p => p.TransactionAdditionalInfo).HasForeignKey(d => d.TransactionIdPk).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransactionAdditionalInfo_Transactions");
            });

            modelBuilder.Entity<TransactionResultInfo>(entity =>
            {
                entity.ToTable("TransactionResultInfo", "payment");
                entity.HasIndex(e => e.TransactionIdPk, "IDX_TransactionResultInfo_TraId").HasFillFactor(80);
                entity.Property(e => e.AuthorizationCode).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.BatchNbr).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.BatchSpsdate).HasColumnType("datetime").HasColumnName("BatchSPSDate");
                entity.Property(e => e.CardHolder).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.CardMask).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CardNbrLfd).HasMaxLength(4).IsUnicode(false);
                entity.Property(e => e.Country).HasMaxLength(10).IsUnicode(false);
                entity.Property(e => e.CreatedBy).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.Currency).HasMaxLength(10).IsUnicode(false);
                entity.Property(e => e.CustomerDocNumber).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CustomerDocType).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CustomerEmail).HasMaxLength(byte.MaxValue).IsUnicode(false);
                entity.Property(e => e.MailSynchronizationDate).HasDefaultValueSql("NULL").HasColumnType("datetime");
                entity.Property(e => e.ResponseCode).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.StateExtendedMessage).HasMaxLength(byte.MaxValue).IsUnicode(false);
                entity.Property(e => e.StateMessage).HasMaxLength(byte.MaxValue).IsUnicode(false);
                entity.Property(e => e.SynchronizationDate).HasColumnType("datetime");
                entity.Property(e => e.TicketNumber).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.TransactionIdPk).HasColumnName("TransactionIdPK");
                entity.HasOne(d => d.TransactionIdPkNavigation).WithMany().HasForeignKey(d => d.TransactionIdPk).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_TransactionResultInfo_Transactions");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.ToTable("Transactions", "dbo");
                entity.HasIndex(e => e.ElectronicPaymentCode, "EPCIndex").HasFillFactor(80);
                entity.HasIndex(e => e.CreatedOn, "IDX_Transactions_CreatedOn").HasFillFactor(80);
                entity.HasIndex(e => e.TransactionId, "NonClusteredIndex-20150108-091207").HasFillFactor(80);
                entity.HasIndex(e => new { e.Client, e.Id, e.TransactionId, e.MerchantId }, "NonClusteredIndex-20150528-084058").HasFillFactor(80);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Channel).HasMaxLength(50);
                entity.Property(e => e.Client).HasMaxLength(50);
                entity.Property(e => e.ConvertionRate).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.CurrencyCode).HasMaxLength(3).IsUnicode(false).HasDefaultValue("").IsFixedLength();
                entity.Property(e => e.ElectronicPaymentCode).HasMaxLength(60).IsUnicode(false).HasDefaultValue("");
                entity.Property(e => e.InternalNbr).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.JsonObject).IsUnicode(false).HasColumnName("JSonObject");
                entity.Property(e => e.MerchantId).HasMaxLength(30).IsUnicode(false).HasDefaultValue("");
                entity.Property(e => e.Product).HasMaxLength(50);
                entity.Property(e => e.SalePoint).HasMaxLength(10).IsUnicode(false);
                entity.Property(e => e.Service).HasMaxLength(50);
                entity.Property(e => e.TransactionId).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.TrxAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TrxCurrencyCode).HasMaxLength(3).IsUnicode(false).HasDefaultValue("").IsFixedLength();
                entity.Property(e => e.Validator).HasMaxLength(50);
                entity.Property(e => e.WebSvcMethod).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.HasSequence("PaymentClaimNumberSequence", "common").StartsAt(40000);
            modelBuilder.HasSequence("TicketNumberSequence", "common").StartsAt(40000);
            modelBuilder.HasSequence("TransactionNumberSequence", "common").StartsAt(900000000);
        }
    }
}
