namespace ProyectoGalacticoWebApi.Models
{
    public class CardsPersonaje
    {
        public int Id { get; init; }
        public int Poder { get; set; }
        public string HabilidadEspecial { get; set; } = string.Empty;
        public string Arma { get; set; } = string.Empty; // si da tiempo se añadira un sistema de armas
        public int NivelDePeligrosidad { get; set; }
    }
}
