using System;
using System.Collections.Generic;
using System.Text;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class NotariasUsuariosConfig:IEntityTypeConfiguration<NotariaUsuarios>
    {
        public void Configure(EntityTypeBuilder<NotariaUsuarios> builder)
        {
            builder.ToTable("NotariasUsuario", "Parametricas");
            builder.Property(e => e.NotariaUsuariosId).ValueGeneratedOnAdd();

            builder.Property(e => e.NotariaId).IsRequired();

            builder.Property(e => e.UsuariosId).IsRequired();

            builder.Property(e => e.UserEmail).IsRequired().HasMaxLength(60);
            builder.Property(e => e.Celular).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Area).IsRequired().HasMaxLength(60);
            builder.Property(e => e.Cargo).IsRequired().HasMaxLength(60);

            builder.HasOne(e => e.Notaria)
              .WithMany(e => e.NotariasUsuarios)
               .HasForeignKey(e => e.NotariaId)
              .HasConstraintName("FK_NotariasUsuarios_Notarias")
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Persona)
              .WithMany()
               .HasForeignKey(e => e.PersonaId)
              .HasConstraintName("FK_NotariasUsuarios_Personas")
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(nu => nu.UserEmail);
            builder.HasIndex(nu => new { nu.IsDeleted, nu.UserEmail } );
            builder.HasIndex(nu => nu.IsDeleted)
                .IncludeProperties(nu => nu.UsuariosId);
        }
    }
}
