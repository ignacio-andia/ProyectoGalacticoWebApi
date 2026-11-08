using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Services.Interfaces;
using ProyectoGalacticoWebApi.Results;
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

        [HttpGet("{Id}")]
        public async Task<ActionResult> ObtenerPersonajePorId(int Id)
        {
            var personaje = await _personajeService.ObtenerPersonajePorIdAsync(Id);
            if(personaje == null)
            {
                return NotFound();
            }
            return Ok(personaje);
        }

        [HttpPost]
        public async Task<ActionResult> CrearPersonaje(CrearPersonajeDto dto)
        {
            var personaje = await _personajeService.CrearPersonajeAsync(dto);
            return CreatedAtAction(nameof(ObtenerPersonajePorId), new { id = personaje.Id }, personaje); //Para el 221
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarPersonaje(int  id, ActualizarPersonajeDto dto)
        {
            var personaje = await _personajeService.ActualizarPersonajeAsync(id, dto);
            if (!personaje)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarPersonaje(int  id)
        {
            var personaje = await _personajeService.EliminarPersonajeAsync(id);
            if (!personaje)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoPersonajeDto dto)
        {
            var resultado = await _personajeService.CambiarEstadoAsync(id, dto);
            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
