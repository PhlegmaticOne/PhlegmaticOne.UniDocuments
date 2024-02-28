using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhlegmaticOne.UniDocuments.Domain.Models;

namespace PhlegmaticOne.UniDocuments.Data.EntityFramework.Configurations;

public class StudyDocumentReportConfiguration : IEntityTypeConfiguration<StudyDocumentReport>
{
    public void Configure(EntityTypeBuilder<StudyDocumentReport> builder)
    {
        builder.ToTable(ConfigurationConstants.StudyDocumentReportsTableName);

        builder.HasKey(t => t.Id);

        builder.HasOne(x => x.Document)
            .WithOne(x => x.Report)
            .HasForeignKey<StudyDocument>(x => x.ReportId);

        builder.Property(x => x.Description).IsRequired();
    }
}
