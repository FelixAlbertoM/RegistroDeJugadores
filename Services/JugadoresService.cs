using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroDeJugadores.DAL;
using RegistroDeJugadores.DTOs;
using RegistroDeJugadores.Models;

namespace RegistroDeJugadores.Services;

public class JugadoresService
{
    private readonly IDbContextFactory<Contexto> DbFactory;
    private readonly ILogger<JugadoresService> _logger;

    public JugadoresService(IDbContextFactory<Contexto> dbFactory, ILogger<JugadoresService> logger)
    {
        DbFactory = dbFactory;
        _logger = logger;
    }
    public async Task<bool> Guardar(Jugadores jugador)
    {
        try
        {
            return !await Existe(jugador.JugadorId)
                ? await Insertar(jugador)
                : await Modificar(jugador);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar el jugador {JugadorId}", jugador.JugadorId);
            return false;
        }

    }

    public async Task<bool> ExisteNombre(string nombre, int jugadorId = 0)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Jugadores
            .AnyAsync(j => j.Nombres != null && j.Nombres.ToLower() == nombre.ToLower()
                           && j.JugadorId != jugadorId);
    }

    private async Task<bool> Existe(int jugadorId)
    {
        try
        {
            await using var contexto =  await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AnyAsync(j => j.JugadorId == jugadorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando la existencia del jugador {JugadorId}", jugadorId);
            return false;
        }
    }

    private async Task<bool> Insertar(Jugadores jugador)
    {

        try
        { 
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Jugadores.Add(jugador);
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error insertando jugador con nombre {Nombres}", jugador.Nombres);
            return false;
        }
    }

    private async Task<bool> Modificar(Jugadores jugador)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jugador.Nombres))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (jugador.Partidas < 0)
                throw new ArgumentException("Las partidas no pueden ser negativas.");

            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Update(jugador);
            return await contexto
                .SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error modificando jugador {JugadorId}", jugador.JugadorId);
            return false;
        }
    }

    public async Task<Jugadores?> Buscar(int jugadorId)
    {
        try
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .FirstOrDefaultAsync(j => j.JugadorId == jugadorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error buscando jugador {JugadorId}", jugadorId);
            return null;
        }
    }

    public async Task<bool> Eliminar(int jugadorId)
    {
        try
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .AsNoTracking()
                .Where(j => j.JugadorId == jugadorId)
                .ExecuteDeleteAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando jugador {JugadorId}", jugadorId);
            return false;
        }
    }

    public async Task<List<Jugadores>> Listar(Expression<Func<Jugadores, bool>> criterio)
    {
        try
        {

            using var contexto = await DbFactory.CreateDbContextAsync();
            var query = contexto.Jugadores.AsQueryable();
            query = query.Where(criterio);
            return await query.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listando jugadores");
            return new List<Jugadores>();
        }
    }

    public async Task<List<JugadorDTO>> ListarDTO(Expression<Func<Jugadores, bool>> criterio)
    {
        try
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Jugadores
                .Where(criterio)
                .Select(j => new JugadorDTO
                {
                    JugadorId = j.JugadorId,
                    Nombres = j.Nombres,
                    Partidas = j.Partidas
                })
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listando jugadores (DTO)");
            return new List<JugadorDTO>();
        }
    }
}



