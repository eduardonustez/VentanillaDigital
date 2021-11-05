using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class DocumentoPrevioConfig : IEntityTypeConfiguration<DocumentoPrevio>
    {
        public void Configure(EntityTypeBuilder<DocumentoPrevio> builder)
        {
            builder.ToTable("DocumentosPrevios", "Transaccional");
            builder.Property(e => e.DocumentoPrevioId)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Seguimiento)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.HasOne(m => m.Tramite)
                .WithOne(m => m.DocumentoPrevio)
                .HasForeignKey<DocumentoPrevio>(m => m.TramiteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
