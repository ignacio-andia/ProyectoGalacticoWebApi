namespace ProyectoGalacticoWebApi.Models
{
    public class CredencialPersonaje
    {
        public int Id { get; init; }
        public int PersonajeId { get; set; }
        public Personaje Personaje { get; set; } = null!;

        public string CodigoAccesoHash { get; set; } = string.Empty;
        public string CanalDeComunicacion { get; set; } = string.Empty;
    }
}
