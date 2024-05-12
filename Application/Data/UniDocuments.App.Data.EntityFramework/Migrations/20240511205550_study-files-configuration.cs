using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class studyfilesconfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocumentFile_StudyDocuments_StudyDocumentId",
                table: "StudyDocumentFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyDocumentFile",
                table: "StudyDocumentFile");

            migrationBuilder.RenameTable(
                name: "StudyDocumentFile",
                newName: "StudyDocumentFiles");

            migrationBuilder.RenameIndex(
                name: "IX_StudyDocumentFile_StudyDocumentId",
                table: "StudyDocumentFiles",
                newName: "IX_StudyDocumentFiles_StudyDocumentId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudyDocumentFiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyDocumentFiles",
                table: "StudyDocumentFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocumentFiles_StudyDocuments_StudyDocumentId",
                table: "StudyDocumentFiles",
                column: "StudyDocumentId",
                principalTable: "StudyDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocumentFiles_StudyDocuments_StudyDocumentId",
                table: "StudyDocumentFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyDocumentFiles",
                table: "StudyDocumentFiles");

            migrationBuilder.RenameTable(
                name: "StudyDocumentFiles",
                newName: "StudyDocumentFile");

            migrationBuilder.RenameIndex(
                name: "IX_StudyDocumentFiles_StudyDocumentId",
                table: "StudyDocumentFile",
                newName: "IX_StudyDocumentFile_StudyDocumentId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudyDocumentFile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyDocumentFile",
                table: "StudyDocumentFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocumentFile_StudyDocuments_StudyDocumentId",
                table: "StudyDocumentFile",
                column: "StudyDocumentId",
                principalTable: "StudyDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
