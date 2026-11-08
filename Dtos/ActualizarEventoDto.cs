using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class ActualizarEventoDto
    {
        [Required]
        [StringLength(70)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int Fecha { get; set; }
        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;
    }
}
