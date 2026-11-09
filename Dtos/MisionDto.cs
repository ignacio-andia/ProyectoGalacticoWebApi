namespace ProyectoGalacticoWebApi.Dtos
{
    public class MisionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int NivelDeDificultad { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int PoderMinimo { get; set; }
        public int Recompensa { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Resultado { get; set; }
    }
}
