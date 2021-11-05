using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class CertificadoConfig : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.ToTable("Certificados", "Parametricas");
            builder.Property(e => e.CertificadoId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UsuarioId)
                .IsRequired()
                .HasMaxLength(60);

        }
    }
}
