using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudyDocumentConfiguration : IEntityTypeConfiguration<StudyDocument>
{
    public void Configure(EntityTypeBuilder<StudyDocument> builder)
    {
        builder.ToTable(ConfigurationConstants.StudyDocumentsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DateLoaded);

        builder.Property(x => x.Fingerprint);

        builder.Property(x => x.ValuableParagraphsCount);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasOne(x => x.StudyDocumentFile)
            .WithOne(x => x.StudyDocument)
            .HasForeignKey<StudyDocumentFile>(x => x.StudyDocumentId);

        builder.HasIndex(x => x.Name);
    }
}
