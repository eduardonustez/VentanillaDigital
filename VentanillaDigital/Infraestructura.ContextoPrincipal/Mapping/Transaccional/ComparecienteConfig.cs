using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class ComparecienteConfig : IEntityTypeConfiguration<Compareciente>
    {
        public void Configure(EntityTypeBuilder<Compareciente> builder)
        {
            builder.ToTable("Comparecientes", "Transaccional");
            builder.Property(e => e.ComparecienteId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.PeticionRNEC)
                .IsRequired()
                .HasMaxLength(36)
                .HasDefaultValue(string.Empty);

            builder.Property(e => e.NombreDigitado)
                .HasMaxLength(100);
            /*
            builder.HasOne(e => e.Tramite)
            .WithMany(c => c.Comparecientes)
             .HasForeignKey(e => e.TramiteId)
            .HasConstraintName("FK_Comparecientes_Tramites")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);*/
            builder.HasIndex(c => c.TramiteId);

            builder.HasOne(e => e.Foto)
            .WithOne()
             .HasForeignKey<Compareciente>(d => d.FotoId)
            .HasConstraintName("FK_Fotos_Comparecientes")
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Firma)
                .WithOne()
                .HasForeignKey<Compareciente>(d => d.FirmaId)
                .HasConstraintName("FK_Firmas_Comparecientes")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ImagenDocumento)
                .WithOne()
                .HasForeignKey<Compareciente>(d => d.ImagenDocumentoId)
                .HasConstraintName("FK_ImagenesDocumento_Comparecientes")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Persona)
                .WithMany()
                .HasForeignKey(d => d.PersonaId)
                .HasConstraintName("FK_Personas_Comparecientes")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.Vigencia)
                .HasMaxLength(100);
        }
    }
}
