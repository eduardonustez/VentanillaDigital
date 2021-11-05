using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.ContextoPrincipal.Mapping.Parametricas
{
    public class UsuarioAdministracionConfig : IEntityTypeConfiguration<UsuarioAdministracion>
    {
        public void Configure(EntityTypeBuilder<UsuarioAdministracion> builder)
        {
            builder.ToTable("UsuariosAdministracion", "Parametricas");
            builder.Property(e => e.UsuarioAdministracionId)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Login)
                .HasMaxLength(150)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(m => m.Password)
                .IsUnicode(false);

            builder.Property(m => m.Rol)
                .HasMaxLength(150)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}
