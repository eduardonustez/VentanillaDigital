using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class ActoNotarialConfig : IEntityTypeConfiguration<ActoNotarial>
    {
        public void Configure(EntityTypeBuilder<ActoNotarial> builder)
        {
            builder.ToTable("ActosNotariales", "Parametricas");
            builder.HasKey(m => m.ActoNotarialId);
            builder.Property(m => m.Codigo)
                .HasMaxLength(16)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.Nombre)
                .HasMaxLength(512)
                .IsUnicode(false)
                .IsRequired();
            builder.HasOne(m => m.TipoTramiteVirtual)
                .WithMany(m => m.ActosNotariales)
                .HasForeignKey(m => m.TipoTramiteVirtualId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
