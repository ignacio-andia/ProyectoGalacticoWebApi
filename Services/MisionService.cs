using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Services
{
    public class MisionService : IMisionService
    {
        private readonly AppDbContext _context;

        public MisionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoOperacion<MisionDto>> CrearMisionAsync(CrearMisionDto dto)
        {
            var mision = new Mision
            {
                Nombre = dto.Nombre,
                Tipo = dto.Tipo,
                NivelDeDificultad = dto.NivelDeDificultad,
                Descripcion = dto.Descripcion,
                PoderMinimo = dto.PoderMinimo,
                Recompensa = dto.Recompensa,
                Estado = EstadoMision.Disponible
            };

            _context.Misiones.Add(mision);
            await _context.SaveChangesAsync();

            var misionDto = new MisionDto
            {
                Id = mision.Id,
                Nombre = mision.Nombre,
                Tipo = mision.Tipo,
                NivelDeDificultad = mision.NivelDeDificultad,
                Descripcion = mision.Descripcion,
                PoderMinimo = mision.PoderMinimo,
                Recompensa = mision.Recompensa,
                Estado = mision.Estado.ToString(),
                Resultado = mision.Resultado?.ToString()
            };

            return ResultadoOperacion<MisionDto>.Exito(misionDto);
        }


        public async Task<IEnumerable<MisionDto>> ListarMisionesAsync()
        {
            var misiones = await _context.Misiones.AsNoTracking().ToListAsync();

            return misiones.Select(m => new MisionDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Tipo = m.Tipo,
                NivelDeDificultad = m.NivelDeDificultad,
                Descripcion = m.Descripcion,
                PoderMinimo = m.PoderMinimo,
                Recompensa = m.Recompensa,
                Estado = m.Estado.ToString(),
                Resultado = m.Resultado?.ToString()
            });
        }
    }
}
