using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroDeJugadores.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    JugadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Partidas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.JugadorId);
                });

            migrationBuilder.CreateTable(
                name: "Victorias",
                columns: table => new
                {
                    PartidaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jugador1Id = table.Column<int>(type: "int", nullable: false),
                    Jugador2Id = table.Column<int>(type: "int", nullable: true),
                    EstadoPartida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GanadorId = table.Column<int>(type: "int", nullable: true),
                    TurnoJugadorId = table.Column<int>(type: "int", nullable: true),
                    EstadoTablero = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.PartidaId);
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_GanadorId",
                        column: x => x.GanadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_Jugador1Id",
                        column: x => x.Jugador1Id,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_Jugador2Id",
                        column: x => x.Jugador2Id,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidas_Jugadores_TurnoJugadorId",
                        column: x => x.TurnoJugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "JugadorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_Nombres",
                table: "Jugadores",
                column: "Nombres",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_GanadorId",
                table: "Victorias",
                column: "GanadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_Jugador1Id",
                table: "Victorias",
                column: "Jugador1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_Jugador2Id",
                table: "Victorias",
                column: "Jugador2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_TurnoJugadorId",
                table: "Victorias",
                column: "TurnoJugadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Victorias");

            migrationBuilder.DropTable(
                name: "Jugadores");
        }
    }
}
