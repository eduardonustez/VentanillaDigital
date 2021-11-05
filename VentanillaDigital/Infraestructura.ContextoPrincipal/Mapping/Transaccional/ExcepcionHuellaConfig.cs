using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class ExcepcionHuellaConfig : IEntityTypeConfiguration<ExcepcionHuella>
    {
        public void Configure(EntityTypeBuilder<ExcepcionHuella> builder)
        {
            builder.ToTable("ExcepcionesHuellas", "Transaccional");
            builder.Property(e => e.ExcepcionHuellaId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.DedosExceptuados).IsRequired();
            builder.Property(e => e.Descripcion).IsRequired();
            
            builder.HasOne(e => e.Compareciente)
            .WithOne(c => c.ExcepcionHuella)
              .HasForeignKey<ExcepcionHuella>(e => e.ComparecienteId)
            .HasConstraintName("FK_ExcepcionesHuellas_Comparecientes")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
