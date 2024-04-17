using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class studentusernames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Students");
        }
    }
}
