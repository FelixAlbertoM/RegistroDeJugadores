using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.DAL;

public class Contexto : DbContext
{
    public DbSet<Jugadores> Jugadores { get; set; }
    public DbSet<Partidas> Partidas { get; set; }

    public Contexto(DbContextOptions<Contexto> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       
        modelBuilder.Entity<Partidas>()
            .HasOne(p => p.Jugador1)
            .WithMany()
            .HasForeignKey(p => p.Jugador1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
            .HasOne(p => p.Jugador2)
            .WithMany()
            .HasForeignKey(p => p.Jugador2Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
            .HasOne(p => p.Ganador)
            .WithMany()
            .HasForeignKey(p => p.GanadorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partidas>()
            .HasOne(p => p.TurnoJugador)
            .WithMany()
            .HasForeignKey(p => p.TurnoJugadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

