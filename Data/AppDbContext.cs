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
        public DbSet<Mision> Misiones => Set<Mision>();

        // Tablas intermedias
        public DbSet<ParticipaEvento> ParticipaEventos => Set<ParticipaEvento>();
        public DbSet<ParticipaMision> ParticipaMisiones => Set<ParticipaMision>();

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

                e.Property(p => p.Monedas)
                    .IsRequired()
                    .HasDefaultValue(0);
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
                    .IsRequired(false)
                    .HasMaxLength(200);
            });


            // Relación N:N entre Personaje y Mision
            modelBuilder.Entity<ParticipaMision>()
                .HasKey(pm => new { pm.IdPersonaje, pm.IdMision });

            modelBuilder.Entity<ParticipaMision>()
                .HasOne(pm => pm.Personaje)
                .WithMany(p => p.Misiones)
                .HasForeignKey(pm => pm.IdPersonaje);

            modelBuilder.Entity<ParticipaMision>()
                .HasOne(pm => pm.Mision)
                .WithMany(m => m.Participantes)
                .HasForeignKey(pm => pm.IdMision);


            // Configuración de Mision
            modelBuilder.Entity<Mision>(e =>
            {
                e.Property(m => m.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(m => m.Tipo)
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(m => m.NivelDeDificultad).IsRequired();

                e.Property(m => m.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                e.Property(m => m.PoderMinimo).IsRequired();
                e.Property(m => m.Recompensa).IsRequired();
                e.Property(m => m.Estado).IsRequired();
                e.Property(m => m.Resultado).IsRequired(false);
            });

        }
    }
}