using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PulseStore.BLL.Entities;

namespace PulseStore.DAL.EntityConfigurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.Property(s => s.Name).IsRequired().HasMaxLength(256);

        builder.HasData(new Stock { Id = 1, Name = "Stock 1" });
    }
}