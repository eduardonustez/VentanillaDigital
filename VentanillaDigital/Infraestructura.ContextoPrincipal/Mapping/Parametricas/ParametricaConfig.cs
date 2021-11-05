using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class ParametricaConfig : IEntityTypeConfiguration<Parametrica>
    {
        public void Configure(EntityTypeBuilder<Parametrica> builder)
        {
            builder.ToTable("Parametricas", "Parametricas");
            builder.Property(e => e.ParametricaId)
            .ValueGeneratedOnAdd();

            builder.Property(e => e.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Valor)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.Longitud)
                .IsRequired();

            builder.Property(e => e.Expresion)
                .IsRequired();
        }
    }
}
