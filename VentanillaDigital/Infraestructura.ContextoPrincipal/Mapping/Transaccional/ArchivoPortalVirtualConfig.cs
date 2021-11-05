using Dominio.ContextoPrincipal.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Transaccional
{
    public class ArchivoPortalVirtualConfig : IEntityTypeConfiguration<ArchivosPortalVirtual>
    {
        public void Configure(EntityTypeBuilder<ArchivosPortalVirtual> builder)
        {
            builder.ToTable("ArchivosPortalVirtual", "Transaccional");
            builder.Property(e => e.ArchivosPortalVirtualId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Formato)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Base64)
                .IsRequired();

            builder.HasOne(e => e.TramitesPortalVirtual)
                .WithMany()
                .HasForeignKey(d => d.TramitesPortalVirtualId)
                .HasConstraintName("FK_ArchivosPortalVirtual_TramitesPortalVirtual")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoArchivoTramiteVirtual)
                .WithMany()
                .HasForeignKey(d => d.TipoArchivo)
                .HasConstraintName("FK_ArchivosPortalVirtual_TipoArchivoTramiteVirtual")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
