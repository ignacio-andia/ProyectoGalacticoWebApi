using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class CrearCardsPersonajeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Poder { get; set; }
        [Required]
        [StringLength(100)]
        public string HabilidadEspecial { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Arma { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int NivelDePeligrosidad { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int PersonajeId { get; set; }
    }
}
