namespace ProyectoGalacticoWebApi.Models
{
    public class Personaje
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public Faccion Faccion { get; set; }
        public string Afiliacion { get; set; } = string.Empty;
        public EstadoPersonaje? Estado { get; set; } // se añadio nuevo estado (Herido)
        public bool FuerzaSensitiva { get; set; }
        public string? ImagenUrl { get; set; } = string.Empty;

        
    }
}
