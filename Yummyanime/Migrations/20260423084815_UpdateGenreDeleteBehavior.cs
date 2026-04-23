using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummyanime.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenreDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Genres_GenreId",
                table: "Animes");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Genres_GenreId",
                table: "Animes",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Genres_GenreId",
                table: "Animes");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Genres_GenreId",
                table: "Animes",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
