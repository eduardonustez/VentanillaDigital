using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class MetadataArchivoConfig : IEntityTypeConfiguration<MetadataArchivo>
    {
        public void Configure(EntityTypeBuilder<MetadataArchivo> builder)
        {
            builder.ToTable("MetadataArchivos", "Transaccional");
            builder.Property(e => e.MetadataArchivoId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Ruta)
            .HasMaxLength(250);

        }
    }
}
