using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class TramiteConfig : IEntityTypeConfiguration<Tramite>
    {
        public void Configure(EntityTypeBuilder<Tramite> builder)
        {
            builder.ToTable("Tramites", "Transaccional");
            builder.Property(e => e.TramiteId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CantidadComparecientes)
                .IsRequired();


            builder.Property(e => e.Fecha)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.DatosAdicionales)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(e => e.Notaria)
            .WithMany()
             .HasForeignKey(e => e.NotariaId)
            .HasConstraintName("FK_Tramites_Notarias")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EstadoTramite)
            .WithMany()
             .HasForeignKey(e => e.EstadoTramiteId)
            .HasConstraintName("FK_Tramites_EstadosTramites")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoTramite)
            .WithMany() 
             .HasForeignKey(e => e.TipoTramiteId)
            .HasConstraintName("FK_Tramites_TiposTramites")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ActaNotarial)
          .WithOne()
           .HasForeignKey<Tramite>(e => e.ActaNotarialId)
          .HasConstraintName("FK_Tramites_MetadataActaNotarial")
          .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => new { t.IsDeleted, t.NotariaId });
            builder.HasIndex(t => new { t.IsDeleted, t.NotariaId, t.EstadoTramiteId })
                .IncludeProperties(t => new { t.TramiteId, t.FechaCreacion, t.CantidadComparecientes, t.TipoTramiteId });
            builder.HasIndex(t => new { t.IsDeleted, t.NotariaId, t.FechaCreacion });
            builder.HasIndex(t => new { t.IsDeleted, t.NotariaId, t.EstadoTramiteId, t.FechaCreacion })
                .IncludeProperties(t => new { t.TramiteId, t.CantidadComparecientes, t.TipoTramiteId });
        }
    }
}
