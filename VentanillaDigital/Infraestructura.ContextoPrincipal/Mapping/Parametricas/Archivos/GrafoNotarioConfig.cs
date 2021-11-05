using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class GrafoNotarioConfig : IEntityTypeConfiguration<GrafoNotario>
    {
        public void Configure(EntityTypeBuilder<GrafoNotario> builder)
        {
            builder.ToTable("GrafosNotarios", "Parametricas");
            builder.Property(e => e.GrafoNotarioId)
                .ValueGeneratedOnAdd();
        }
    }
}
