namespace ProyectoGalacticoWebApi.Results
{
    public enum TipoResultado
    {
        Success,
        NotFound,
        Conflict,
        BadRequest
    }

    public class ResultadoOperacion<T>
    {
        public TipoResultado Tipo { get; set; }
        public T? Datos { get; set; }
        public string? Mensaje { get; set; }

        public static ResultadoOperacion<T> Exito(T datos)
        {
            return new ResultadoOperacion<T>
            {
                Tipo = TipoResultado.Success,
                Datos = datos
            };
        }

        public static ResultadoOperacion<T> Error(
            TipoResultado tipo, string mensaje)
        {
            return new ResultadoOperacion<T>
            {
                Tipo = tipo,
                Mensaje = mensaje
            };
        }
    }
}