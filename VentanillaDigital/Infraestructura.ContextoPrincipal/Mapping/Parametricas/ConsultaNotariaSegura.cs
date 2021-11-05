using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class ConsultaNotariaSeguraConfig : IEntityTypeConfiguration<ConsultaNotariaSegura>
    {
        public void Configure(EntityTypeBuilder<ConsultaNotariaSegura> builder)
        {
            builder.ToTable("ConsultaNotariaSegura", "Parametricas");
            builder.Property(e => e.ConsultaNotariaSeguraId).ValueGeneratedOnAdd();
            builder.Property(e => e.TramiteId).IsRequired();
            builder.Property(e => e.TramiteIdHash).IsRequired();
            builder.Property(e => e.NotariaId).IsRequired().HasColumnType("INTEGER");
            builder.Property(e => e.Email);
            builder.Property(e => e.FechaConsulta).IsRequired();
            builder.Property(e => e.EncontroArchivo).IsRequired();
        }
    }
}