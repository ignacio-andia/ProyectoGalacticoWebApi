using ProyectoGalacticoWebApi.Dtos;
namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface IPersonajeService
    {
        Task<IEnumerable<PersonajeDto>> ListarPersonajesAsync();
        Task<PersonajeDto?> ObtenerPersonajePorIdAsync(int Id);
        Task<PersonajeDto> CrearPersonajeAsync(CrearPersonajeDto Dto);
        Task<bool> ActualizarPersonajeAsync(int Id, ActualizarPersonajeDto Dto);
        Task<bool> EliminarPersonajeAsync(int Id);


    }
}
