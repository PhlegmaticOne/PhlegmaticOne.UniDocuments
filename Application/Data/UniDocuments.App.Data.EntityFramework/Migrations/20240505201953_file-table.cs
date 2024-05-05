using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class filetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudyDocumentFileId",
                table: "StudyDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudyDocumentFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudyDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDocumentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyDocumentFile_StudyDocuments_StudyDocumentId",
                        column: x => x.StudyDocumentId,
                        principalTable: "StudyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocumentFile_StudyDocumentId",
                table: "StudyDocumentFile",
                column: "StudyDocumentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyDocumentFile");

            migrationBuilder.DropColumn(
                name: "StudyDocumentFileId",
                table: "StudyDocuments");
        }
    }
}
