using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class EstadoTramiteConfig : IEntityTypeConfiguration<EstadoTramite>
    {
        public void Configure(EntityTypeBuilder<EstadoTramite> builder)
        {
            builder.ToTable("EstadosTramites", "Parametricas");
            builder.HasKey(e => e.EstadoTramiteId);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60);
            
            builder.Property(e => e.Descripcion)
                .HasMaxLength(250);

        }
    }
}
