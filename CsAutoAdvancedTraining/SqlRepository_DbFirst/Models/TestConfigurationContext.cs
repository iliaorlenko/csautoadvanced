using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SqlRepository_DbFirst.Models;

public partial class TestConfigurationContext : DbContext
{
    public TestConfigurationContext()
    {
    }

    public TestConfigurationContext(DbContextOptions<TestConfigurationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BrowserConfig> BrowserConfigs { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestStep> TestSteps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=TestConfiguration; Trusted_Connection=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BrowserConfig>(entity =>
        {
            entity.HasMany(d => d.Users).WithMany(p => p.BrowserConfigs)
                .UsingEntity<Dictionary<string, object>>(
                    "SqlBrowserConfigSqlUser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UsersId"),
                    l => l.HasOne<BrowserConfig>().WithMany().HasForeignKey("BrowserConfigsId"),
                    j =>
                    {
                        j.HasKey("BrowserConfigsId", "UsersId");
                        j.ToTable("SqlBrowserConfigSqlUser");
                        j.HasIndex(new[] { "UsersId" }, "IX_SqlBrowserConfigSqlUser_UsersId");
                    });
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Tests_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Tests).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TestStep>(entity =>
        {
            entity.HasIndex(e => e.TestId, "IX_TestSteps_TestId");

            entity.HasOne(d => d.Test).WithMany(p => p.TestSteps).HasForeignKey(d => d.TestId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Password).HasDefaultValueSql("(N'Password1*')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
