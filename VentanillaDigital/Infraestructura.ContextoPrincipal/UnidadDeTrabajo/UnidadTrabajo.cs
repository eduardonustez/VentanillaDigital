using Infraestructura.Nucleo.Contextos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio.Nucleo.Entidad;
using Dominio.ContextoPrincipal.Entidad;
using Infraestructura.ContextoPrincipal.Mapping;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Infraestructura.ContextoPrincipal.Migrations;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Dominio.ContextoPrincipal.Entidad.Log;

namespace Infraestructura.ContextoPrincipal.UnidadDeTrabajo
{
    public class UnidadTrabajo : ContextoBase
    {
        #region Constructor
        public UnidadTrabajo(DbContextOptions<UnidadTrabajo> options)
           : base(options)
        {
        }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnidadTrabajo).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        #region DbSet Members

        #region Tramites
        public DbSet<Tramite> Tramite { get; set; }
        public DbSet<Compareciente> Compareciente { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        #endregion

        #region Parametricas
        public DbSet<TipoIdentificacion> TiposIdentificacion { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<PersonaDatos> PersonaDatos { get; set; }
        public DbSet<NotariaCliente> NotariaCliente { get; set; }
        public DbSet<Notaria> Notaria { get; set; }
        public DbSet<Notario> Notario { get; set; }
        public DbSet<NotariaUsuarios> NotariaUsuarios { get; set; }
        public DbSet<TipoTramite> TipoTramite { get; set; }
        public DbSet<EstadoTramite> EstadoTramite { get; set; }
        public DbSet<Parametrica> Parametrica { get; set; }
        public DbSet<PlantillaActa> PlantillaActa { get; set; }
        public DbSet<ConvenioRNEC> ConveniosRNEC { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<MetadataArchivo> MetadataArchivos { get; set; }
        public DbSet<UsuarioAdministracion> UsuariosAdministracion { get; set; }
        public DbSet<DocumentoPendienteAutorizar> DocumentosPendienteAutorizar { get; set; }
        public DbSet<LogTramitePortalVirtual> LogTramitePortalVirtual { get; set; }
        public DbSet<ActoNotarial> ActosNotariales { get; set; }
        public DbSet<ActoPorTramite> ActosPorTramite { get; set; }
        public DbSet<TramitesPortalVirtual> TramitesPortalVirtual { get; set; }
        #endregion

        #endregion

    }
}
