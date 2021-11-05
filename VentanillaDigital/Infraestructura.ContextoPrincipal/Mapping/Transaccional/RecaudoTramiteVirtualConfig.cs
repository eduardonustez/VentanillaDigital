using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class RecaudoTramiteVirtualConfig : IEntityTypeConfiguration<RecaudoTramiteVirtual>
    {
        public void Configure(EntityTypeBuilder<RecaudoTramiteVirtual> builder)
        {
            builder.ToTable("RecaudosTramiteVirtual", "Transaccional");
            builder.Property(e => e.RecaudoTramiteVirtualId)
                .ValueGeneratedOnAdd();
            builder.Property(m => m.NombreCompleto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.Correo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.CUS)
                .HasMaxLength(255)
                .IsUnicode(false);
            builder.Property(m => m.NumeroIdenficacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.UsuarioCreacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.Observacion)
                .HasMaxLength(512)
                .IsUnicode(false)
                .IsRequired();
            builder.Property(m => m.ValorTotal)
               .HasColumnType("decimal(18,0)")
               .IsRequired();
            builder.Property(m => m.ValorPagado)
               .HasColumnType("decimal(18,0)");
            builder.Property(m => m.IVA)
               .HasColumnType("decimal(18,0)");

            builder.HasOne(m => m.TramitesPortalVirtual)
                .WithMany(m => m.RecaudosTramiteVirtual)
                .HasForeignKey(m => m.TramitePortalVirtualId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
