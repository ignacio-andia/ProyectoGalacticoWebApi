using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Models;

namespace ProyectoGalacticoWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tablas principales
        public DbSet<Personaje> Personajes => Set<Personaje>();
        public DbSet<CardsPersonaje> CardsPersonajes => Set<CardsPersonaje>();
        public DbSet<Evento> Eventos => Set<Evento>();

        // Tabla intermedia
        public DbSet<ParticipaEvento> ParticipaEventos => Set<ParticipaEvento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación 1:1 entre Personaje y CardsPersonaje
            modelBuilder.Entity<Personaje>()
                .HasOne(p => p.CardsPersonaje)
                .WithOne(c => c.Personaje)
                .HasForeignKey<CardsPersonaje>(c => c.PersonajeId);

            // Relación N:N entre Personaje y Evento
            modelBuilder.Entity<ParticipaEvento>()
                .HasKey(pe => new { pe.IdPersonaje, pe.IdEvento });

            modelBuilder.Entity<ParticipaEvento>()
                .HasOne(pe => pe.Personaje)
                .WithMany(p => p.Eventos)
                .HasForeignKey(pe => pe.IdPersonaje);

            modelBuilder.Entity<ParticipaEvento>()
                .HasOne(pe => pe.Evento)
                .WithMany(e => e.Participantes)
                .HasForeignKey(pe => pe.IdEvento);

            // Configuración de propiedades de Personaje
            modelBuilder.Entity<Personaje>(e =>
            {
                e.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(p => p.Especie)
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(p => p.Faccion)
                    .IsRequired();

                e.Property(p => p.Afiliacion)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Configuración de CardsPersonaje
            modelBuilder.Entity<CardsPersonaje>(e =>
            {
                e.Property(c => c.Poder).IsRequired();

                e.Property(c => c.HabilidadEspecial)
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(c => c.Arma)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(c => c.NivelDePeligrosidad).IsRequired();
            });

            // Configuración de Evento
            modelBuilder.Entity<Evento>(e =>
            {
                e.Property(ev => ev.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(ev => ev.Fecha).IsRequired();

                e.Property(ev => ev.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(ev => ev.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                e.Property(ev => ev.Resultado)
                    .HasMaxLength(200);
            });
        }
    }
}