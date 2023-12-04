using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PulseStore.BLL.Entities;

namespace PulseStore.DAL.EntityConfigurations;

public class StockProductConfiguration : IEntityTypeConfiguration<StockProduct>
{
    public void Configure(EntityTypeBuilder<StockProduct> builder)
    {
        builder.Property(sp => sp.Quantity).IsRequired();

        builder.HasIndex(sp => new { sp.ProductId, sp.StockId }).IsUnique();

        builder.HasOne(sp => sp.Product)
           .WithMany(p => p.StockProducts)
           .HasForeignKey(sp => sp.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Stock)
           .WithMany(s => s.StockProducts)
           .HasForeignKey(sp => sp.StockId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}