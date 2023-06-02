using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Enums;

namespace SqlRepository_DbFirst.Models
{
    public partial class TestConfigurationContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrowserConfig>(entity =>
            {
                entity.Property(e => e.BrowserName).HasConversion(new StringToEnumConverter<Browser>());
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role).HasConversion(new StringToEnumConverter<UserRole>());
            });
        }
    }
}
