using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.DAL;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.Services;

public class PartidasService
{
    private readonly IDbContextFactory<Contexto> _dbFactory;
    private readonly ILogger<PartidasService> _logger;

    public PartidasService(IDbContextFactory<Contexto> bdFactory, ILogger<PartidasService> logger)
    {
        _dbFactory = bdFactory;
        _logger = logger;
    }

    private async Task<bool> Existe(int partidaId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas.AnyAsync(p => p.PartidaId == partidaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando Existencia de la partida {PartidaId}", partidaId);
            return false;
        }
    }

    private async Task<bool> Insertar(Partidas partida)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            contexto.Partidas.Add(partida);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error insertando partida de {Jugador1Id} vs {Jugador2Id}", partida.Jugador1Id,
                partida.Jugador2Id);
            return false;
        }
    }

    private async Task<bool> Modificar(Partidas partida)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            contexto.Update(partida);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, " Error al modificar la partida {PartidaId}", partida.PartidaId);
            return false;
        }
    }

    public async Task<bool> Guardar(Partidas partida)
    {
        try
        {
            bool existe = await Existe(partida.PartidaId);
            _logger.LogInformation("Partida {PartidaId} existe: {Existe}", partida.PartidaId, existe);
            return !existe
                ? await Insertar(partida)
                : await Modificar(partida);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando Partida {PartidaId}", partida.PartidaId);
            return false;
        }
    }

    public async Task<Partidas> Buscar(int partidaId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas
                .Include(p => p.Jugador1)
                .Include(p => p.Jugador2)
                .Include(p => p.Ganador)
                .Include(p => p.TurnoJugador)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PartidaId == partidaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error buscando la partida {PartidaId}", partidaId);
            return null;
        }
    }

    public async Task<bool> Eliminar(int partidaId)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.Partidas.Where(p => p.PartidaId == partidaId).ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la partida {PartidaId}", partidaId);
            return false;
        }
    }

    public async Task<List<Partidas>> Listar(Expression<Func<Partidas, bool>> criterio)
    {
        try
        { 
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            var query = contexto.Partidas
                .Include(p => p.Jugador1)
                .Include(p => p.Jugador2)
                .Include(p => p.Ganador)
                .Include(p => p.TurnoJugador)
                .Where(criterio);
            var resultados= await query.AsNoTracking().ToListAsync();
            return resultados.Where(p => p.Jugador1 != null || p.Jugador2 != null).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Listar la partida");
            return new List<Partidas>();
        }
    }


    public async Task<int> TotalPartidasJugadas()
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Partidas.CountAsync();
    }

    public async Task<int> TotalPartidasGanadas(int jugadorId = 0)
    {
        try
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return jugadorId == 0
                ? await contexto.Partidas.CountAsync(p => p.GanadorId != null)
                : await contexto.Partidas.CountAsync(p => p.GanadorId == jugadorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculando total de partidas ganadas");
            return 0;
        }
    }

    public async Task ActualizarEstadisticas(Partidas partida)
    {
        await using var contexto = await _dbFactory.CreateDbContextAsync();

        var jugador1 = await contexto.Jugadores.FindAsync(partida.Jugador1Id);
        var jugador2 = await contexto.Jugadores.FindAsync(partida.Jugador2Id);

        if (jugador1 == null || jugador2 == null)
            return;

        jugador1.Jugadas++;
        jugador2.Jugadas++;

        if (partida.GanadorId == null)
        {
            jugador1.Empates++;
            jugador2.Empates++;
        }
        else
        {
            foreach(var jugador in new[] { jugador1, jugador2 })
            {
                if (jugador.JugadorId == partida.GanadorId)
                    jugador.Partidas++;
                else
                    jugador.Derrotas++;
            }
        }
        await contexto.SaveChangesAsync();

    }

     
}