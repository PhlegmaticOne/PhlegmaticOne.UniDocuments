using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Enums;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable(ConfigurationConstants.StudentsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasDefaultValue(ApplicationRole.Default)
            .HasConversion<int>();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.HasMany(x => x.Activities)
            .WithMany(x => x.Students);

        builder.HasMany(x => x.Documents)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentId);

        builder.Property(x => x.JoinDate).IsRequired();

        builder.Property(x => x.Password).IsRequired();

        builder.Property(x => x.UserName).IsRequired();

        builder.HasIndex(x => x.UserName);
    }
}