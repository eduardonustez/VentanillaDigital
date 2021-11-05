using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class DocumentoPendienteAutorizarConfig : IEntityTypeConfiguration<DocumentoPendienteAutorizar>
    {
        public void Configure(EntityTypeBuilder<DocumentoPendienteAutorizar> builder)
        {
            builder.ToTable("DocumentosPendienteAutorizar", "Transaccional");
            builder.Property(e => e.DocumentoPendienteAutorizarId)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Estado)
                .HasDefaultValueSql("1")
                .IsRequired();

            builder.Property(m => m.Intentos)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(m => m.MachineName)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(m => m.Seguimiento)
                .HasMaxLength(500);

            builder.HasOne(m => m.Tramite)
                .WithMany()
                .HasForeignKey(m => m.TramiteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.Notaria)
                .WithMany()
                .HasForeignKey(m => m.NotariaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
