using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class PersonaDatosConfig : IEntityTypeConfiguration<PersonaDatos>
    {
        public void Configure(EntityTypeBuilder<PersonaDatos> builder)
        {
            builder.ToTable("PersonasDatos", "Parametricas");

            builder.Property(e => e.PersonaDatosId)
                .ValueGeneratedOnAdd();
                       

            builder.Property(e => e.TipoDatoId)
                .HasConversion<int>();

            builder.Property(e => e.ValorDato)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(d => d.Persona)
                .WithMany(e => e.PersonaDatos)
                .HasForeignKey(d => d.PersonaId)
                .HasConstraintName("FK_PersonaDatos_Personas")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
