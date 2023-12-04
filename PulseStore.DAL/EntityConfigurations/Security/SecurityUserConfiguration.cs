using PulseStore.BLL.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations.Security;

public class SecurityUserConfiguration : IEntityTypeConfiguration<SecurityUser>
{
    public void Configure(EntityTypeBuilder<SecurityUser> builder)
    {
        builder.Property(su => su.FirstName).HasMaxLength(256);
        builder.Property(su => su.LastName).HasMaxLength(256);

        builder.HasOne(su => su.NfcDevice)
            .WithMany(n => n.SecurityUsers)
            .HasForeignKey(su => su.NfcDeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(su => su.Stocks)
           .WithMany(s => s.SecurityUsers)
           .UsingEntity(t => t.ToTable("SecurityUserStocks"));
    }
}