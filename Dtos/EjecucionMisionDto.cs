namespace ProyectoGalacticoWebApi.Dtos
{
    public class EjecucionMisionDto
    {
        public int MisionId { get; set; }
        public string NombreMision { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public int PoderTotal { get; set; }
        public int PoderMinimo { get; set; }
        public double ProbabilidadExito { get; set; }
        public int RecompensaTotal { get; set; }
        public int RecompensaDistribuida { get; set; }
        public List<ParticipanteEjecucionMisionDto> Participantes { get; set; } = new List<ParticipanteEjecucionMisionDto>();
    }
}
