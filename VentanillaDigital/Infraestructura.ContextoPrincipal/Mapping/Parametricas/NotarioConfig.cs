using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class NotarioConfig : IEntityTypeConfiguration<Notario>
    {
        public void Configure(EntityTypeBuilder<Notario> builder)
        {
            builder.ToTable("Notarios","Parametricas");

            builder.Property(e => e.NotarioId)
                .ValueGeneratedOnAdd();


            builder.Property(e => e.Nit)
            .HasMaxLength(20)
              .IsRequired();

            builder.Property(e => e.Pin)
            .HasMaxLength(6);

            builder.Property(e => e.TipoNotario).IsRequired();


            builder.HasOne(d => d.NotariaUsuarios)
                .WithOne(e=>e.Notario)
                .HasForeignKey<Notario>(d => d.NotariaUsuariosId)
                .HasConstraintName("FK_Notarios_NotariasUsuarios")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.GrafoArchivo)
            .WithOne()
            .HasForeignKey<Notario>(d => d.GrafoId)
            .HasConstraintName("FK_Notarios_GrafosNotarios")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
