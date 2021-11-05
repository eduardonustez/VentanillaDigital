using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias", "Parametricas");
            builder.Property(e => e.CategoriaId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(250);

        }
    }
}
