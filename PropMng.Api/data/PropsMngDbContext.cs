using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PropMng.Api.data
{
    public partial class PropsMngDbContext : DbContext
    {
        public PropsMngDbContext()
        {
        }

        public PropsMngDbContext(DbContextOptions<PropsMngDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomesLog> IncomesLogs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoicesLog> InvoicesLogs { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LogsEnterance> LogsEnterances { get; set; }
        public virtual DbSet<LogsError> LogsErrors { get; set; }
        public virtual DbSet<LogsOperation> LogsOperations { get; set; }
        public virtual DbSet<LogsToken> LogsTokens { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonsCorp> PersonsCorps { get; set; }
        public virtual DbSet<PersonsLog> PersonsLogs { get; set; }
        public virtual DbSet<Prop> Props { get; set; }
        public virtual DbSet<PropsLog> PropsLogs { get; set; }
        public virtual DbSet<PropsUnit> PropsUnits { get; set; }
        public virtual DbSet<PropsUnitsLog> PropsUnitsLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=.; initial catalog=PropsMngDb; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_AspNetUsers_Employees");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Income>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiptNum).HasMaxLength(50);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Incomes)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Incomes_Invoices1");
            });

            modelBuilder.Entity<IncomesLog>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.LogId });

                entity.HasOne(d => d.Log)
                    .WithMany(p => p.IncomesLogs)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomesLogs_Logs");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.IncomesLogs)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IncomesLogs_Incomes");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthlyRent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(29, 2)")
                    .HasComputedColumnSql("([MonthlyRent]*[Months])", false);

                entity.Property(e => e.TotalAmountWithTax)
                    .HasColumnType("numeric(33, 4)")
                    .HasComputedColumnSql("([MonthlyRent]*[Months]+([MonthlyRent]*[Months])*(0.15))", false);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoices_Persons");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoices_PropsUnits");
            });

            modelBuilder.Entity<InvoicesLog>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.LogId });

                entity.HasOne(d => d.Log)
                    .WithMany(p => p.InvoicesLogs)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoicesLogs_Logs");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.InvoicesLogs)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoicesLogs_Invoices");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ScreenCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Enterance)
                    .WithMany(p => p.InverseEnterance)
                    .HasForeignKey(d => d.EnteranceId)
                    .HasConstraintName("FK_Logs_Logs");
            });

            modelBuilder.Entity<LogsEnterance>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IpAddress).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.LogsEnterance)
                    .HasForeignKey<LogsEnterance>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LogsEnterances_Logs");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogsEnterances)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LogsEnterances_AspNetUsers");
            });

            modelBuilder.Entity<LogsError>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IpAddress).HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.LogsError)
                    .HasForeignKey<LogsError>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LogsErrors_Logs");
            });

            modelBuilder.Entity<LogsOperation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.LogsOperation)
                    .HasForeignKey<LogsOperation>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FkSessionLogMainOperationsSessionLog");
            });

            modelBuilder.Entity<LogsToken>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Token })
                    .HasName("PK_SessionLogEnterancesTokens");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.LogsTokens)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionLogEnterancesTokens_SessionLogEnterances");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.IdNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNum)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PersonsCorp>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CrName).IsRequired();

                entity.Property(e => e.CrNum).IsRequired();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PersonsCorp)
                    .HasForeignKey<PersonsCorp>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonsCorps_Persons");
            });

            modelBuilder.Entity<PersonsLog>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.LogId })
                    .HasName("PK_EmployeesLogs");

                entity.HasOne(d => d.Log)
                    .WithMany(p => p.PersonsLogs)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeesLogs_Logs");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.PersonsLogs)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeesLogs_Employees");
            });

            modelBuilder.Entity<Prop>(entity =>
            {
                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.CmName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CrNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.TaxRegNum)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PropsLog>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.LogId });

                entity.HasOne(d => d.Log)
                    .WithMany(p => p.PropsLogs)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropsLogs_Logs");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.PropsLogs)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropsLogs_Props");
            });

            modelBuilder.Entity<PropsUnit>(entity =>
            {
                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MonthlyRent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Prop)
                    .WithMany(p => p.PropsUnits)
                    .HasForeignKey(d => d.PropId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropsUnits_Props");
            });

            modelBuilder.Entity<PropsUnitsLog>(entity =>
            {
                entity.HasKey(e => new { e.RecId, e.LogId });

                entity.HasOne(d => d.Log)
                    .WithMany(p => p.PropsUnitsLogs)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropsUnitsLogs_Logs");

                entity.HasOne(d => d.Rec)
                    .WithMany(p => p.PropsUnitsLogs)
                    .HasForeignKey(d => d.RecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropsUnitsLogs_PropsUnits");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
