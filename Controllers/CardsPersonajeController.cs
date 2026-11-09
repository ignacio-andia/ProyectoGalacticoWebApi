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
        [EndpointSummary("Listar todas las cards de personajes")]
        [EndpointDescription("""
        Obtiene la lista completa de cards registradas en el catalogo galactico.

        FUNCIONAMIENTO:
        - Consulta las cards almacenadas en la base de datos.
        - Devuelve la informacion de las cards encontradas.
        - Cada card contiene informacion sobre el poder, habilidad especial,
          arma y nivel de peligrosidad del personaje.

        RESPUESTAS:
        - 200 OK: lista de cards obtenida correctamente.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarCards()
        {
            var cards = await _cardsService.ListarCardsAsync();

            return Ok(cards);
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("Obtener una card por ID")]
        [EndpointDescription("""
        Busca y devuelve una card mediante su identificador.

        PARAMETROS:
        - id: identificador unico de la card.

        FUNCIONAMIENTO:
        - Busca la card en la base de datos.
        - Si existe, devuelve toda su informacion.
        - Si no existe, devuelve un error indicando que no fue encontrada.

        RESPUESTAS:
        - 200 OK: card encontrada correctamente.
        - 404 Not Found: no existe una card con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Obtener la card de un personaje")]
        [EndpointDescription("""
        Busca la card asociada a un personaje mediante su identificador.

        PARAMETROS:
        - personajeId: identificador del personaje cuya card se desea consultar.

        FUNCIONAMIENTO:
        - Busca la card relacionada con el personaje indicado.
        - Si existe, devuelve la informacion de su card.
        - Si no existe una card asociada, devuelve un error.

        RESPUESTAS:
        - 200 OK: card del personaje encontrada correctamente.
        - 404 Not Found: no se encontro una card asociada al personaje.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Crear una nueva card de personaje")]
        [EndpointDescription("""
        Registra una nueva card y la asocia con un personaje existente.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los datos definidos en CrearCardsPersonajeDto.
        - Incluye la informacion de poder, habilidad especial, arma,
          nivel de peligrosidad e identificador del personaje.

        FUNCIONAMIENTO:
        - Recibe los datos de la nueva card.
        - Delega las validaciones y la creacion al servicio.
        - Si la operacion es exitosa, guarda la card en la base de datos.
        - Devuelve la card creada y su ubicacion para consultarla.

        RESPUESTAS:
        - 201 Created: card creada correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 404 Not Found: no se encontro el personaje relacionado.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Actualizar una card de personaje")]
        [EndpointDescription("""
        Actualiza la informacion de una card existente.

        PARAMETROS:
        - id: identificador de la card que se desea modificar.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los nuevos datos definidos en ActualizarCardsPersonajeDto.

        FUNCIONAMIENTO:
        - Busca la card mediante su identificador.
        - Delega las validaciones y la actualizacion al servicio.
        - Si la operacion es exitosa, guarda los cambios en la base de datos.

        RESPUESTAS:
        - 204 No Content: card actualizada correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 404 Not Found: no se encontro la card o el recurso relacionado.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ActualizarCard(int id, ActualizarCardsPersonajeDto dto)
        {
            var resultado = await _cardsService.ActualizarCardAsync(id, dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [EndpointSummary("Eliminar una card de personaje")]
        [EndpointDescription("""
        Elimina una card del catalogo mediante su identificador.

        PARAMETROS:
        - id: identificador de la card que se desea eliminar.

        FUNCIONAMIENTO:
        - Busca la card en la base de datos.
        - Delega la eliminacion al servicio.
        - Si la operacion es exitosa, elimina el registro.

        RESPUESTAS:
        - 204 No Content: card eliminada correctamente.
        - 404 Not Found: no se encontro la card solicitada.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> EliminarCard(int id)
        {
            var resultado = await _cardsService.EliminarCardAsync(id);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }
            return NoContent();
        }
    }
}