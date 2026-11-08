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
        public int Monedas { get; set; }



        //Relaciones
        public ICollection<ParticipaEvento> Eventos { get; set; } = new List<ParticipaEvento>(); // para poder hacer la relacion de muchos a muchos entre Personaje y Evento, se necesita una tabla intermedia que es ParticipaEvento
        public CardsPersonaje CardsPersonaje { get; set; } = null!; // propieda de navegacion para la relacion con CardsPersonaje
        public ICollection<ParticipaMision> Misiones { get; set; } = new List<ParticipaMision>();
    }
}
