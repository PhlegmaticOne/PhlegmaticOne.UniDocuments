using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class IdToIntAndAddMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MetricsId",
                table: "StudyDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudyDocumentMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinnowingData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FingerprintsData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDocumentMetrics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocuments_MetricsId",
                table: "StudyDocuments",
                column: "MetricsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocuments_StudyDocumentMetrics_MetricsId",
                table: "StudyDocuments",
                column: "MetricsId",
                principalTable: "StudyDocumentMetrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocuments_StudyDocumentMetrics_MetricsId",
                table: "StudyDocuments");

            migrationBuilder.DropTable(
                name: "StudyDocumentMetrics");

            migrationBuilder.DropIndex(
                name: "IX_StudyDocuments_MetricsId",
                table: "StudyDocuments");

            migrationBuilder.DropColumn(
                name: "MetricsId",
                table: "StudyDocuments");
        }
    }
}
