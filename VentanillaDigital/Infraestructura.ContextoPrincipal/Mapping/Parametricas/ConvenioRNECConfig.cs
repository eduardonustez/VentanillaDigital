using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class ConvenioRNECConfig : IEntityTypeConfiguration<ConvenioRNEC>
    {
        public void Configure(EntityTypeBuilder<ConvenioRNEC> builder)
        {
            builder.ToTable("ConveniosRNEC", "Parametricas");
            builder.Property(e => e.ConvenioRNECId)
                .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Notaria)
             .WithMany()
              .HasForeignKey(e => e.NotariaId)
             .HasConstraintName("FK_ConveniosRNEC_Notarias")
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
