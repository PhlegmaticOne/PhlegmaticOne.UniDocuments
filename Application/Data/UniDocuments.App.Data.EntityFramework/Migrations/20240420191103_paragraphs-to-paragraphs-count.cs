using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class paragraphstoparagraphscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValuableParagraphs",
                table: "StudyDocuments");

            migrationBuilder.AddColumn<int>(
                name: "ValuableParagraphsCount",
                table: "StudyDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValuableParagraphsCount",
                table: "StudyDocuments");

            migrationBuilder.AddColumn<string>(
                name: "ValuableParagraphs",
                table: "StudyDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
