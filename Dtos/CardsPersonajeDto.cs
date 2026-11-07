namespace ProyectoGalacticoWebApi.Dtos
{
    public class CardsPersonajeDto
    {
        public int Id { get; init; }
        public int Poder { get; set; }
        public string HabilidadEspecial { get; set; } = string.Empty;
        public string Arma { get; set; } = string.Empty; 
        public int NivelDePeligrosidad { get; set; }
        public int PersonajeId { get; set; } //clave Foranea
    }
}
