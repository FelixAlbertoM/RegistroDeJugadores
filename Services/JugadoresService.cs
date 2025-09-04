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
            await using var contexto = await DbFactory.CreateDbContextAsync();
            bool existeNombre = await contexto.Jugadores
                .AnyAsync(j => j.Nombres == jugador.Nombres && j.JugadorId != jugador.JugadorId);

            if (!await Existe(jugador.JugadorId))
            {
                return await Insertar(jugador);
            }
            else
            {
                return await Modificar(jugador);
            }
        }

        public async Task<bool> ExisteNombre(string nombre, int jugadorId = 0)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(j => j.Nombres.ToLower() == nombre.ToLower()
                               && j.JugadorId != jugadorId);
        }

        private async Task<bool> Existe(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(j => j.JugadorId == jugadorId);
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
            return await contexto.Jugadores
                .FirstOrDefaultAsync(j => j.JugadorId == jugadorId);
        }

        public async Task<bool> Eliminar(int jugadorId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AsNoTracking()
                .Where(j => j.JugadorId == jugadorId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores.Where(criterio).AsNoTracking().ToListAsync();
        }
    }
}
