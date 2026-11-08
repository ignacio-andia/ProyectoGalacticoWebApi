namespace ProyectoGalacticoWebApi.Models
{
    public class Mision
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int NivelDeDificultad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int PoderMinimo { get; set; }
        public int Recompensa { get; set; }
        public EstadoMision Estado { get; set; }
        public ResultadoMision? Resultado { get; set; }

        //Relaciones
        public ICollection<ParticipaMision> Participantes { get; set; } = new List<ParticipaMision>();
    }
}