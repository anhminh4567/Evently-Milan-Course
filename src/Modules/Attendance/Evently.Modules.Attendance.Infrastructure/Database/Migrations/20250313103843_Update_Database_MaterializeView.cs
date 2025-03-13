using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Attendance.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database_MaterializeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventStatistics",
                schema: "attendance",
                columns: table => new
                {
                    EventId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TicketsSold = table.Column<int>(type: "integer", nullable: false),
                    AttendeesCheckedIn = table.Column<int>(type: "integer", nullable: false),
                    DuplicateCheckInTickets = table.Column<List<string>>(type: "text[]", nullable: false),
                    InvalidCheckInTickets = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatistics", x => x.EventId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStatistics",
                schema: "attendance");
        }
    }
}
