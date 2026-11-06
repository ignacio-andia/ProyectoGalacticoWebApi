namespace ProyectoGalacticoWebApi.Models
{
    public class Evento
    {
        public int Id { get; init; }
        public string Nombre { get; set; } = string.Empty;
        public int Fecha { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
    }
}
