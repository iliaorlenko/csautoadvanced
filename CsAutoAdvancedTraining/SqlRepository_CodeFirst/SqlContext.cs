using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Enums;
using SqlRepository_CodeFirst.SqlConfigEntities;

namespace SqlRepository_CodeFirst
{
    public class SqlContext : DbContext
    {
        public DbSet<SqlBrowserConfig> BrowserConfigs => Set<SqlBrowserConfig>();
        public DbSet<SqlUser> Users => Set<SqlUser>();
        public DbSet<SqlTest> Tests => Set<SqlTest>();
        public DbSet<SqlTestStep> TestSteps => Set<SqlTestStep>();

        public SqlContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=TestConfiguration; Trusted_Connection=True; Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlUser>()
                .Property(user => user.Password)
                .HasDefaultValue("Password1*");

            modelBuilder
                .Entity<SqlUser>()
                .Property(user => user.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());

            modelBuilder
                .Entity<SqlBrowserConfig>()
                .Property(cfg => cfg.BrowserName)
                .HasConversion(new EnumToStringConverter<Browser>());
        }
    }
}
