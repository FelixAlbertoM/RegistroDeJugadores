using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroDeJugadores.Models;

    public class Movimientos
    {
        [Key]
        public int MovimientoId { get; set; }
        public int PartidaId { get; set; }
        public int JugadorId { get; set; }
        [Range(0, 2)]
        public int PosicionFila { get; set; }
        [Range(0, 2)]
        public int PosicionColumna { get; set; }
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        //Propiedades de navegacion
        [ForeignKey(nameof(PartidaId))]
        public virtual Partidas Partida { get; set; }

        [ForeignKey(nameof(JugadorId))]
        public virtual Jugadores Jugador { get; set; }

        }

