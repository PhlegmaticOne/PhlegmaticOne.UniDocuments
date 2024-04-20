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

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasOne(x => x.Activity)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.ActivityId);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.StudentId);

        builder.HasIndex(x => x.Name);
    }
}
