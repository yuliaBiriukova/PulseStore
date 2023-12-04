using PulseStore.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations
{
    public class UserProductViewConfiguration : IEntityTypeConfiguration<UserProductView>
    {
        public void Configure(EntityTypeBuilder<UserProductView> builder)
        {
            builder.HasKey(upv => new { upv.UserId, upv.ProductId });

            builder.Property(upv => upv.UserId)
                .IsRequired();

            builder.Property(upv => upv.ViewedAt)
                .IsRequired();

            builder.HasOne(upv => upv.Product)
                .WithMany(p => p.UserProductViews)
                .HasForeignKey(upv => upv.ProductId);
        }
    }
}
