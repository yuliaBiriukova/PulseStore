using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PulseStore.BLL.Entities;

namespace PulseStore.DAL.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(256);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasData(
            new Category { Id = 1, Name = "BCAA"},
            new Category { Id = 2, Name = "Fat Burner" });
    }
}