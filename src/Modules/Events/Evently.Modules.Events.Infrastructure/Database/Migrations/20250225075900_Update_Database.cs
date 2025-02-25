using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Modules.Events.Api.Database.Migrations;

/// <inheritdoc />
public partial class Update_Database : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "pk_events",
            schema: "events",
            table: "events");

        migrationBuilder.RenameTable(
            name: "events",
            schema: "events",
            newName: "Events",
            newSchema: "events");

        migrationBuilder.RenameColumn(
            name: "title",
            schema: "events",
            table: "Events",
            newName: "Title");

        migrationBuilder.RenameColumn(
            name: "status",
            schema: "events",
            table: "Events",
            newName: "Status");

        migrationBuilder.RenameColumn(
            name: "location",
            schema: "events",
            table: "Events",
            newName: "Location");

        migrationBuilder.RenameColumn(
            name: "description",
            schema: "events",
            table: "Events",
            newName: "Description");

        migrationBuilder.RenameColumn(
            name: "id",
            schema: "events",
            table: "Events",
            newName: "Id");

        migrationBuilder.RenameColumn(
            name: "starts_at_utc",
            schema: "events",
            table: "Events",
            newName: "StartsAtUtc");

        migrationBuilder.RenameColumn(
            name: "ends_at_utc",
            schema: "events",
            table: "Events",
            newName: "EndsAtUtc");

        migrationBuilder.AlterColumn<string>(
            name: "Id",
            schema: "events",
            table: "Events",
            type: "text",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uuid");

        migrationBuilder.AddColumn<string>(
            name: "CategoryId",
            schema: "events",
            table: "Events",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Events",
            schema: "events",
            table: "Events",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "Categories",
            schema: "events",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                IsArchived = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TicketTypes",
            schema: "events",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                EventId = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Price = table.Column<decimal>(type: "numeric", nullable: false),
                Currency = table.Column<string>(type: "text", nullable: false),
                Quantity = table.Column<decimal>(type: "numeric", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TicketTypes", x => x.Id);
                table.ForeignKey(
                    name: "FK_TicketTypes_Events_EventId",
                    column: x => x.EventId,
                    principalSchema: "events",
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Events_CategoryId",
            schema: "events",
            table: "Events",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_TicketTypes_EventId",
            schema: "events",
            table: "TicketTypes",
            column: "EventId");

        migrationBuilder.AddForeignKey(
            name: "FK_Events_Categories_CategoryId",
            schema: "events",
            table: "Events",
            column: "CategoryId",
            principalSchema: "events",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Events_Categories_CategoryId",
            schema: "events",
            table: "Events");

        migrationBuilder.DropTable(
            name: "Categories",
            schema: "events");

        migrationBuilder.DropTable(
            name: "TicketTypes",
            schema: "events");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Events",
            schema: "events",
            table: "Events");

        migrationBuilder.DropIndex(
            name: "IX_Events_CategoryId",
            schema: "events",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "CategoryId",
            schema: "events",
            table: "Events");

        migrationBuilder.RenameTable(
            name: "Events",
            schema: "events",
            newName: "events",
            newSchema: "events");

        migrationBuilder.RenameColumn(
            name: "Title",
            schema: "events",
            table: "events",
            newName: "title");

        migrationBuilder.RenameColumn(
            name: "Status",
            schema: "events",
            table: "events",
            newName: "status");

        migrationBuilder.RenameColumn(
            name: "Location",
            schema: "events",
            table: "events",
            newName: "location");

        migrationBuilder.RenameColumn(
            name: "Description",
            schema: "events",
            table: "events",
            newName: "description");

        migrationBuilder.RenameColumn(
            name: "Id",
            schema: "events",
            table: "events",
            newName: "id");

        migrationBuilder.RenameColumn(
            name: "StartsAtUtc",
            schema: "events",
            table: "events",
            newName: "starts_at_utc");

        migrationBuilder.RenameColumn(
            name: "EndsAtUtc",
            schema: "events",
            table: "events",
            newName: "ends_at_utc");

        migrationBuilder.AlterColumn<Guid>(
            name: "id",
            schema: "events",
            table: "events",
            type: "uuid",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddPrimaryKey(
            name: "pk_events",
            schema: "events",
            table: "events",
            column: "id");
    }
}
