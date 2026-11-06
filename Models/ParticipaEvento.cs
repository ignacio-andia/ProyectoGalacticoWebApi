namespace ProyectoGalacticoWebApi.Models
{
    public class ParticipaEvento
    {
        public int IdPersonaje { get; set; }
        public int IdEvento { get; set; }

        public Personaje Personaje { get; set; } = null!; // null! porque se va a inicializar en la base de datos 
        public Evento Evento { get; set; } = null!;
    }
}
