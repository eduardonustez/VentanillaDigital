using Dominio.ContextoPrincipal.Entidad.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Log
{
    public class LogTramitePortalVirtualConfig : IEntityTypeConfiguration<LogTramitePortalVirtual>
    {
        public void Configure(EntityTypeBuilder<LogTramitePortalVirtual> builder)
        {
            builder.ToTable("LogTramitePortalVirtual", "Log");
            builder.HasKey(e => e.LogTramiteVirtualPortalId);
            builder.Property(e => e.ClaveTestamentoCerrado)
                .HasMaxLength(200)
                .IsUnicode(false);
            builder.Property(e => e.Lat)
                .HasColumnType("decimal(9,6)")
                .IsRequired();
            builder.Property(e => e.Lng)
                .HasColumnType("decimal(9,6)")
                .IsRequired();
            builder.Property(e => e.LogResponseSNR)
                .IsUnicode(false);
        }
    }
}
