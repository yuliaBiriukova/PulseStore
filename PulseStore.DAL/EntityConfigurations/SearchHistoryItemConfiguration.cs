using PulseStore.BLL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PulseStore.DAL.EntityConfigurations;

public class SearchHistoryItemConfiguration : IEntityTypeConfiguration<SearchHistoryItem>
{
    public void Configure(EntityTypeBuilder<SearchHistoryItem> builder)
    {
        builder.Property(s => s.UserId).IsRequired().HasMaxLength(450);
        builder.Property(s => s.Query)
            .IsRequired()
            .HasMaxLength(256)
            .UseCollation("SQL_Latin1_General_CP1_CS_AS");
        builder.Property(s => s.Date).IsRequired();
    }
}