using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.DAL;

    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) {}

        public DbSet<Jugadores> Jugador { get; set; }
    }

