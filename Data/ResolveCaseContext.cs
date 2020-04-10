﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<CaseType> CaseType { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Approver> Approver { get; set; }
        public DbSet<GroupAssignment> GroupAssignment { get; set; }
        public DbSet<CaseAudit> CaseAudit { get; set; }
        public DbSet<CaseComment> CaseComment { get; set; }
        public DbSet<SampleCaseType> SampleCaseType { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>()
                .HasOne(p => p.LocalUser)
                .WithMany(q => q.Cases)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Case>()
                .HasOne(p => p.CaseType)
                .WithMany(q => q.Cases)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Case>()
            .Property(b => b.CaseCreationTimestamp)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Approver>()
                .HasKey(c => new { c.CaseID, c.LocalUserID });

            modelBuilder.Entity<GroupAssignment>()
                .HasKey(c => new { c.CaseID, c.LocalGroupID });

            modelBuilder.Entity<CaseComment>()
            .Property(b => b.CommentTimestamp)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<CaseAudit>()
            .Property(b => b.AuditTimestamp)
            .HasDefaultValueSql("getdate()");


        }

    }
}