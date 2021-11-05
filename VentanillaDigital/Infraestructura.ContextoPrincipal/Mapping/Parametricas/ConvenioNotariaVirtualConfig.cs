using Dominio.ContextoPrincipal.Entidad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class ConvenioNotariaVirtualConfig : IEntityTypeConfiguration<ConvenioNotariaVirtual>
    {
        public void Configure(EntityTypeBuilder<ConvenioNotariaVirtual> builder)
        {
            builder.ToTable("ConvenioNotariaVirtual", "Parametricas");
            builder.Property(e => e.ConvenioNotariaVirtualId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.NotariaId)
                .IsRequired();

            builder.Property(e => e.Latitud1)
                .IsRequired()
                .HasColumnType("decimal (10,6)");

            builder.Property(e => e.Latitud2)
                .IsRequired()
                .HasColumnType("decimal (10,6)");

            builder.Property(e => e.Longitud1)
                .IsRequired()
                .HasColumnType("decimal (10,6)");

            builder.Property(e => e.Longitud2)
                .IsRequired()
                .HasColumnType("decimal (10,6)");

            builder.Property(e => e.SerialCertificado)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.IdNotariaSNR)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.AutorizacionAutenticacionSNR)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.ApiUserSNR)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.ApiKeySNR)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.SerialCertificadoNotarioEncargado)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.UrlApiMiFirma)
                .HasMaxLength(512)
                .IsUnicode(false);
            builder.Property(e => e.UrlSubirDocumentosMiFirma)
                .HasMaxLength(1024)
                .IsUnicode(false);

            builder.Property(e => e.ConfigurationGuid)
                   .HasMaxLength(250)
                   .IsUnicode(false);

            builder.Property(e => e.LoginConvenio)
                   .HasMaxLength(250)
                   .IsUnicode(false);

            builder.Property(e => e.PasswordConvenio)
                   .HasMaxLength(250)
                   .IsUnicode(false);
        }
    }
}
