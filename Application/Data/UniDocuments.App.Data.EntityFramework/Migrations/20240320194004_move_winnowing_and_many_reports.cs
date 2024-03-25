using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class move_winnowing_and_many_reports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocuments_StudyDocumentMetrics_MetricsId",
                table: "StudyDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocuments_StudyDocumentReports_ReportId",
                table: "StudyDocuments");

            migrationBuilder.DropTable(
                name: "StudyDocumentMetrics");

            migrationBuilder.DropIndex(
                name: "IX_StudyDocuments_MetricsId",
                table: "StudyDocuments");

            migrationBuilder.DropIndex(
                name: "IX_StudyDocuments_ReportId",
                table: "StudyDocuments");

            migrationBuilder.DropColumn(
                name: "MetricsId",
                table: "StudyDocuments");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "StudyDocuments");

            migrationBuilder.AddColumn<byte[]>(
                name: "WinnowingData",
                table: "StudyDocuments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "StudyDocumentReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocumentReports_DocumentId",
                table: "StudyDocumentReports",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocumentReports_StudyDocuments_DocumentId",
                table: "StudyDocumentReports",
                column: "DocumentId",
                principalTable: "StudyDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyDocumentReports_StudyDocuments_DocumentId",
                table: "StudyDocumentReports");

            migrationBuilder.DropIndex(
                name: "IX_StudyDocumentReports_DocumentId",
                table: "StudyDocumentReports");

            migrationBuilder.DropColumn(
                name: "WinnowingData",
                table: "StudyDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "StudyDocumentReports");

            migrationBuilder.AddColumn<Guid>(
                name: "MetricsId",
                table: "StudyDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "StudyDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudyDocumentMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinnowingData = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StudyDocuments_ReportId",
                table: "StudyDocuments",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocuments_StudyDocumentMetrics_MetricsId",
                table: "StudyDocuments",
                column: "MetricsId",
                principalTable: "StudyDocumentMetrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyDocuments_StudyDocumentReports_ReportId",
                table: "StudyDocuments",
                column: "ReportId",
                principalTable: "StudyDocumentReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
