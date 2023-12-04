using PulseStore.BLL.Entities.SensorReadings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations;

public class SensorReadingsConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.HasIndex(u => u.DeviceMacAddress)
        .IsUnique();

        builder.HasOne(sr => sr.Stock)
            .WithMany(st => st.Sensors)
            .HasForeignKey(sr => sr.StockId);

        builder.HasMany(sr => sr.SensorReadings)
            .WithOne(rd => rd.Sensor)
            .HasForeignKey(rd => rd.SensorId);
    }
}
