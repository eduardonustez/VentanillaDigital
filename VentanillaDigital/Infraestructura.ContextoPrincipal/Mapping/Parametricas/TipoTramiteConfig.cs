using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Mapping
{
    public class TipoTramiteConfig : IEntityTypeConfiguration<TipoTramite>
    {
        public void Configure(EntityTypeBuilder<TipoTramite> builder)
        {
            builder.ToTable("TiposTramites", "Parametricas");
            builder.Property(e => e.TipoTramiteId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(250);
            
            builder.Property(e => e.Descripcion)
                .HasMaxLength(250);

            builder.Property(e => e.CodigoTramite)
                .IsRequired();

            builder.HasOne(e => e.Categoria)
            .WithMany(c=>c.TiposTramites)
             .HasForeignKey(e => e.CategoriaId)
            .HasConstraintName("FK_TiposTramites_Categorias")
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PlantillaActa)
            .WithMany(p=>p.TiposTramites)
             .HasForeignKey(e => e.PlantillaActaId)
            .HasConstraintName("FK_TiposTramites_PlantillasActas")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PlantillaSticker)
            .WithMany()
             .HasForeignKey(e => e.PlantillaStickerId)
            .HasConstraintName("FK_TiposTramites_PlantillasStickers")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
