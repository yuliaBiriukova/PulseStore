using PulseStore.BLL.Entities.TemplateFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PulseStore.DAL.EntityConfigurations;

public class TemplateFileConfiguration : IEntityTypeConfiguration<TemplateFile>
{
    public void Configure(EntityTypeBuilder<TemplateFile> builder)
    {
        builder.Property(t => t.FilePath).HasMaxLength(256);
        builder.HasIndex(t => t.Type).IsUnique();
    }
}