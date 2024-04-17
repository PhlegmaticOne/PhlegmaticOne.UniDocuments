using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniDocuments.App.Domain.Models;

namespace UniDocuments.App.Data.EntityFramework.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable(ConfigurationConstants.StudentsTableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(ConfigurationConstants.NamePropertyMaxLength);

        builder.Property(x => x.Password).IsRequired();

        builder.Property(x => x.UserName).IsRequired();

        builder.HasIndex(x => x.LastName);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Students)
            .HasForeignKey(x => x.GroupId);
    }
}