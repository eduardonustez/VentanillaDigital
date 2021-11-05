using System;
using Aplicacion.Nucleo.Base;
using System.Threading.Tasks;
using System.Linq;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using GenericExtensions;
using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.EntityFrameworkCore;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using System.Collections.Generic;
using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Infraestructura.Transversal.Encriptacion;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;

namespace plicacion.ContextoPrincipal.Servicio
{
    public class CertificadoServicio : BaseServicio, ICertificadoServicio
    {

        #region Propiedades
        private INotariasUsuarioRepositorio _notariasUsuarioRepositorio { get; }
        private ICertificadoRepositorio _certificadoRepositorio { get; }

        #endregion

        #region Constructor
        public CertificadoServicio(
            INotariasUsuarioRepositorio notariasUsuarioRepositorio
            , ICertificadoRepositorio certificadoRepositorio) : base(notariasUsuarioRepositorio, certificadoRepositorio)
        {
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _certificadoRepositorio = certificadoRepositorio;
        }

      

        #endregion

        #region Contratos
        public async Task<SolicitudCertificadoDTO> ObtenerDatosSolicitud(string userId)
        {
           NotariaUsuarios notarioUsuario = (await _notariasUsuarioRepositorio
                .Obtener(n => n.UsuariosId == userId, 
                n => n.Persona.TipoIdentificacion,n=>n.Notaria.Ubicacion.UbicacionPadre
                ,n=>n.Notario.GrafoArchivo)).FirstOrDefault();

            if (notarioUsuario == null)
                throw new ArgumentException("El usuario no puede realizar esta solicitud");

            string dniNotarioPrincipal = "";
            string tipoDocNotarioPrincipal = "";
            if (notarioUsuario?.Notario?.TipoNotario == 1)
            {
                dniNotarioPrincipal = notarioUsuario?.Persona?.NumeroDocumento;
                tipoDocNotarioPrincipal = notarioUsuario?.Persona?.TipoIdentificacion.Abreviatura;
            }
            else
            {
                NotariaUsuarios notarioUsuarioPrincipal = (await _notariasUsuarioRepositorio
                .Obtener(n => n.NotariaId == notarioUsuario.NotariaId && (!n.Notario.IsDeleted && n.Notario.TipoNotario==1),
                n => n.Persona.TipoIdentificacion)).FirstOrDefault();
                dniNotarioPrincipal = notarioUsuarioPrincipal?.Persona.NumeroDocumento;
                tipoDocNotarioPrincipal = notarioUsuarioPrincipal?.Persona?.TipoIdentificacion.Abreviatura;
            }
            string depto = notarioUsuario?.Notaria?.Ubicacion?.UbicacionPadre?.Nombre;
            string municipio = notarioUsuario?.Notaria?.Ubicacion?.Nombre;
            return new SolicitudCertificadoDTO()
            {
                Pais="Colombia",
                Departamento = depto==null?municipio:depto,
                Municipio = municipio,
                Notaria = notarioUsuario?.Notaria?.Nombre,
                Direccion = notarioUsuario?.Notaria?.Direccion,
                DniNotarioPrincipal = dniNotarioPrincipal,
                TipoDocumentoNotarioPrincipal =tipoDocNotarioPrincipal,
                NumeroDocumento = notarioUsuario?.Persona?.NumeroDocumento,
                TipoDocumento = notarioUsuario?.Persona?.TipoIdentificacion.Abreviatura,
                Nombres = notarioUsuario?.Persona?.Nombres,
                Apellidos= notarioUsuario?.Persona?.Apellidos,
                Cargo=notarioUsuario?.Notario?.TipoNotario==1?"Notario":"Notario Encargado",
                Celular = notarioUsuario?.Persona?.NumeroCelular,
                Correo = notarioUsuario?.UserEmail,
                Firma = notarioUsuario?.Notario?.GrafoArchivo?.Contenido
            };

        }

        public async Task<string> RegistrarSolicitud(CertificadoCreateDTO solicitud)
        {
            var certificado = solicitud.Adaptar<Certificado>();
            _certificadoRepositorio.Agregar(certificado);
            _certificadoRepositorio.UnidadDeTrabajo.Commit();
            return certificado.CertificadoId.ToString();
        }

        public async Task<CertificadoSelectedDTO> ObtenerCertificadoNotario(string usuarioId)
        {
            NotariaUsuarios notarioUsuario = (await _notariasUsuarioRepositorio
              .Obtener(n => n.UsuariosId == usuarioId
              , n => n.Notario)).FirstOrDefault();
            return notarioUsuario.Adaptar<CertificadoSelectedDTO>();
        }

        public async Task ActualizarCertificadoNotario(CertificadoSelectedDTO certificadoSelected)
        {
            NotariaUsuarios notarioUsuario = (await _notariasUsuarioRepositorio
              .Obtener(n => n.UsuariosId == certificadoSelected.UsuarioId
              , n => n.Notario)).FirstOrDefault();
            notarioUsuario.Notario.Certificadoid = certificadoSelected.CertificadoId;
            _notariasUsuarioRepositorio.Modificar(notarioUsuario);
            _notariasUsuarioRepositorio.UnidadDeTrabajo.Commit();
        }
        public async Task ActualizarUsuarioNotario(CertificadoSelectedDTO certificadoSelected)
        {
            NotariaUsuarios notarioUsuario = (await _notariasUsuarioRepositorio
              .Obtener(n => n.UsuariosId == certificadoSelected.UsuarioId
              , n => n.Notario)).FirstOrDefault();
            notarioUsuario.Notario.UsuarioCertificado = certificadoSelected.UsuarioCertificado;
            _notariasUsuarioRepositorio.Modificar(notarioUsuario);
            _notariasUsuarioRepositorio.UnidadDeTrabajo.Commit();
        }
        #endregion

    }
}
