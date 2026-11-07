using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Services.Interfaces;
namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonajeController : ControllerBase
    {
        private readonly IPersonajeService _personajeService;
        public PersonajeController(IPersonajeService personajeService)
        {
            _personajeService = personajeService;
        }

        [HttpGet]
        public async Task<ActionResult> ListarPersonajes()
        {
            var personajes = await _personajeService.ListarPersonajesAsync();
            return Ok(personajes);
        }
    }
}
