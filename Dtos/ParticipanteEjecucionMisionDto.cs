namespace ProyectoGalacticoWebApi.Dtos
{
    public class ParticipanteEjecucionMisionDto
    {
        public int PersonajeId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string EstadoFinal { get; set; } = string.Empty;
        public int MonedasRecibidas { get; set; }
    }
}
