using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface IMisionService
    {
        Task<ResultadoOperacion<MisionDto>> CrearMisionAsync(CrearMisionDto dto);

        Task<IEnumerable<MisionDto>> ListarMisionesAsync();

        Task<ResultadoOperacion<bool>> AgregarParticipanteAsync(int misionId, int personajeId);

        Task<ResultadoOperacion<EjecucionMisionDto>> IniciarMisionAsync(int misionId);
    }
}
