﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniDocuments.App.Data.EntityFramework.Context;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240509172534_teachers-and-app-improve")]
    partial class teachersandappimprove
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentStudyActivity", b =>
                {
                    b.Property<Guid>("ActivitiesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StudentsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ActivitiesId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("StudentStudyActivity");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserName");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("Name");

                    b.ToTable("StudyActivities", (string)null);
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateLoaded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fingerprint")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StudyDocumentFileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ValuableParagraphsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("Name");

                    b.HasIndex("StudentId");

                    b.ToTable("StudyDocuments", (string)null);
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyDocumentFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StudyDocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StudyDocumentId")
                        .IsUnique();

                    b.ToTable("StudyDocumentFile");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserName");

                    b.ToTable("Teachers", (string)null);
                });

            modelBuilder.Entity("StudentStudyActivity", b =>
                {
                    b.HasOne("UniDocuments.App.Domain.Models.StudyActivity", null)
                        .WithMany()
                        .HasForeignKey("ActivitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniDocuments.App.Domain.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyActivity", b =>
                {
                    b.HasOne("UniDocuments.App.Domain.Models.Teacher", "Creator")
                        .WithMany("Activities")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyDocument", b =>
                {
                    b.HasOne("UniDocuments.App.Domain.Models.StudyActivity", "Activity")
                        .WithMany("Documents")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniDocuments.App.Domain.Models.Student", "Student")
                        .WithMany("Documents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyDocumentFile", b =>
                {
                    b.HasOne("UniDocuments.App.Domain.Models.StudyDocument", "StudyDocument")
                        .WithOne("StudyDocumentFile")
                        .HasForeignKey("UniDocuments.App.Domain.Models.StudyDocumentFile", "StudyDocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudyDocument");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.Student", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyActivity", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.StudyDocument", b =>
                {
                    b.Navigation("StudyDocumentFile")
                        .IsRequired();
                });

            modelBuilder.Entity("UniDocuments.App.Domain.Models.Teacher", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
