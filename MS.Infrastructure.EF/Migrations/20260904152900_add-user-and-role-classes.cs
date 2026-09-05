using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MS.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class adduserandroleclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "Email", "Password" },
                values: new object[,]
                {
                    { 1, "bw@gmail.com", "1939" },
                    { 2, "soliwp@gmail.com", "1380" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "RoleId", "RoleName", "UserId" },
                values: new object[,]
                {
                    { 1, "admin", 1 },
                    { 2, "patient", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_roles_UserId",
                table: "roles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
