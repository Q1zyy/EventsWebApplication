using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsWebApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MicroBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswrodHash",
                table: "Users",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "PasswrodHash");
        }
    }
}
