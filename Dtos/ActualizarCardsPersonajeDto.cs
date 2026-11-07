using System.ComponentModel.DataAnnotations;

namespace ProyectoGalacticoWebApi.Dtos
{
    public class ActualizarCardsPersonajeDto
    {
        [Required]
        public int Poder { get; set; }
        [Required]
        [StringLength(100)]
        public string HabilidadEspecial { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Arma { get; set; } = string.Empty;
        [Required]
        public int NivelDePeligrosidad { get; set; }
        [Required]
        public int PersonajeId { get; set; }
    }
}
