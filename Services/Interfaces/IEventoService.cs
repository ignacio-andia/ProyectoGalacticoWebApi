using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface IEventoService
    {
        // Gestion de eventos
        Task<IEnumerable<EventoDto>> ListarEventosAsync();
        Task<EventoDto?> ObtenerEventoPorIdAsync(int id);
        Task<ResultadoOperacion<EventoDto>> CrearEventoAsync(CrearEventoDto dto);
        Task<ResultadoOperacion<bool>> ActualizarEventoAsync(int id, ActualizarEventoDto dto);


        // Gestion de participantes
        Task<ResultadoOperacion<bool>> AgregarParticipanteAsync(int eventoId, int personajeId);
        Task<ResultadoOperacion<bool>> QuitarParticipanteAsync(int eventoId, int personajeId);
        //Task<IEnumerable<PersonajeDto>?> ListarParticipantesAsync(int eventoId);


        // Consultas
        //Task<IEnumerable<EventoDto>?> ObtenerEventosPorPersonajeAsync(int personajeId);
        //Task<PersonajeDto?> ObtenerMVPAsync(int eventoId);


        // Simulacion
        //Task<ResultadoOperacion<SimulacionEventoDto>> SimularEventoAsync(int eventoId);
    }
}
