using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable(ConfigurationConstants.TeachersTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasDefaultValue(StudyRole.Default)
            .HasConversion<int>();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasMany(x => x.Activities)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId);

        builder.Property(x => x.Password).IsRequired();

        builder.Property(x => x.UserName).IsRequired();

        builder.HasIndex(x => x.UserName);
    }
}
