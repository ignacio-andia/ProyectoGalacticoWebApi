namespace ProyectoGalacticoWebApi.Services.Interfaces
{
    public interface ICifradoService
    {
        string Cifrar(string textoPlano);
        string Descifrar(string textoCifrado);
    }
}