using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummyanime.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToUserAnimeFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserAnimeFavorites",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserAnimeFavorites");
        }
    }
}
