using PulseStore.BLL.Entities.OrderDocuments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations;

public class OrderDocumentConfiguration : IEntityTypeConfiguration<OrderDocument>
{
    public void Configure(EntityTypeBuilder<OrderDocument> builder)
    {
        builder.HasKey(od => new { od.OrderId, od.Type });
        builder.Property(od => od.FilePath).HasMaxLength(256);

        builder.HasOne(od => od.Order)
           .WithMany()
           .HasForeignKey(od => od.OrderId);
    }
}