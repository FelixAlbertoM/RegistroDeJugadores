using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.DAL;

public class Contexto : DbContext
{
    public DbSet<Jugadores> Jugadores { get; set; }
    public DbSet<Partidas> Partidas { get; set; }
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

}

