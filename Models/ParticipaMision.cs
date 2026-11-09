namespace ProyectoGalacticoWebApi.Models
{
    public class ParticipaMision
    {
        public int IdPersonaje { get; set; }
        public int IdMision { get; set; }

        public Personaje Personaje { get; set; } = null!;
        public Mision Mision { get; set; } = null!;
    }
}
