using ProyectoGalacticoWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class ActualizarPersonajeDto
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Especie { get; set; } = string.Empty;
        public Faccion Faccion { get; set; }
        [Required]
        [StringLength(70)]
        public string Afiliacion { get; set; } = string.Empty;
        public bool FuerzaSensitiva { get; set; }
        [StringLength(300)]
        public string? ImagenUrl { get; set; } = string.Empty;
    }
}
