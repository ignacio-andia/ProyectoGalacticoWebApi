using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface ICredencialService
    {
        Task<ResultadoOperacion<CredencialPersonajeDto>> CrearCredencialAsync(int personajeId, CrearCredencialDto dto);
        Task<ResultadoOperacion<CredencialPersonajeDto>> ObtenerCredencialAsync(int personajeId);
    }
}
