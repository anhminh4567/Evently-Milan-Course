using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Evently.Modules.Users.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "users",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "users",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "users",
                columns: table => new
                {
                    PermissionCode = table.Column<string>(type: "character varying(100)", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.PermissionCode, x.RoleName });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionCode",
                        column: x => x.PermissionCode,
                        principalSchema: "users",
                        principalTable: "Permissions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleName",
                        column: x => x.RoleName,
                        principalSchema: "users",
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "users",
                columns: table => new
                {
                    RolesName = table.Column<string>(type: "character varying(50)", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesName, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesName",
                        column: x => x.RolesName,
                        principalSchema: "users",
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "Permissions",
                column: "Code",
                values: new object[]
                {
                    "carts:add",
                    "carts:read",
                    "carts:remove",
                    "categories:read",
                    "categories:update",
                    "event-statistics:read",
                    "events:read",
                    "events:search",
                    "events:update",
                    "orders:create",
                    "orders:read",
                    "ticket-types:read",
                    "ticket-types:update",
                    "tickets:check-in",
                    "tickets:read",
                    "users:read",
                    "users:update"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "Roles",
                column: "Name",
                values: new object[]
                {
                    "Administrator",
                    "Member"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "RolePermissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "carts:add", "Administrator" },
                    { "carts:add", "Member" },
                    { "carts:read", "Administrator" },
                    { "carts:read", "Member" },
                    { "carts:remove", "Administrator" },
                    { "carts:remove", "Member" },
                    { "categories:read", "Administrator" },
                    { "categories:update", "Administrator" },
                    { "event-statistics:read", "Administrator" },
                    { "events:read", "Administrator" },
                    { "events:search", "Administrator" },
                    { "events:search", "Member" },
                    { "events:update", "Administrator" },
                    { "orders:create", "Administrator" },
                    { "orders:create", "Member" },
                    { "orders:read", "Administrator" },
                    { "orders:read", "Member" },
                    { "ticket-types:read", "Administrator" },
                    { "ticket-types:read", "Member" },
                    { "ticket-types:update", "Administrator" },
                    { "tickets:check-in", "Administrator" },
                    { "tickets:check-in", "Member" },
                    { "tickets:read", "Administrator" },
                    { "tickets:read", "Member" },
                    { "users:read", "Administrator" },
                    { "users:read", "Member" },
                    { "users:update", "Administrator" },
                    { "users:update", "Member" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleName",
                schema: "users",
                table: "RolePermissions",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "users",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "users");
        }
    }
}
