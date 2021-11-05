using Infraestructura.Nucleo.Contextos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Nucleo.Entidad;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;

namespace Infraestructura.ContextoArchivos.UnidadDeTrabajo
{
    public class UnidadTrabajoArchivos : ContextoBase
    {
        #region Constructor
        public UnidadTrabajoArchivos(DbContextOptions<UnidadTrabajoArchivos> options)
           : base(options)
        {
        }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnidadTrabajoArchivos).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        #region DbSet Members

        #region Parametricas
        #endregion

        #endregion

    }
}
