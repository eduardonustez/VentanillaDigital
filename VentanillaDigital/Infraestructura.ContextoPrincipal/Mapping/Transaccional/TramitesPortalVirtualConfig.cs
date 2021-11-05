using Dominio.ContextoPrincipal.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class TramitesPortalVirtualConfig : IEntityTypeConfiguration<TramitesPortalVirtual>
    {
        public void Configure(EntityTypeBuilder<TramitesPortalVirtual> builder)
        {
            builder.ToTable("TramitesPortalVirtual", "Transaccional");
            builder.Property(e => e.TramitesPortalVirtualId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.NumeroDocumento
            ).HasMaxLength(50).IsRequired();

            builder.Property(e => e.TipoDocumento
            ).IsRequired();

            builder.Property(e => e.NotariaId
            ).IsRequired();

            builder.Property(e => e.CUANDI
            ).HasMaxLength(40).IsRequired();


            builder.HasIndex(x => new { x.TramiteVirtualGuid, x.NotariaId }).IsUnique();

            builder.Property(e => e.DatosAdicionales
            ).IsRequired(false);

            builder.Property(e => e.DetalleCambioEstado
            ).HasMaxLength(255)
            .IsUnicode(false);

            builder.Property(e => e.TramiteVirtualGuid
            ).IsRequired(false)
            .HasMaxLength(255);

            //builder.HasOne(e => e.TipoTramiteVirtual)
            // .WithMany()
            //  .HasForeignKey(e => e.TipoTramiteVirtualId)
            // .HasConstraintName("FK_TramitesPortalVirtual_TipoTramiteVirtual")
            // .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ActoPrincipal)
            .WithMany(m => m.TramitesPortalVirtual)
             .HasForeignKey(e => e.ActoPrincipalId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.EstadoTramiteVirtual)
             .WithMany()
              .HasForeignKey(e => e.EstadoTramiteVirtualId)
             .HasConstraintName("FK_TramitesPortalVirtual_EstadoTramiteVirtual")
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
