using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lullaby.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupKey = table.Column<string>(type: "text", nullable: false),
                    EventStarts = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventEnds = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDateTimeDetailed = table.Column<bool>(type: "boolean", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    EventDescription = table.Column<string>(type: "text", nullable: false),
                    EventPlace = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventType = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

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
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
