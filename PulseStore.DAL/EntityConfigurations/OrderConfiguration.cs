using PulseStore.BLL.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.FirstName).HasMaxLength(256);
        builder.Property(o => o.LastName).HasMaxLength(256);
        builder.Property(o => o.Email).HasMaxLength(256);
        builder.Property(o => o.PhoneNumber).HasMaxLength(32);
    }
}