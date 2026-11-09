using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MisionController : ControllerBase
    {
        private readonly IMisionService _misionService;

        public MisionController(IMisionService misionService)
        {
            _misionService = misionService;
        }

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

        [HttpGet]
        [EndpointSummary("Listar todas las misiones")]
        [EndpointDescription("""
        Obtiene la lista completa de misiones registradas en el catalogo galactico.

        FUNCIONAMIENTO:
        - Consulta las misiones almacenadas en la base de datos.
        - Devuelve la informacion de cada mision.
        - Incluye su dificultad, poder minimo, recompensa y estado actual.
        - Permite consultar misiones disponibles, finalizadas o canceladas.

        RESPUESTAS:
        - 200 OK: lista de misiones obtenida correctamente.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarMisiones()
        {
            var misiones = await _misionService.ListarMisionesAsync();
            return Ok(misiones);
        }

        [HttpPost]
        [EndpointSummary("Crear una nueva mision")]
        [EndpointDescription("""
        Registra una nueva mision en el catalogo galactico.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los datos definidos en CrearMisionDto.
        - Incluye el nombre, tipo, nivel de dificultad y descripcion.
        - Tambien establece el poder minimo requerido y la recompensa en monedas.

        FUNCIONAMIENTO:
        - Recibe los datos de la nueva mision.
        - Delega las validaciones y la creacion al servicio.
        - Si la operacion es exitosa, registra la mision en la base de datos.
        - La mision queda disponible para recibir participantes.

        RESPUESTAS:
        - 201 Created: mision creada correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CrearMision(CrearMisionDto dto)
        {
            var resultado = await _misionService.CrearMisionAsync(dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return StatusCode(201, resultado.Datos);
        }

        [HttpPost("{id:int}/participantes/{personajeId:int}")]
        [EndpointSummary("Agregar un personaje a una mision")]
        [EndpointDescription("""
        Registra un personaje como participante de una mision.

        PARAMETROS:
        - id: identificador de la mision.
        - personajeId: identificador del personaje que se desea agregar.

        REQUISITOS:
        - La mision debe existir y estar disponible.
        - El personaje debe existir y encontrarse vivo.
        - El personaje debe tener una card registrada.
        - El personaje no debe estar registrado previamente en la mision.
        - El personaje no debe participar en otra mision disponible.
        - Todos los participantes de la mision deben pertenecer a la misma faccion.

        FUNCIONAMIENTO:
        - Recibe los identificadores de la mision y del personaje.
        - Verifica las condiciones necesarias para registrar la participacion.
        - Si las validaciones son correctas, registra al personaje en la mision.

        RESPUESTAS:
        - 204 No Content: participante agregado correctamente.
        - 400 Bad Request: no se cumplen los requisitos para participar.
        - 404 Not Found: no se encontro la mision o el personaje.
        - 409 Conflict: el personaje ya participa en la mision o en otra mision disponible.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AgregarParticipante(int id, int personajeId)
        {
            var resultado = await _misionService.AgregarParticipanteAsync(id, personajeId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }

        [HttpPost("{id:int}/iniciar")]
        [EndpointSummary("Iniciar y ejecutar una mision")]
        [EndpointDescription("""
        Ejecuta una mision y determina su resultado a partir del poder total de los participantes y el nivel de dificultad.

        PARAMETROS:
        - id: identificador de la mision que se desea ejecutar.

        REQUISITOS:
        - La mision debe existir y estar disponible.
        - Debe tener al menos un participante registrado.
        - Todos los participantes deben tener una card.
        - La suma del poder de los participantes debe alcanzar el poder minimo requerido.

        FUNCIONAMIENTO:
        - Calcula el poder total de los participantes.
        - Calcula la probabilidad de exito considerando el poder total y la dificultad.
        - Limita la probabilidad de exito a un minimo del 5% y un maximo del 95%.
        - Determina aleatoriamente si la mision termina con exito o fracaso.
        - Si la mision es exitosa, cada participante puede terminar vivo o herido.
        - Si la mision fracasa, todos los participantes mueren.
        - Distribuye la recompensa entre los participantes vivos o heridos.
        - Los participantes muertos no reciben monedas.
        - Registra el resultado de la mision y los cambios en la base de datos.

        RESPUESTAS:
        - 200 OK: mision ejecutada correctamente. Devuelve el resultado y los datos de los participantes.
        - 400 Bad Request: no se cumplen los requisitos para ejecutar la mision.
        - 404 Not Found: no se encontro la mision.
        - 409 Conflict: la mision no esta disponible para ejecutarse.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> IniciarMision(int id)
        {
            var resultado = await _misionService.IniciarMisionAsync(id);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return Ok(resultado.Datos);
        }
    }
}