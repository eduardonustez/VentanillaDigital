using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class TipoTramiteVirtualConfig : IEntityTypeConfiguration<TipoTramiteVirtual>
    {
        public void Configure(EntityTypeBuilder<TipoTramiteVirtual> builder)
        {
            builder.ToTable("TipoTramiteVirtual", "Parametricas");
            builder.HasKey(e => e.TipoTramiteID);

            builder.Property(e => e.Nombre)
                .HasMaxLength(200).IsRequired();

            builder.Property(e => e.Descripcion)
                .HasMaxLength(250).IsRequired();
        }
    }
}
