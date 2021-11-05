using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class ArchivoConfig : IEntityTypeConfiguration<Archivo>
    {
        public void Configure(EntityTypeBuilder<Archivo> builder)
        {
            builder.ToTable("Archivos", "Transaccional");
            builder.Property(e => e.ArchivoId)
                .ValueGeneratedOnAdd();

            
            builder.HasOne(e => e.MetadataArchivo)
            .WithOne(c => c.Archivo)
              .HasForeignKey<Archivo>(d => d.MetadataArchivoId)
            .HasConstraintName("FK_Archivos_MetadataArchivos")
            .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
