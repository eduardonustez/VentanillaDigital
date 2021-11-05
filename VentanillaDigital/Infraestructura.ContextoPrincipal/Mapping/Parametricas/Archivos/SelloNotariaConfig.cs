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
    public class SelloNotariaConfig : IEntityTypeConfiguration<SelloNotaria>
    {
        public void Configure(EntityTypeBuilder<SelloNotaria> builder)
        {
            builder.ToTable("SellosNotarias", "Parametricas");
            builder.Property(e => e.SelloNotariaId)
                .ValueGeneratedOnAdd();
        }
    }
}
