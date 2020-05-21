using Microsoft.EntityFrameworkCore;
using Resolve.Models;

namespace Resolve.Data
{
    public class ResolveCaseContext : DbContext
    {
        public ResolveCaseContext(DbContextOptions<ResolveCaseContext> options)
            : base(options)
        {
        }

        public DbSet<LocalUser> LocalUser { get; set; }
        public DbSet<LocalGroup> LocalGroup { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<CaseType> CaseType { get; set; }
        public DbSet<CaseTypeGroup> CaseTypeGroup { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Approver> Approver { get; set; }
        public DbSet<GroupAssignment> GroupAssignment { get; set; }
        public DbSet<CaseAudit> CaseAudit { get; set; }
        public DbSet<CaseComment> CaseComment { get; set; }
        public DbSet<CaseAttachment> CaseAttachment { get; set; }
        public DbSet<Resolve.Models.OnBehalf> OnBehalf { get; set; }
        public DbSet<SampleCaseType> SampleCaseType { get; set; }
        public DbSet<Sample2> Sample2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>()
                .HasOne(p => p.LocalUser)
                .WithMany(q => q.Cases)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Case>()
                .Property(p => p.CaseCID)
                .HasComputedColumnSql("'CASE' + CONVERT([nvarchar](23),[CaseID]+10000000)");
            modelBuilder.Entity<Case>()
                .HasOne(p => p.CaseType)
                .WithMany(q => q.Cases)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Case>()
            .Property(b => b.CaseCreationTimestamp)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Case>()
                .Property(r => r.Processed)
                .HasDefaultValue(0);

            modelBuilder.Entity<Approver>()
                .HasKey(c => new { c.CaseID, c.LocalUserID });
            modelBuilder.Entity<Approver>()
                .Property(e => e.Order)
                .HasDefaultValue(1);
            modelBuilder.Entity<Approver>()
                .Property(e => e.Approved)
                .HasDefaultValue(0);

            modelBuilder.Entity<CaseType>()
                .Property(e => e.GroupNumber)
                .HasDefaultValue(1);
            modelBuilder.Entity<CaseType>()
                .HasAlternateKey(e => e.CaseTypeTitle);

            modelBuilder.Entity<CaseTypeGroup>()
                .HasKey(c => new { c.CaseTypeID, c.LocalGroupID });
            modelBuilder.Entity<CaseTypeGroup>()
                .Property(e => e.Order)
                .HasDefaultValue(1);

            modelBuilder.Entity<GroupAssignment>()
                .HasKey(c => new { c.CaseID, c.LocalGroupID });

            modelBuilder.Entity<OnBehalf>()
                .HasKey(c => new { c.CaseID, c.LocalUserID });

            modelBuilder.Entity<UserGroup>()
                .HasKey(c => new { c.LocalUserID, c.LocalGroupID });

            modelBuilder.Entity<LocalGroup>()
                .HasAlternateKey(e => e.GroupName);

            modelBuilder.Entity<CaseComment>()
            .Property(b => b.CommentTimestamp)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<CaseAudit>()
            .Property(b => b.AuditTimestamp)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<CaseAttachment>()
            .Property(b => b.AttachmentTimestamp)
            .HasDefaultValueSql("getdate()");


        }
               

    }
}