using PulseStore.BLL.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(op => new { op.OrderId, op.ProductId });

        builder.Property(op => op.PricePerItem).HasPrecision(10, 2);

        builder.HasOne(op => op.Order)
           .WithMany(o => o.OrderProducts)
           .HasForeignKey(op => op.OrderId);

        builder.HasOne(op => op.Product)
            .WithMany()
            .HasForeignKey(op => op.ProductId);
    }
}