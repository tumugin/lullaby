using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lullaby.Migrations
{
    /// <inheritdoc />
    public partial class AddEventIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GroupKey",
                table: "Events",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventEnds",
                table: "Events",
                column: "EventEnds");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventStarts",
                table: "Events",
                column: "EventStarts");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GroupKey",
                table: "Events",
                column: "GroupKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_EventEnds",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventStarts",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_GroupKey",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "GroupKey",
                table: "Events",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
