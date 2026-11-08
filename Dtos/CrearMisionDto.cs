using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class CrearMisionDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [Range(1, 10)]
        public int NivelDeDificultad { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int PoderMinimo { get; set; }

        [Range(1, int.MaxValue)]
        public int Recompensa { get; set; }
    }
}