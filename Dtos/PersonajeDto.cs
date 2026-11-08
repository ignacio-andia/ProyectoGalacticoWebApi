using ProyectoGalacticoWebApi.Models;
namespace ProyectoGalacticoWebApi.Dtos
{
    public class PersonajeDto
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Faccion { get; set; } = string.Empty;
        public string Afiliacion { get; set; } = string.Empty;
        public string? Estado { get; set; }
        public bool FuerzaSensitiva { get; set; }
        public string? ImagenUrl { get; set; } = string.Empty;
        public int Monedas { get; set; }
    }
}
