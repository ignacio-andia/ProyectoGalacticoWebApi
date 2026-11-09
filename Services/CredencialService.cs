using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Services
{
    public class CredencialService : ICredencialService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<CredencialPersonaje> _hasher;
        private readonly ICifradoService _cifradoService;

        public CredencialService(AppDbContext context, IPasswordHasher<CredencialPersonaje> hasher, ICifradoService cifradoService)
        {
            _context = context;
            _hasher = hasher;
            _cifradoService = cifradoService;
        }

        public async Task<ResultadoOperacion<CredencialPersonajeDto>> CrearCredencialAsync(int personajeId, CrearCredencialDto dto)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == personajeId);

            if (personaje is null)
            {
                return ResultadoOperacion<CredencialPersonajeDto>.Error(TipoResultado.NotFound, "El personaje no existe");
            }

            var yaTieneCredencial = await _context.CredencialesPersonajes.AnyAsync(c => c.PersonajeId == personajeId);

            if (yaTieneCredencial)
            {
                return ResultadoOperacion<CredencialPersonajeDto>.Error(TipoResultado.Conflict, "El personaje ya tiene una credencial");
            }

            var credencial = new CredencialPersonaje
            {
                PersonajeId = personajeId,
                CanalDeComunicacion = _cifradoService.Cifrar(dto.CanalDeComunicacion)
            };

            credencial.CodigoAccesoHash = _hasher.HashPassword(credencial, dto.CodigoAcceso);

            _context.CredencialesPersonajes.Add(credencial);
            await _context.SaveChangesAsync();

            var credencialDto = new CredencialPersonajeDto
            {
                Id = credencial.Id,
                PersonajeId = credencial.PersonajeId,
                CanalDeComunicacion = dto.CanalDeComunicacion
            };

            return ResultadoOperacion<CredencialPersonajeDto>.Exito(credencialDto);
        }

        public async Task<ResultadoOperacion<CredencialPersonajeDto>> ObtenerCredencialAsync(int personajeId)
        {
            var credencial = await _context.CredencialesPersonajes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.PersonajeId == personajeId);

            if (credencial is null)
            {
                return ResultadoOperacion<CredencialPersonajeDto>.Error(TipoResultado.NotFound, "El personaje no tiene credenciales");
            }

            var credencialDto = new CredencialPersonajeDto
            {
                Id = credencial.Id,
                PersonajeId = credencial.PersonajeId,
                CanalDeComunicacion = _cifradoService.Descifrar(credencial.CanalDeComunicacion)
            };

            return ResultadoOperacion<CredencialPersonajeDto>.Exito(credencialDto);
        }
    }
}
