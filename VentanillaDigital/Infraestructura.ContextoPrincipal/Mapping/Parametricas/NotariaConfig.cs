using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class NotariaConfig : IEntityTypeConfiguration<Notaria>
    {
        public void Configure(EntityTypeBuilder<Notaria> builder)
        {
            builder.ToTable("Notarias", "Parametricas");
            builder.Property(e => e.NotariaId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(60);
                       

            builder.Property(e => e.Direccion)
                .HasMaxLength(250);

            builder.Property(e => e.Telefono)
                .HasMaxLength(20);

            builder.Property(e => e.NumeroNotariaEnLetras)
                .HasMaxLength(150);
            
            builder.Property(e => e.CirculoNotaria)
                .HasMaxLength(150);

            builder.HasOne(e => e.Ubicacion)
              .WithMany()
               .HasForeignKey(e => e.UbicacionId)
              .HasConstraintName("FK_Notarias_Ubicaciones")
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.SelloArchivo)
            .WithOne()
            .HasForeignKey<Notaria>(d => d.SelloId)
            .HasConstraintName("FK_Notarias_SellosNotarias")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
