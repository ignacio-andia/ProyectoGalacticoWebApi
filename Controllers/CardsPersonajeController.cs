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


    }
}
