using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class adddocumentnameremovereports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyDocumentReports");

            migrationBuilder.DropIndex(
                name: "IX_Students_LastName",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StudyDocuments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocuments_Name",
                table: "StudyDocuments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserName",
                table: "Students",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyDocuments_Name",
                table: "StudyDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StudyDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "StudyDocumentReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDocumentReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyDocumentReports_StudyDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "StudyDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastName",
                table: "Students",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocumentReports_DocumentId",
                table: "StudyDocumentReports",
                column: "DocumentId");
        }
    }
}
