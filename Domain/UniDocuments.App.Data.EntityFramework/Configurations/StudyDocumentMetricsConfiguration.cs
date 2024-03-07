using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudyDocumentMetricsConfiguration : IEntityTypeConfiguration<StudyDocumentMetrics>
{
    public void Configure(EntityTypeBuilder<StudyDocumentMetrics> builder)
    {
        builder.ToTable(ConfigurationConstants.StudyDocumentMetricsTableName);

        builder.HasKey(t => t.Id);

        builder.HasOne(x => x.Document)
            .WithOne(x => x.Metrics)
            .HasForeignKey<StudyDocument>(x => x.MetricsId);

        builder.Property(x => x.WinnowingData).IsRequired();
    }
}
