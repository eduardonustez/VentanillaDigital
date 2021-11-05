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

namespace plicacion.ContextoPrincipal.Servicio
{
    public class NotarioServicio : BaseServicio, INotarioServicio
    {
        #region Const - Mensajes
        private const string NOTARIAINCORRECTA = "El número de notaria es incorrecta";
        private const string TIPONotarioINCORRECTO = "El tipo de Notario es incorrecto";
        private const string NOMBRENotarioEXISTENTE = "Nombre de Máquina ya existe";
        private const string MACNotarioEXISTENTE = "MAC de la Máquina ya asignada a la Notaría";
        private const string ERRORSISTEMA = "Error al validar por favor intente de nuevo, si el error persiste por favor comunicarse con el administrador";

        #endregion

        #region Propiedades
        private INotarioRepositorio _NotarioRepositorio { get; }
        private INotariaRepositorio _notariaRepositorio { get; }
        private INotariasUsuarioRepositorio _NotariasUsuarioRepositorio { get; }
        private IConvenioRNECRepositorio _ConvenioRNECRepositorio { get; }
        public IProcedimientoAlmacenadoRepositorio _procedimientoAlmacenado { get; }
        public ICertificadoRepositorio _certificadoRepositorio { get; }

        #endregion

        #region Constructor
        public NotarioServicio(
            INotarioRepositorio NotarioRepositorio
            , IProcedimientoAlmacenadoRepositorio procedimientoAlmacenadoRepositorio
            , IConvenioRNECRepositorio convenioRNECRepositorio
            , INotariasUsuarioRepositorio notariasUsuarioRepositorio
            , INotariaRepositorio notariaRepositorio
            ,ICertificadoRepositorio certificadoRepositorio) : base(NotarioRepositorio
                ,convenioRNECRepositorio,notariasUsuarioRepositorio,notariaRepositorio,certificadoRepositorio)
        {
            _NotarioRepositorio = NotarioRepositorio;
            _procedimientoAlmacenado = procedimientoAlmacenadoRepositorio;
            _ConvenioRNECRepositorio = convenioRNECRepositorio;
            _NotariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _notariaRepositorio = notariaRepositorio;
            _certificadoRepositorio = certificadoRepositorio;
        }

        #endregion

        #region Contratos


        public async Task<NotarioDTO> ActualizarGrafoPinNotario(NotarioCreateDTO notarioDto)
        {

            NotarioDTO notarioReturn = new NotarioDTO();
            NotariaUsuarios notariausuario = 
                await _NotariasUsuarioRepositorio
                    .GetOneAsync(x => x.IsDeleted == false && x.UserEmail == notarioDto.Email,
                    nu=>nu.Notaria)
                    .ConfigureAwait(false);
            Notario notarioExistente = 
                await _NotarioRepositorio
                .GetOneAsync(x => x.IsDeleted == false && x.NotariaUsuariosId == notariausuario.NotariaUsuariosId,
                no=>no.GrafoArchivo)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(notarioDto.Grafo))
            {
                if (notarioExistente.GrafoId != null)
                {
                    notarioExistente.GrafoArchivo.FechaModificacion = DateTime.Now;
                    notarioExistente.GrafoArchivo.UsuarioModificacion = notarioDto.Email;
                    notarioExistente.GrafoArchivo.MapDataUrl(notarioDto.Grafo);
                }
                else
                {
                    notarioExistente.GrafoArchivo = GrafoNotario.FromDataUrl(
                        notarioDto.Grafo,
                        nombre: $"{notarioExistente.NotarioId}_{notarioDto.Email}",
                        ruta: $"grafos/{notariausuario.NotariaId}_{notariausuario.Notaria.Nombre}/");
                }
            }
            if (notarioDto.Pin != null && notarioDto.Pin != "")
            {
                notarioExistente.Pin = notarioDto.Pin;
            }

            int IdResult = await _NotarioRepositorio.ActualizarNotario(notarioExistente).ConfigureAwait(false);

            if (IdResult > 0)
            {
                notarioReturn.Grafo = notarioExistente.GrafoArchivo?.ToDataUrl();

                notarioReturn.NotarioId = notarioExistente.NotarioId;
                notarioReturn.Nit = notarioExistente.Nit;
                notarioReturn.NotariaUsuariosId = notarioExistente.NotariaUsuariosId;
                notarioReturn.NotarioId = notarioExistente.NotarioId;
                notarioReturn.Pin = notarioExistente.Pin;
                notarioReturn.TipoNotario = notarioExistente.TipoNotario;
            }

            return notarioReturn;
        }

        public async Task<EstadoPinFirmaDTO> ObtenerEstadoPinFirma(string email)
        {
            EstadoPinFirmaDTO resul = new EstadoPinFirmaDTO();
            NotariaUsuarios Notariausuario =
                (await _NotariasUsuarioRepositorio
                        .Obtener(x => x.IsDeleted == false && x.UserEmail == email,
                        nu => nu.Notario)
                        .ConfigureAwait(false))
                        .FirstOrDefault();
            resul.FirmaRegistrada = Notariausuario?.Notario?.GrafoId != null;
            resul.PinAsignado = Notariausuario?.Notario?.Pin != null;
            resul.CertificadoSolicitado = (await _certificadoRepositorio.Obtener(c => c.UsuarioId == Notariausuario.UsuariosId)).Any();
            return resul;

        }
        //public async Task<OpcionesConfiguracioNotarioDTO> ObtenerOpcionesConfiguracion(string email)
        //{
        //    OpcionesConfiguracioNotarioDTO resul = new OpcionesConfiguracioNotarioDTO();
        //    NotariaUsuarios Notariausuario =
        //        (await _NotariasUsuarioRepositorio
        //        .Obtener(x => x.IsDeleted == false && x.UserEmail == email,
        //        x => x.Notario)
        //        .ConfigureAwait(false))
        //        .FirstOrDefault();
        //    resul.UsarSticker = Notariausuario.Notario.UsarSticker;
        //    return resul;
        //}

        public Task<string> ObtenerGrafo(string email)
        {
            return _NotariasUsuarioRepositorio.ObtenerTodo()
                .Where(nu => nu.IsDeleted == false && nu.UserEmail == email)
                .Select(nu => nu.Notario.GrafoArchivo.ToDataUrl())
                .FirstOrDefaultAsync();
        }

        //public async Task SeleccionarFormatoImpresion(SeleccionarFormatoImpresionDTO seleccionFormato)
        //{
        //    var notariaUsuario = (await _NotariasUsuarioRepositorio
        //        .Obtener(n => n.UsuariosId == seleccionFormato.UserId,
        //        nu => nu.Notario))
        //        .FirstOrDefault();
        //    if (notariaUsuario == null || notariaUsuario.Notario == null)
        //    {
        //        throw new ArgumentException("Notario no existe.");
        //    }
        //    else
        //    {
        //        notariaUsuario.Notario.UsarSticker = seleccionFormato.UsarSticker;
        //        _NotariasUsuarioRepositorio.Modificar(notariaUsuario);
        //        _NotariasUsuarioRepositorio.UnidadDeTrabajo.Commit();

        //    }

        //}

        public async Task<IEnumerable<NotarioReturnDTO>> ObtenerNotariosNotaria(long NotariaId)
        {
            var notarios = (await _NotarioRepositorio.Obtener(
                n => n.NotariaUsuarios.NotariaId == NotariaId,
                n => n.NotariaUsuarios.Persona
                )).ToList();

            Notaria notaria = _notariaRepositorio.Obtener(NotariaId);
            if (notaria.NotarioEnTurno == null)
            {
                notaria.NotarioEnTurno = 0;
            }

            var notariosDTO = notarios.Select(n => n.Adaptar<NotarioReturnDTO>()).ToList();
            
            foreach(var n in notariosDTO)
            {
                if (n.NotarioId == notaria.NotarioEnTurno)
                {
                    n.NotarioDeTurno = true;
                }
            }
            return notariosDTO;

        }
        public async Task<long> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO)
        {
            Notaria notaria = await _notariaRepositorio.ObtenerAsync(notarioNotariaDTO.NotariaId);

            notaria.NotarioEnTurno = notarioNotariaDTO.NotarioId;
            _notariaRepositorio.Modificar(notaria);
            _notariaRepositorio.UnidadDeTrabajo.Commit();
            return notarioNotariaDTO.NotarioId;
        }

        public async Task<bool> ValidarSolicitudPin(ValSolicitudPinDTO valSolicitudPinDTO)
        {
            var notariaUsuario =
               (await _NotariasUsuarioRepositorio.Obtener(
                      t => t.IsDeleted == false &&
                      t.UserEmail == valSolicitudPinDTO.UserName,
                      nu => nu.Notario
                   )).FirstOrDefault();
            string pin = notariaUsuario?.Notario?.Pin;
            if (string.IsNullOrWhiteSpace(pin))
                return false;
            //return CifradoSHA512.CompararTexto($"{pin}20210122", valSolicitudPinDTO.Clave);
            return CifradoSHA512.CompararTexto($"{pin}{DateTime.Now.ToString("yyyyMMdd")}",valSolicitudPinDTO.Clave);
        }
        public async Task<bool> EsPinValido(ValSolicitudPinDTO valSolicitudPinDTO)
        {
            var notariaUsuario =
               (await _NotariasUsuarioRepositorio.Obtener(
                      t => t.IsDeleted == false &&
                      t.UserEmail == valSolicitudPinDTO.UserName,
                      nu => nu.Notario
                   )).FirstOrDefault();
            string pin = notariaUsuario?.Notario?.Pin;
            if (string.IsNullOrWhiteSpace(pin))
                return false;
            return pin==valSolicitudPinDTO.Clave;
        }

        #endregion

        #region Negocio




        #endregion
    }
}
