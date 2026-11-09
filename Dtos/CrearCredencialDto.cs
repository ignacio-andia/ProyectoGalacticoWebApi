using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class CrearCredencialDto
    {
        [Required]
        [StringLength(100)]
        public string CodigoAcceso { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CanalDeComunicacion { get; set; } = string.Empty;
    }
}