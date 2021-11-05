using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class PlantillaActaConfig : IEntityTypeConfiguration<PlantillaActa>
    {
        public void Configure(EntityTypeBuilder<PlantillaActa> builder)
        {
            builder.ToTable("PlantillasActas", "Parametricas");
            builder.Property(e => e.PlantillaActaId)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Nombre)
                .HasMaxLength(100);
            builder.Property(e => e.Contenido)
                .IsRequired();

        }
    }
}
