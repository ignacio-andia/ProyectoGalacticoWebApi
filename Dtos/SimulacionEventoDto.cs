namespace ProyectoGalacticoWebApi.Dtos
{
    public class SimulacionEventoDto
    {
        public int EventoId { get; set; }
        public string NombreEvento { get; set; } = string.Empty;

        public double FuerzaBaseRebeldes { get; set; }
        public double FuerzaBaseImperio { get; set; }

        public double FactorRebelde { get; set; }
        public double FactorImperio { get; set; }

        public double FuerzaFinalRebeldes { get; set; }
        public double FuerzaFinalImperio { get; set; }

        public string Resultado { get; set; } = string.Empty;
        public string Criterio { get; set; } = string.Empty;
    }
}
