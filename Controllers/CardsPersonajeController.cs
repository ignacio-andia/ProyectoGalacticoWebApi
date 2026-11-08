using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Services.Interfaces;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsPersonajeController : ControllerBase
    {
        private IActionResult ManejarResultado<T>(ResultadoOperacion<T> resultado)
        {
            return resultado.Tipo switch
            {
                TipoResultado.NotFound => NotFound(resultado.Mensaje),
                TipoResultado.Conflict => Conflict(resultado.Mensaje),
                TipoResultado.BadRequest => BadRequest(resultado.Mensaje),
                _ => Ok(resultado.Datos)
            };
        }

        private readonly ICardsPersonajeService _cardsService;

        public CardsPersonajeController(ICardsPersonajeService cardsService)
        {
            _cardsService = cardsService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarCards()
        {
            var cards = await _cardsService.ListarCardsAsync();

            return Ok(cards);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerCardPorId(int id)
        {
            var card = await _cardsService.ObtenerCardPorIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpGet("personaje/{personajeId:int}")]
        public async Task<IActionResult> ObtenerCardDePersonaje(int personajeId)
        {
            var card = await _cardsService.ObtenerCardDePersonajeAsync(personajeId);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCard(CrearCardsPersonajeDto dto)
        {
            var resultado = await _cardsService.CrearCardAsync(dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return CreatedAtAction(
                nameof(ObtenerCardPorId),
                new { id = resultado.Datos!.Id },
                resultado.Datos);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCard(int id, ActualizarCardsPersonajeDto dto)
        {
            var resultado = await _cardsService.ActualizarCardAsync(id, dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }

    }
}
