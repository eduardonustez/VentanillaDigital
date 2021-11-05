using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class UsuarioTokenPortalAdministradorConfig : IEntityTypeConfiguration<UsuarioTokenPortalAdministrador>
    {
        public void Configure(EntityTypeBuilder<UsuarioTokenPortalAdministrador> builder)
        {
            builder.ToTable("UsuarioTokenPortalAdministrador", "Parametricas");
            builder.Property(u => u.UsuarioTokenPortalAdministradorId);

            builder.Property(u => u.UsuarioAdministracionId)
                .IsRequired();

            builder.Property(u => u.LoginProvider)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(u => u.Token)
                .IsRequired(true)
                .HasMaxLength(3000);

            builder.Property(u => u.Nombre)
                .IsRequired(true)
                .HasMaxLength(40);

            builder.Property(u => u.FechaExpiracion)
                .IsRequired(true);

            builder.HasIndex(x => new { x.UsuarioAdministracionId, x.LoginProvider, x.Nombre}).IsUnique(true);
        }
    }
}
