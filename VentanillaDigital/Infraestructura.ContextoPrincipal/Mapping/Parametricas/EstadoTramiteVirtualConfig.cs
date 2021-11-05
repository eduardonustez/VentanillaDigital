using Dominio.ContextoPrincipal.Entidad;
using Dominio.Nucleo.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class EstadoTramiteVirtualConfig : IEntityTypeConfiguration<EstadoTramiteVirtual>
    {
        public void Configure(EntityTypeBuilder<EstadoTramiteVirtual> builder)
        {
            builder.ToTable("EstadoTramiteVirtual", "Parametricas");
            builder.HasKey(e => e.EstadoTramiteID);


            builder.Property(e => e.Nombre)
                .HasMaxLength(200).IsRequired();

            builder.Property(e => e.Descripcion)
                .HasMaxLength(250).IsRequired();
        }
    }
}
