using System.ComponentModel.DataAnnotations;
using ProyectoGalacticoWebApi.Models;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class CambiarEstadoPersonajeDto
    {
        [Required]
        public EstadoPersonaje Estado { get; set; }
    }
}