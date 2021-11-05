using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class MaquinaConfig : IEntityTypeConfiguration<Maquina>
    {
        public void Configure(EntityTypeBuilder<Maquina> builder)
        {
            builder.ToTable("Maquinas", "Parametricas");
            builder.Property(e => e.MaquinaId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .HasMaxLength(200).IsRequired();

            builder.Property(e => e.MAC)
                .HasMaxLength(50).IsRequired();

            builder.Property(e => e.DireccionIP)
                .HasMaxLength(50).IsRequired();

            builder.Property(e => e.TipoMaquina)
                .IsRequired();

            builder.HasOne(e => e.Notaria)
             .WithMany()
              .HasForeignKey(e => e.NotariaId)
             .HasConstraintName("FK_Maquinas_Notarias")
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
