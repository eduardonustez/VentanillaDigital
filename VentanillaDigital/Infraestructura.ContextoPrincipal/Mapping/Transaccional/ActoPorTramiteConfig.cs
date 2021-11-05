using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class ActoPorTramiteConfig : IEntityTypeConfiguration<ActoPorTramite>
    {
        public void Configure(EntityTypeBuilder<ActoPorTramite> builder)
        {
            builder.ToTable("ActosPorTramite", "Transaccional");
            builder.HasIndex(m => m.ActoPorTramiteId);
            builder.Property(m => m.Cuandi)
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            builder.HasOne(m => m.TramitesPortalVirtual)
                .WithMany(m => m.ActosPorTramite)
                .HasForeignKey(m => m.TramitePortalVirtualId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.ActoNotarial)
                .WithMany(m => m.ActosPorTramite)
                .HasForeignKey(m => m.ActoNotarialId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
