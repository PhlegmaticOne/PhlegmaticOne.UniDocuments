using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudyDocumentFileConfiguration : IEntityTypeConfiguration<StudyDocumentFile>
{
    public void Configure(EntityTypeBuilder<StudyDocumentFile> builder)
    {
        builder.ToTable(ConfigurationConstants.StudyDocumentFilesTableName);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.Content).IsRequired();
    }
}