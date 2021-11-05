using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class UbicacionConfig : IEntityTypeConfiguration<Ubicacion>
    {
        public void Configure(EntityTypeBuilder<Ubicacion> builder)
        {
            builder.ToTable("Ubicaciones", "Parametricas");
            builder.Property(e => e.UbicacionId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired();

            builder.HasOne(e => e.UbicacionPadre)
                .WithMany(e => e.UbicacionesHijo)
                .HasForeignKey(e => e.UbicacionPadreId)
                .HasConstraintName("FK_Ubicacion_UbicacionPadre")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
