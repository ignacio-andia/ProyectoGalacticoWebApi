using ProyectoGalacticoWebApi.Dtos;

namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface ICardsPersonajeService
    {
        Task<IEnumerable<CardsPersonajeDto>> ListarCardsAsync();
        Task<CardsPersonajeDto?> ObtenerCardPorIdAsync(int id);
        Task<CardsPersonajeDto?> ObtenerCardDePersonajeAsync(int id);
        Task<CardsPersonajeDto> CrearCardAsync(CrearCardsPersonajeDto dto);
        Task<bool> ActualizarCardAsync(int id, ActualizarCardsPersonajeDto dto);
        Task<bool> EliminarCardAsync(int id);

    }
}
