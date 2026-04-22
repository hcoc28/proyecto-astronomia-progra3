using AstronomiaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AstronomiaApp.Data;

public class AstronomiaDbContext : DbContext
{
    public AstronomiaDbContext(DbContextOptions<AstronomiaDbContext> options) : base(options) { }

    public DbSet<TipoObjeto> TiposObjeto { get; set; }
    public DbSet<ObjetoAstronomico> ObjetosAstronomicos { get; set; }
    public DbSet<SistemaPlanetario> SistemasPlanetarios { get; set; }
    public DbSet<Constelacion> Constelaciones { get; set; }
    public DbSet<Relacion> Relaciones { get; set; }
    public DbSet<ConsultaLog> ConsultasLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ObjetoAstronomico -> multiples FK, desactivar cascade para evitar
        // error SQL Server "multiple cascade paths"
        modelBuilder.Entity<ObjetoAstronomico>()
            .HasOne(o => o.Tipo)
            .WithMany(t => t.Objetos)
            .HasForeignKey(o => o.TipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ObjetoAstronomico>()
            .HasOne(o => o.Sistema)
            .WithMany(s => s.Objetos)
            .HasForeignKey(o => o.SistemaId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ObjetoAstronomico>()
            .HasOne(o => o.Constelacion)
            .WithMany(c => c.Objetos)
            .HasForeignKey(o => o.ConstelacionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacion -> dos FK a ObjetoAstronomico, sin cascade
        modelBuilder.Entity<Relacion>()
            .HasOne(r => r.Origen)
            .WithMany(o => o.RelacionesOrigen)
            .HasForeignKey(r => r.OrigenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Relacion>()
            .HasOne(r => r.Destino)
            .WithMany(o => o.RelacionesDestino)
            .HasForeignKey(r => r.DestinoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indice para busqueda rapida por nombre
        modelBuilder.Entity<ObjetoAstronomico>()
            .HasIndex(o => o.Nombre)
            .IsUnique();
    }
}
