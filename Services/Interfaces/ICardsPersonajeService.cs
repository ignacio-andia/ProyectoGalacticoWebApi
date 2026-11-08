using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface ICardsPersonajeService
    {
        Task<IEnumerable<CardsPersonajeDto>> ListarCardsAsync();
        Task<CardsPersonajeDto?> ObtenerCardPorIdAsync(int id);
        Task<CardsPersonajeDto?> ObtenerCardDePersonajeAsync(int id);
        Task<ResultadoOperacion<CardsPersonajeDto>> CrearCardAsync(CrearCardsPersonajeDto dto);
        Task<ResultadoOperacion<bool>> ActualizarCardAsync(int id, ActualizarCardsPersonajeDto dto);
        Task<ResultadoOperacion<bool>> EliminarCardAsync(int id);

    }
}
