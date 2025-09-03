using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RegistroDeJugadores.Models
{
    [Index(nameof(Nombres), IsUnique = true)]
    public class Jugadores
    {
        [Key]
        public int JugadorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombres { get; set; }


        [Required(ErrorMessage = "La cantidad de partidas son obligatorias")]
        public int Partidas { get; set; }

    }
}
