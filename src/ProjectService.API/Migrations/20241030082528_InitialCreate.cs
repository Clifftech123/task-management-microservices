using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTag",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTag", x => new { x.ProjectId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProjectTag_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTags",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTags", x => new { x.ProjectId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProjectTags_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedAt", "Description", "EndDate", "Name", "StartDate", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7049), new TimeSpan(0, 0, 0, 0, 0)), "Description 1", new DateTime(2024, 11, 9, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7037), "Project 1", new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7018), new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7051), new TimeSpan(0, 0, 0, 0, 0)), "user1" },
                    { 2, new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7058), new TimeSpan(0, 0, 0, 0, 0)), "Description 2", new DateTime(2024, 11, 9, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7057), "Project 2", new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7057), new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7059), new TimeSpan(0, 0, 0, 0, 0)), "user2" },
                    { 3, new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7061), new TimeSpan(0, 0, 0, 0, 0)), "Description 3", new DateTime(2024, 11, 9, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7060), "Project 3", new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Local).AddTicks(7060), new DateTimeOffset(new DateTime(2024, 10, 30, 8, 25, 26, 213, DateTimeKind.Unspecified).AddTicks(7061), new TimeSpan(0, 0, 0, 0, 0)), "user3" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "C#", "user1" },
                    { 2, "ASP.NET", "user1" },
                    { 3, "Entity Framework", "user2" },
                    { 4, "Azure", "user2" },
                    { 5, "Blazor", "user3" },
                    { 6, "Microservices", "user3" },
                    { 7, "Docker", "user4" },
                    { 8, "Kubernetes", "user4" },
                    { 9, "DevOps", "user5" },
                    { 10, "CI/CD", "user5" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTag_TagId",
                table: "ProjectTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTags_TagId",
                table: "ProjectTags",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTag");

            migrationBuilder.DropTable(
                name: "ProjectTags");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
