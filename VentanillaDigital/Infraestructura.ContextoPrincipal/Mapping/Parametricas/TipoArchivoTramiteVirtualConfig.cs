using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class TipoArchivoTramiteVirtualConfig : IEntityTypeConfiguration<TipoArchivoTramiteVirtual>
    {
        public void Configure(EntityTypeBuilder<TipoArchivoTramiteVirtual> builder)
        {
            builder.ToTable("TipoArchivoTramiteVirtual", "Parametricas");
            builder.HasKey(e => e.TipoArchivoTramiteVirtualId);
            builder.Property(x => x.TipoArchivoTramiteVirtualId).ValueGeneratedNever();

            builder.Property(e => e.Nombre)
                .HasMaxLength(150).IsRequired();

            builder.Property(e => e.Descripcion)
                .HasMaxLength(300).IsRequired();
        }
    }
}
