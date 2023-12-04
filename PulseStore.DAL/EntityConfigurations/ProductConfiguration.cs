using PulseStore.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(256);

        builder.Property(p => p.Description).HasMaxLength(500);

        builder.Property(p => p.Price).IsRequired().HasPrecision(10, 2);

        builder.Property(p => p.IsPublished).IsRequired();

        builder.Property(p => p.DateCreated).IsRequired();

        builder.HasOne(p => p.Category)
           .WithMany(c => c.Products)
           .HasForeignKey(p => p.CategoryId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new Product { 
                Id = 1, 
                Name = "Olimp Labs, BCAA Xplode Powder, 500 g", 
                Description = "High quality balanced amino acid complex.",
                Price = 25,
                MinTemperature = 0,
                MaxTemperature = 25,
                IsPublished = true,
                DateCreated = DateTime.Parse("2023-09-28 16:45:20.1803137"),
                CategoryId = 1
            });
    }
}