using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.DAL;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.Services
{
    public class JugadoresService(IDbContextFactory<Contexto> DbFactory)
    {
        public async Task<bool> Guardar(Jugadores jugador)
        {
            if (!await Existe(jugador.JugadorId))
            {
                return await Insertar(jugador);
            }
            else
            {
                return await Modificar(jugador);
            }
        }

        private async Task<bool> Existe(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(p => p.JugadorId == jugadorId);
        }

        private async Task<bool> Insertar(Jugadores jugador)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Jugadores.Add(jugador);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Jugadores jugador)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(jugador);
            return await contexto
                .SaveChangesAsync() > 0;
        }

        public async Task<Jugadores?> Buscar(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores.Include(d => d.Partidas)
                .FirstOrDefaultAsync(p => p.JugadorId == jugadorId);
        }

        public async Task<bool> Eliminar(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AsNoTracking()
                .Where(p => p.JugadorId == jugadorId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .Where(criterio)
                .ToListAsync();
        }
    }
}
