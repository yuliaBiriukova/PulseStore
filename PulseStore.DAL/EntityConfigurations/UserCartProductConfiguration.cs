using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PulseStore.BLL.Entities;

namespace PulseStore.DAL.EntityConfigurations;

internal class CartProductConfiguration : IEntityTypeConfiguration<UserCartProduct>
{
    public void Configure(EntityTypeBuilder<UserCartProduct> builder)
    {
        builder.Property(c => c.UserId).IsRequired().HasMaxLength(450);

        builder.Property(c => c.Quantity).IsRequired();

        builder.HasIndex(c => new { c.ProductId, c.UserId }).IsUnique();

        builder.HasOne(c => c.Product)
           .WithMany(p => p.UserCartProducts)
           .HasForeignKey(c => c.ProductId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}