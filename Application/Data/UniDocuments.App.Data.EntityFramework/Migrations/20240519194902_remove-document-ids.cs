using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class removedocumentids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyDocumentFiles_StudyDocumentId",
                table: "StudyDocumentFiles");

            migrationBuilder.DropColumn(
                name: "StudyDocumentFileId",
                table: "StudyDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocumentFiles_StudyDocumentId",
                table: "StudyDocumentFiles",
                column: "StudyDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyDocumentFiles_StudyDocumentId",
                table: "StudyDocumentFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "StudyDocumentFileId",
                table: "StudyDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocumentFiles_StudyDocumentId",
                table: "StudyDocumentFiles",
                column: "StudyDocumentId",
                unique: true);
        }
    }
}
