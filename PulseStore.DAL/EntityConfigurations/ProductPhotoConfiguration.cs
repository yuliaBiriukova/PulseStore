using PulseStore.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.EntityConfigurations;

public class ProductPhotoConfiguration : IEntityTypeConfiguration<ProductPhoto>
{
    public void Configure(EntityTypeBuilder<ProductPhoto> builder)
    {
        builder.Property(p => p.ImagePath).IsRequired();

        builder.Property(p => p.SequenceNumber).IsRequired();

        builder.HasOne(p => p.Product)
           .WithMany(p => p.ProductPhotos)
           .HasForeignKey(p => p.ProductId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasData( new ProductPhoto { 
            Id = 1, 
            ImagePath = "https://PulseStorestorage.blob.core.windows.net/photo-container/testproduct.jpeg", 
            ProductId = 1,
            SequenceNumber = 1}
        );
    }
}