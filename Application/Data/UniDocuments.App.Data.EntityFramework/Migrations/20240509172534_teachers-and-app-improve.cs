using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniDocuments.App.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class teachersandappimprove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "StudyActivities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StudentStudyActivity",
                columns: table => new
                {
                    ActivitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStudyActivity", x => new { x.ActivitiesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_StudentStudyActivity_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentStudyActivity_StudyActivities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "StudyActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyActivities_CreatorId",
                table: "StudyActivities",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyActivity_StudentsId",
                table: "StudentStudyActivity",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserName",
                table: "Teachers",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyActivities_Teachers_CreatorId",
                table: "StudyActivities",
                column: "CreatorId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyActivities_Teachers_CreatorId",
                table: "StudyActivities");

            migrationBuilder.DropTable(
                name: "StudentStudyActivity");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_StudyActivities_CreatorId",
                table: "StudyActivities");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StudyActivities");
        }
    }
}
