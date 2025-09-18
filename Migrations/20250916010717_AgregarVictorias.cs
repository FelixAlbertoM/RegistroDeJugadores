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
                table: "Victorias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Empates",
                table: "Victorias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victorias",
                table: "Victorias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Derrotas",
                table: "Victorias");

            migrationBuilder.DropColumn(
                name: "Empates",
                table: "Victorias");

            migrationBuilder.DropColumn(
                name: "Victorias",
                table: "Victorias");
        }
    }
}
