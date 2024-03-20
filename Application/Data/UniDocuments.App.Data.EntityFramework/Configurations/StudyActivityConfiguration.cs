using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudyActivityConfiguration : IEntityTypeConfiguration<StudyActivity>
{
    public void Configure(EntityTypeBuilder<StudyActivity> builder)
    {
        builder.ToTable(ConfigurationConstants.StudyActivitiesTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.HasIndex(x => x.Name);

        builder.HasMany(x => x.Groups)
            .WithMany(x => x.Activities);

        builder.HasMany(x => x.Documents)
            .WithOne(x => x.Activity)
            .HasForeignKey(x => x.ActivityId);
    }
}
