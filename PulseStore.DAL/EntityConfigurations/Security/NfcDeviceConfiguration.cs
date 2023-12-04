using PulseStore.BLL.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations.Security;

public class NfcDeviceConfiguration : IEntityTypeConfiguration<NfcDevice>
{
    public void Configure(EntityTypeBuilder<NfcDevice> builder)
    {
        builder.Property(n => n.NUID).HasMaxLength(8);

        builder.HasIndex(n => n.NUID).IsUnique();

        builder.HasData(
            new NfcDevice { Id = 1, NUID = "231C0B0E" },
            new NfcDevice { Id = 2, NUID = "2362AD33" },
            new NfcDevice { Id = 3, NUID = "3309060E" },
            new NfcDevice { Id = 4, NUID = "63D3159A" },
            new NfcDevice { Id = 5, NUID = "B39A689A" },
            new NfcDevice { Id = 6, NUID = "E9675F15" });
    }
}