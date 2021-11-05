using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class TipoDatoConfig : IEntityTypeConfiguration<TipoDato>
    {
        public void Configure(EntityTypeBuilder<TipoDato> builder)
        {
            builder.ToTable("TiposDatos", "Parametricas");
            builder.Property(e => e.TipoDatoId).HasConversion<int>();

            builder.HasData(Enum.GetValues(typeof(TipoDatoId))
                .Cast<TipoDatoId>()
                .Select(e => new TipoDato()
                {
                    TipoDatoId = e,
                    Nombre = e.ToString()
                })
                );

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60);
            

        }
    }
}
