using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RegistroDeJugadores.Models;

    [Index(nameof(Nombres), IsUnique = true)]
    public class Jugadores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JugadorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las partidas no pueden ser negativas")]
        public int Victorias { get; set; } = 0;
        public int Empates { get; set; } = 0;
        public int Derrotas { get; set; } = 0;
        public int Jugadas { get; set; } = 0;

}

