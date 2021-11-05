using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class PersonaConfig : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Personas", "Parametricas");

            builder.Property(e => e.PersonaId)
                .ValueGeneratedOnAdd();


            builder.Property(e => e.TipoIdentificacionId)
                .IsRequired();

            builder.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Apellidos)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(512);

            builder.Property(e => e.NumeroCelular)
                .HasMaxLength(20);

            builder.Property(e => e.Genero)
                .IsRequired(false)
                .HasMaxLength(1);

            builder.HasIndex(e => new { e.TipoIdentificacionId, e.NumeroDocumento })
                .IsUnique(true);

            builder.HasOne(d => d.TipoIdentificacion)
                .WithMany()
                .HasForeignKey(d => d.TipoIdentificacionId)
                .HasConstraintName("FK_TipoIdentificacion_Personas")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
