using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroDeJugadores.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVictorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Derrotas",
                table: "Partidas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Empates",
                table: "Partidas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Partidas",
                table: "Partidas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Derrotas",
                table: "Partidas");

            migrationBuilder.DropColumn(
                name: "Empates",
                table: "Partidas");

            migrationBuilder.DropColumn(
                name: "Partidas",
                table: "Partidas");
        }
    }
}
