namespace RegistroDeJugadores.DTOs;

    public class PartidaDTO
    {
        public int PartidaId { get; set; }
        public string Jugador1 { get; set; } = string.Empty;
        public string Jugador2 { get; set; } = string.Empty;
        public string EstadoPartida { get; set; } = string.Empty;
        public string Ganador { get; set; } = string.Empty;
    }

