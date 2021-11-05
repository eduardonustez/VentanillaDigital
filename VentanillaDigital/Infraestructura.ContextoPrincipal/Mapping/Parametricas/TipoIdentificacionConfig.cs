using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class TipoIdentificacionConfig : IEntityTypeConfiguration<TipoIdentificacion>
    {
        public void Configure(EntityTypeBuilder<TipoIdentificacion> builder)
        {
            builder.ToTable("TiposIdentificacion", "Parametricas");
            builder.HasKey(t => t.TipoIdentificacionId);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60);
            
            builder.Property(e => e.Abreviatura)
                .HasMaxLength(3);

        }
    }
}
