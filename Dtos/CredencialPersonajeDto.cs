namespace ProyectoGalacticoWebApi.Dtos
{
    public class CredencialPersonajeDto
    {
        public int Id { get; set; }
        public int PersonajeId { get; set; }
        public string CanalDeComunicacion { get; set; } = string.Empty;
    }
}
