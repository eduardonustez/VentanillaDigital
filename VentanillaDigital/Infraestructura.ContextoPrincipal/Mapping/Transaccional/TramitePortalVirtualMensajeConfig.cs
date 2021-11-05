using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class TramitePortalVirtualMensajeConfig : IEntityTypeConfiguration<TramitePortalVirtualMensaje>
    {
        public void Configure(EntityTypeBuilder<TramitePortalVirtualMensaje> builder)
        {
            builder.ToTable("TramitePortalVirtualMensajes", "Transaccional");
            builder.Property(e => e.TramitePortalVirtualMensajeId)
                .ValueGeneratedOnAdd();
            builder.Property(m => m.Mensaje)
                .HasMaxLength(512)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.UsuarioCreacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();

            builder.HasOne(m => m.TramitesPortalVirtual)
                .WithMany(m => m.TramitePortalVirtualMensajes)
                .HasForeignKey(m => m.TramitePortalVirtualId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
